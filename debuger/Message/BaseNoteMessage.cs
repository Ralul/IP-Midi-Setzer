using Core;

namespace debuger.Message;

public class NoteMessage
{
    public MessageState MessageState { get; }
    public NoteType NoteType { get; }
    public int Channel { get; }
    public int Note { get; }
    public int Velocity { get; }

    public NoteMessage(int channel, int note, int? velocity,NoteType noteType, MessageState messageState)
    {
        Channel = channel;
        Note = note;
        Velocity = NoteType == NoteType.On ? velocity.GetValueOrDefault() : 127;
        NoteType = noteType;
        MessageState = messageState;
    }

    public NoteMessage(NoteEventArgs noteArgs, NoteType noteType, MessageState messageState)
    {
        Channel = noteArgs.Channel;
        Note = noteArgs.Note;
        Velocity = noteArgs.Velocity;
        NoteType = noteType;
        MessageState = messageState;
    }

    public override string ToString()
    {
        return $"Chanel: {Channel}, Note: {Note}, Velocity: {Velocity}, NoteType: {NoteType},  MessageState: {MessageState}";
    }
}

public enum MessageState
{
    Receiveed,
    Sending
}

public enum NoteType
{
    On,
    Off
}