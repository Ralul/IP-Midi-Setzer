namespace debuger.Models;

public class StopModel
{
    public int NoteToEnable { get; set; }
    public int NoteToDisable { get; set; }
    
    public bool IsSolenoidToEnableStopOn
    {
        get;
        set
        {
            IsStopOn = true;
            field = value;
        }
    }

    public bool IsSolenoidToDisableStopOn
    {
        get;
        set
        {
            IsStopOn = true;
            field = value;
        }
    }
    public bool IsStopOn { get; private set; }

    public StopModel(int noteToEnable, int noteToDisable)
    {
        NoteToEnable = noteToEnable;
        NoteToDisable = noteToDisable;
    }
}