namespace Core;

public class Stop : ICloneable
{
    private readonly Sender _sender;
    
    public int NoteToEnable { get; }
    public int NoteToDisable { get; }
    public int Channel { get; }
    
    public bool IsEnabled {get; set;}
    
    public Stop(int noteToEnable, int noteToDisable, int channel,  Sender sender)
    {
        NoteToEnable = noteToEnable;
        NoteToDisable = noteToDisable;
        Channel = channel;
        
        _sender = sender;
    }
    
    public async Task EnableStop()
    {
        IsEnabled = true;
        _sender.SendNoteOn(Channel, NoteToEnable);
        await Task.Delay(500);
        _sender.SendNoteOff(Channel, NoteToEnable);
    }
    
    public async Task DisableStop()
    {
        IsEnabled = false;
        _sender.SendNoteOn(Channel, NoteToDisable);
        await Task.Delay(500);
        _sender.SendNoteOff(Channel, NoteToDisable);
    }

    public object Clone()
    {
        var cloneStop = new Stop(NoteToEnable, NoteToDisable, Channel, _sender)
        {
            IsEnabled = IsEnabled
        };
        return cloneStop;
    }
}