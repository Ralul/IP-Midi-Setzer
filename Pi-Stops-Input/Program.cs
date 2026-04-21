using System.Device.Gpio;
using Core;
using Core.Definitions;

namespace Pi_Stops_Input;

class Program
{
    private static readonly Dictionary<int, bool> StopsStatedByIndex = new()
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

        const int s0Pin = 22; // Select/Address pin
        const int s1Pin = 10; // Select/Address pin
        const int s2Pin = 9; // Select/Address pin
        const int s3Pin = 11; // Select/Address pin
        const int sigPin = 0; // Common input or output.


        using var controller = new GpioController();
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
                var isS0PinOn = (i & 1) != 0;
                var isS1PinOn = (i & 2) != 0;
                var isS2PinOn = (i & 4) != 0;
                var isS3PinOn = (i & 8) != 0;

                controller.Write(s0Pin, isS0PinOn ? PinValue.High : PinValue.Low);
                controller.Write(s1Pin, isS1PinOn ? PinValue.High : PinValue.Low);
                controller.Write(s2Pin, isS2PinOn ? PinValue.High : PinValue.Low);
                controller.Write(s3Pin, isS3PinOn ? PinValue.High : PinValue.Low);

                Thread.Sleep(1);

                var stopHasChanged = !controller.Read(sigPin).Equals((PinValue)(object?)StopsStatedByIndex[i]);


                if (stopHasChanged)
                {
                    Console.WriteLine($"{i} is toggled");

                    if (StopsStatedByIndex[i])
                    {
                        sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, i * 2 + 25);
                        Task.Delay(100).Wait();
                        sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, i * 2 + 25);
                    }else
                    {
                        sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, i * 2 + 25 -1);
                        Task.Delay(100).Wait();
                        sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, i * 2 + 25 -1);
                    }

                    StopsStatedByIndex[i] = !StopsStatedByIndex[i];
                }
            }
        }
    }
}

// wenn regeister wippe gedrüpck magent einschalten und wider ausacheltne (note on und wider note off)
// für sequender gilt das nicht da dir bisheirge lokeik überneiemn ohne offset von 25
// für sequender muss sender in dev mode sein
// für stops muss sender in deve mode sein und ien zusäelchen der normal ist