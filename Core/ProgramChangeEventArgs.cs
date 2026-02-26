namespace Core;

public class ProgramChangeEventArgs : EventArgs
{
    public int Channel { get; }
    public int Program { get; }

    public ProgramChangeEventArgs(int channel, int program)
    {
        Channel = channel;
        Program = program;
    }
}