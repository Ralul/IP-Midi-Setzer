using Core;

namespace debuger.Message;

public class NoteOffMessage : BaseNoteMessage
{
    public NoteOffMessage(int channel, int note, int velocity) : base(channel, note, velocity)
    {
    }

    public NoteOffMessage(NoteEventArgs noteArgs) : base(noteArgs)
    {
    }
}