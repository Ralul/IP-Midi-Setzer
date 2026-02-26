using System.Net;
using System.Net.Sockets;

namespace Core;

/// <summary>
/// Receives MIDI messages over a network using the ipMIDI protocol.
/// Listens on a UDP multicast group and raises events per message type.
/// </summary>
public class Receiver : IDisposable
{
    private UdpClient? _udpClient;
    private CancellationTokenSource? _cts;
    private bool _disposed;

    public string MulticastAddress { get; }
    public int Port { get; }
    public bool IsListening { get; private set; }

    public const string DefaultMulticastAddress = "225.0.0.37";
    public const int DefaultPort = 21928;

    // Raised for every incoming UDP packet (raw bytes)
    public event EventHandler<MidiMessageEventArgs>? RawMessageReceived;

    // Raised for parsed MIDI message types
    public event EventHandler<NoteEventArgs>? NoteOn;
    public event EventHandler<NoteEventArgs>? NoteOff;
    public event EventHandler<ControlChangeEventArgs>? ControlChange;
    public event EventHandler<ProgramChangeEventArgs>? ProgramChange;
    public event EventHandler<PitchBendEventArgs>? PitchBend;
    public event EventHandler<AftertouchEventArgs>? Aftertouch;

    // Raised when a packet could not be parsed
    public event EventHandler<MidiMessageEventArgs>? UnknownMessageReceived;

    public Receiver(
        string multicastAddress = DefaultMulticastAddress,
        int port = DefaultPort
    )
    {
        MulticastAddress = multicastAddress;
        Port = port;
    }

    /// <summary>
    /// Starts listening for ipMIDI packets asynchronously.
    /// </summary>
    public void Start()
    {
        if (IsListening)
            throw new InvalidOperationException("Receiver is already listening.");

        _udpClient = new UdpClient();
        _udpClient.Client.SetSocketOption(
            SocketOptionLevel.Socket,
            SocketOptionName.ReuseAddress,
            true
        );
        _udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, Port));
        _udpClient.JoinMulticastGroup(IPAddress.Parse(MulticastAddress));

        _cts = new CancellationTokenSource();
        IsListening = true;

        Task.Run(() => ListenAsync(_cts.Token));
    }

    /// <summary>
    /// Stops listening for ipMIDI packets.
    /// </summary>
    public void Stop()
    {
        if (!IsListening) return;

        _cts?.Cancel();
        _udpClient?.DropMulticastGroup(IPAddress.Parse(MulticastAddress));
        _udpClient?.Close();
        IsListening = false;
    }

    private async Task ListenAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                UdpReceiveResult result = await _udpClient!.ReceiveAsync(ct);
                byte[] data = result.Buffer;

                RawMessageReceived?.Invoke(this, new MidiMessageEventArgs(data));
                ParseAndDispatch(data);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (SocketException)
            {
                // Socket was closed externally (e.g. Stop() was called)
                break;
            }
        }
    }

    private void ParseAndDispatch(byte[] data)
    {
        int i = 0;

        while (i < data.Length)
        {
            byte status = data[i];

            // Ignore real-time / sysex for now
            if (status < 0x80)
            {
                i++;
                continue;
            }

            int type = status & 0xF0;
            int channel = status & 0x0F;

            switch (type)
            {
                case 0x80 when i + 2 < data.Length: // Note Off
                    NoteOff?.Invoke(
                        this,
                        new NoteEventArgs(channel, data[i + 1], data[i + 2])
                    );
                    i += 3;
                    break;

                case 0x90 when i + 2 < data.Length: // Note On
                    // Velocity 0 is conventionally a Note Off
                    if (data[i + 2] == 0)
                        NoteOff?.Invoke(
                            this,
                            new NoteEventArgs(channel, data[i + 1], 0)
                        );
                    else
                        NoteOn?.Invoke(
                            this,
                            new NoteEventArgs(channel, data[i + 1], data[i + 2])
                        );
                    i += 3;
                    break;

                case 0xA0 when i + 2 < data.Length: // Polyphonic Aftertouch (skip)
                    i += 3;
                    break;

                case 0xB0 when i + 2 < data.Length: // Control Change
                    ControlChange?.Invoke(
                        this,
                        new ControlChangeEventArgs(channel, data[i + 1], data[i + 2])
                    );
                    i += 3;
                    break;

                case 0xC0 when i + 1 < data.Length: // Program Change
                    ProgramChange?.Invoke(
                        this,
                        new ProgramChangeEventArgs(channel, data[i + 1])
                    );
                    i += 2;
                    break;

                case 0xD0 when i + 1 < data.Length: // Channel Aftertouch
                    Aftertouch?.Invoke(
                        this,
                        new AftertouchEventArgs(channel, data[i + 1])
                    );
                    i += 2;
                    break;

                case 0xE0 when i + 2 < data.Length: // Pitch Bend
                    int raw = data[i + 1] | (data[i + 2] << 7);
                    PitchBend?.Invoke(
                        this,
                        new PitchBendEventArgs(channel, raw - 8192)
                    );
                    i += 3;
                    break;

                default:
                    UnknownMessageReceived?.Invoke(
                        this,
                        new MidiMessageEventArgs(data)
                    );
                    i++;
                    break;
            }
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        Stop();
        _cts?.Dispose();
        _udpClient?.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}