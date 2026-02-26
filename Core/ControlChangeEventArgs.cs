namespace Core;

public class ControlChangeEventArgs : EventArgs
{
    public int Channel { get; }
    public int Controller { get; }
    public int Value { get; }

    public ControlChangeEventArgs(int channel, int controller, int value)
    {
        Channel = channel;
        Controller = controller;
        Value = value;
    }
}