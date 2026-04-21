using System.Device.Gpio;
using Core;
using Core.Definitions;

namespace Pi_Sequencer_Input;

class Program
{
    private static readonly Dictionary<int, bool> PressedButtonsByIndex = new()
    {
        { 0, false },
        { 1, false },
        { 2, false },
        { 3, false },
        { 4, false },
        { 5, false },
        { 6, false },
        { 7, false },
        { 8, false },
        { 9, false },
        { 10, false },
        { 11, false },
        { 12, false },
        { 13, false },
        { 14, false },
        { 15, false }
    };

    static void Main()
    {
        using var sender = new Sender();

        const int s0Pin = 2; // Select/Address pin
        const int s1Pin = 3; // Select/Address pin
        const int s2Pin = 4; // Select/Address pin
        const int s3Pin = 17; // Select/Address pin
        const int sigPin = 27; // Common input or output.


        using var controller = new GpioController();
        controller.OpenPin(s0Pin, PinMode.Output);
        controller.OpenPin(s1Pin, PinMode.Output);
        controller.OpenPin(s2Pin, PinMode.Output);
        controller.OpenPin(s3Pin, PinMode.Output);
        controller.OpenPin(sigPin, PinMode.InputPullDown);


        Console.WriteLine("Pi-Sequencer-Input. Press Ctrl+C to exit.");

        while (true)
        {
            for (int i = 0; i < 16; i++)
            {
                var isS0PinOn = (i & 1) != 0;
                var isS1PinOn = (i & 2) != 0;
                var isS2PinOn = (i & 4) != 0;
                var isS3PinOn = (i & 8) != 0;

                controller.Write(s0Pin, isS0PinOn ? PinValue.High : PinValue.Low);
                controller.Write(s1Pin, isS1PinOn ? PinValue.High : PinValue.Low);
                controller.Write(s2Pin, isS2PinOn ? PinValue.High : PinValue.Low);
                controller.Write(s3Pin, isS3PinOn ? PinValue.High : PinValue.Low);

                Thread.Sleep(1);

                var isToggled = controller.Read(sigPin);

                if (!PressedButtonsByIndex[i] && isToggled == PinValue.High)
                {
                    PressedButtonsByIndex[i] = true;
                    Console.WriteLine($"{i} is pressed");
                    sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, i);
                }

                if (PressedButtonsByIndex[i] && isToggled == PinValue.Low)
                {
                    PressedButtonsByIndex[i] = false;
                    Console.WriteLine($"{i} is released");
                    sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, i);
                }
            }
        }
    }
}