using System.Net;
using System.Net.Sockets;

namespace Core;

/// <summary>
/// Sends MIDI messages over a network using the ipMIDI protocol.
/// ipMIDI transmits raw MIDI data via UDP multicast (default: 225.0.0.37:21928).
/// </summary>
public class Sender : IDisposable
{
    private readonly UdpClient _udpClient;
    private readonly IPEndPoint _endPoint;
    private bool _disposed;

    public string MulticastAddress { get; }
    public int Port { get; }

    // Default ipMIDI multicast address and port
    public const string DefaultMulticastAddress = "225.0.0.37";
    public const int DefaultPort = 21928;

    public Sender(string multicastAddress = DefaultMulticastAddress, int port = DefaultPort)
    {
        MulticastAddress = multicastAddress;
        Port = port;
        
        _udpClient = new UdpClient();
        
        _endPoint = new IPEndPoint(IPAddress.Parse(multicastAddress), port);
    }

    /// <summary>
    /// Sends a raw MIDI message.
    /// </summary>
    public void Send(byte[] midiData)
    {
        if (midiData == null || midiData.Length == 0)
            throw new ArgumentException("MIDI data cannot be null or empty.", nameof(midiData));

        _udpClient.Send(midiData, midiData.Length, _endPoint);
    }

    /// <summary>
    /// Sends a Note On message.
    /// </summary>
    /// <param name="channel">MIDI channel (0-15)</param>
    /// <param name="note">Note number (0-127)</param>
    /// <param name="velocity">Velocity (0-127)</param>
    public void SendNoteOn(int channel, int note, int velocity)
    {
        ValidateRange(channel, 0, 15, nameof(channel));
        ValidateRange(note, 0, 127, nameof(note));
        ValidateRange(velocity, 0, 127, nameof(velocity));

        Send([(byte)(0x90 | channel), (byte)note, (byte)velocity]);
    }

    /// <summary>
    /// Sends a Note Off message.
    /// </summary>
    /// <param name="channel">MIDI channel (0-15)</param>
    /// <param name="note">Note number (0-127)</param>
    /// <param name="velocity">Release velocity (0-127)</param>
    public void SendNoteOff(int channel, int note, int velocity = 0)
    {
        ValidateRange(channel, 0, 15, nameof(channel));
        ValidateRange(note, 0, 127, nameof(note));
        ValidateRange(velocity, 0, 127, nameof(velocity));

        Send([(byte)(0x80 | channel), (byte)note, (byte)velocity]);
    }

    /// <summary>
    /// Sends a Control Change message.
    /// </summary>
    /// <param name="channel">MIDI channel (0-15)</param>
    /// <param name="controller">Controller number (0-127)</param>
    /// <param name="value">Controller value (0-127)</param>
    public void SendControlChange(int channel, int controller, int value)
    {
        ValidateRange(channel, 0, 15, nameof(channel));
        ValidateRange(controller, 0, 127, nameof(controller));
        ValidateRange(value, 0, 127, nameof(value));

        Send([(byte)(0xB0 | channel), (byte)controller, (byte)value]);
    }

    /// <summary>
    /// Sends a Program Change message.
    /// </summary>
    /// <param name="channel">MIDI channel (0-15)</param>
    /// <param name="program">Program number (0-127)</param>
    public void SendProgramChange(int channel, int program)
    {
        ValidateRange(channel, 0, 15, nameof(channel));
        ValidateRange(program, 0, 127, nameof(program));

        Send([(byte)(0xC0 | channel), (byte)program]);
    }

    /// <summary>
    /// Sends a Pitch Bend message.
    /// </summary>
    /// <param name="channel">MIDI channel (0-15)</param>
    /// <param name="value">Pitch bend value (-8192 to 8191)</param>
    public void SendPitchBend(int channel, int value)
    {
        ValidateRange(channel, 0, 15, nameof(channel));
        ValidateRange(value, -8192, 8191, nameof(value));

        int adjusted = value + 8192;
        byte lsb = (byte)(adjusted & 0x7F);
        byte msb = (byte)((adjusted >> 7) & 0x7F);

        Send([(byte)(0xE0 | channel), lsb, msb]);
    }

    /// <summary>
    /// Sends an Aftertouch (Channel Pressure) message.
    /// </summary>
    /// <param name="channel">MIDI channel (0-15)</param>
    /// <param name="pressure">Pressure value (0-127)</param>
    public void SendAftertouch(int channel, int pressure)
    {
        ValidateRange(channel, 0, 15, nameof(channel));
        ValidateRange(pressure, 0, 127, nameof(pressure));

        Send([(byte)(0xD0 | channel), (byte)pressure]);
    }

    private static void ValidateRange(int value, int min, int max, string paramName)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(
                paramName,
                $"Value must be between {min} and {max}, but was {value}."
            );
    }

    public void Dispose()
    {
        if (_disposed) return;
        _udpClient.DropMulticastGroup(IPAddress.Parse(MulticastAddress));
        _udpClient.Close();
        _udpClient.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}