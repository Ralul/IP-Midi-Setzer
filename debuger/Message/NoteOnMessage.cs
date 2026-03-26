using Core;

namespace debuger.Message;

public class NoteOnMessage : BaseNoteMessage
{
    public NoteOnMessage(int channel, int note, int velocity) : base(channel, note, velocity)
    {
    }

    public NoteOnMessage(NoteEventArgs noteArgs) : base(noteArgs)
    {
    }
}