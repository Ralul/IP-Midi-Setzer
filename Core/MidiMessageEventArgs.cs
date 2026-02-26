namespace Core;

public class MidiMessageEventArgs : EventArgs
{
    public byte[] RawData { get; }
    public DateTime ReceivedAt { get; }

    public MidiMessageEventArgs(byte[] rawData)
    {
        RawData = rawData;
        ReceivedAt = DateTime.UtcNow;
    }
}