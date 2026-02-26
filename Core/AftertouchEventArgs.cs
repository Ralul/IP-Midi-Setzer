namespace Core;

public class AftertouchEventArgs : EventArgs
{
    public int Channel { get; }
    public int Pressure { get; }

    public AftertouchEventArgs(int channel, int pressure)
    {
        Channel = channel;
        Pressure = pressure;
    }
}