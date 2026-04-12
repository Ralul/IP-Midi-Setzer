using System.Device.Gpio;
using Core;
using DotNetEnv;

namespace Pi_Stops_Input;

class Program
{
    
    static void Main()
    {
        Env.Load();
        using var sender = new Sender(interfaceName: Environment.GetEnvironmentVariable("NETWORK_INTERFACENAME"));
        
        const int s0Pin = 17; // Select/Address pin
        const int s1Pin = 18; // Select/Address pin
        const int s2Pin = 27; // Select/Address pin
        const int s3Pin = 22; // Select/Address pin
        const int eoPin = 23; // Enable for all switches ON/OFF
        const int sigPin = 24; // Common input or output.
        
        using GpioController controller = new GpioController();
        controller.OpenPin(s0Pin, PinMode.Output);
        controller.OpenPin(s1Pin, PinMode.Output);
        controller.OpenPin(s2Pin, PinMode.Output);
        controller.OpenPin(s3Pin, PinMode.Output);
        controller.OpenPin(eoPin, PinMode.Output);
        controller.OpenPin(sigPin, PinMode.Input);
        
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

                var isToggled = controller.Read(sigPin);

                if (isToggled == PinValue.High)
                {
                    Console.WriteLine($"{i}: {isToggled}");
                }
            }
        }
    }
}