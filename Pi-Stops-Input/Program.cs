using System.Device.Gpio;
using Core;
using Core.Definitions;
using DotNetEnv;

namespace Pi_Stops_Input;

class Program
{
    static void Main()
    {
        Env.Load();
        using var sender = new Sender(isDveModeOn: true);
        using var senderSecond = new Sender();

        int s0Pin = 2; // Select/Address pin
        int s1Pin = 3; // Select/Address pin
        int s2Pin = 4; // Select/Address pin
        int s3Pin = 17; // Select/Address pin
        int sigPin = 27; // Common input or output.

        var pinMode = Environment.GetEnvironmentVariable("PIN_MODE");

        if (pinMode != null && pinMode.Equals("sequencer", StringComparison.CurrentCultureIgnoreCase))
        {
            s0Pin = 2;
            s1Pin = 3;
            s2Pin = 4;
            s3Pin = 17;
            sigPin = 27;
        }
        else if (pinMode != null && pinMode.Equals("stop-toggles", StringComparison.CurrentCultureIgnoreCase))
        {
            s0Pin = 22;
            s1Pin = 10;
            s2Pin = 9;
            s3Pin = 11;
            sigPin = 0;
        }


        using GpioController controller = new GpioController();
        controller.OpenPin(s0Pin, PinMode.Output);
        controller.OpenPin(s1Pin, PinMode.Output);
        controller.OpenPin(s2Pin, PinMode.Output);
        controller.OpenPin(s3Pin, PinMode.Output);
        controller.OpenPin(sigPin, PinMode.InputPullDown);

        var pressedButtonsByIndex = new Dictionary<int, bool>
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

        Console.WriteLine("Pi-Stops-Input. Press Ctrl+C to exit.");
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

                if (!pressedButtonsByIndex[i] && isToggled == PinValue.High)
                {
                    pressedButtonsByIndex[i] = true;
                    Console.WriteLine($"{i} is pressed");

                    if (pinMode != null && pinMode.Equals("stop-toggles", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, i + 25);
                    }
                    else
                    {
                        sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, i + 25);
                    }
                }

                if (pressedButtonsByIndex[i] && isToggled == PinValue.Low)
                {
                    pressedButtonsByIndex[i] = false;
                    Console.WriteLine($"{i} is released");

                    if (pinMode != null && pinMode.Equals("stop-toggles", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, i + 25);
                    }
                    else
                    {
                        sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, i + 25);
                    }
                }
            }
        }
    }
}

// wenn regeister wippe gedrüpck magent einschalten und wider ausacheltne (note on und wider note off)
// für sequender gilt das nicht da dir bisheirge lokeik überneiemn ohne offset von 25
// für sequender muss sender in dev mode sein
// für stops muss sender in deve mode sein und ien zusäelchen der normal ist