namespace Core;

public class NoteEventArgs : EventArgs
{
    public int Channel { get; }
    public int Note { get; }
    public int Velocity { get; }

    public NoteEventArgs(int channel, int note, int velocity)
    {
        Channel = channel;
        Note = note;
        Velocity = velocity;
    }
}