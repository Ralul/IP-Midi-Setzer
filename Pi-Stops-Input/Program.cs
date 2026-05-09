using System.Device.Gpio;
using Core;
using Core.Definitions;

namespace Pi_Stops_Input;

class Program
{
    private static readonly bool[] StopStates = new bool[16];

    static void Main()
    {
        using var sender = new Sender();
        using var controller = new GpioController();

        const int s0Pin = 22;
        const int s1Pin = 10;
        const int s2Pin = 9;
        const int s3Pin = 11;
        const int sigPin = 0;

        controller.OpenPin(s0Pin, PinMode.Output);
        controller.OpenPin(s1Pin, PinMode.Output);
        controller.OpenPin(s2Pin, PinMode.Output);
        controller.OpenPin(s3Pin, PinMode.Output);
        controller.OpenPin(sigPin, PinMode.InputPullDown);

        Console.WriteLine("Pi-Stops-Input. Press Ctrl+C to exit.");

        while (true)
        {
            for (int i = 0; i < 16; i++)
            {
                controller.Write(s0Pin, (i & 1) != 0 ? PinValue.High : PinValue.Low);
                controller.Write(s1Pin, (i & 2) != 0 ? PinValue.High : PinValue.Low);
                controller.Write(s2Pin, (i & 4) != 0 ? PinValue.High : PinValue.Low);
                controller.Write(s3Pin, (i & 8) != 0 ? PinValue.High : PinValue.Low);

                Thread.Sleep(1);

                bool currentState = controller.Read(sigPin) == PinValue.High;

                if (currentState != StopStates[i])
                {
                    Console.WriteLine($"Stop {i} toggled -> {(currentState ? "ON" : "OFF")}");

                    int baseNote = i * 2 + 24;
                    int note = currentState ? baseNote : baseNote + 1;

                    sender.SendNoteOn(SequencerDefinition.CHANEL_STOPS_1_126, note);
                    Task.Delay(100).Wait();
                    sender.SendNoteOff(SequencerDefinition.CHANEL_STOPS_1_126, note);

                    StopStates[i] = currentState;
                }
            }
        }
    }
}