namespace Core;

public class PitchBendEventArgs : EventArgs
{
    public int Channel { get; }
    /// <summary>Pitch bend value in range -8192 to 8191</summary>
    public int Value { get; }

    public PitchBendEventArgs(int channel, int value)
    {
        Channel = channel;
        Value = value;
    }
}