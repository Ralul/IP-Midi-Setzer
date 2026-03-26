using Core;

namespace debuger.Message;

public abstract class BaseNoteMessage
{
    public int Channel { get; }
    public int Note { get; }
    public int Velocity { get; }
    
    public BaseNoteMessage(int channel, int note, int velocity)
    {
        Channel = channel;
        Note = note;
        Velocity = velocity;
    }

    public BaseNoteMessage(NoteEventArgs noteArgs)
    {
        Channel = noteArgs.Channel;
        Note = noteArgs.Note;
        Velocity = noteArgs.Velocity;
    }
}