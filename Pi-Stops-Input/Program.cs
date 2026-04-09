using System.Device.Gpio;

class Program
{
    
    static void Main()
    {
        const int s1Pin = 17; // Select/Address pin
        const int s2Pin = 18; // Select/Address pin
        const int s3Pin = 27; // Select/Address pin
        const int s4Pin = 22; // Select/Address pin
        const int eoPin = 23; // Enable for all switches ON/OFF
        const int sigPin = 24; // Common input or output.
        
        using GpioController controller = new GpioController();
        controller.OpenPin(s1Pin, PinMode.Output);
        controller.OpenPin(s2Pin, PinMode.Output);
        controller.OpenPin(s3Pin, PinMode.Output);
        controller.OpenPin(s4Pin, PinMode.Output);
        controller.OpenPin(eoPin, PinMode.Output);
        controller.OpenPin(sigPin, PinMode.Input);
        
        Console.WriteLine("Pi-Stops-Input. Press Ctrl+C to exit.");
        while (true)
        {
            
            // controller.Write(ledPin, PinValue.High);
            // Thread.Sleep(1000);
            // controller.Write(ledPin, PinValue.Low);
            // Thread.Sleep(1000);
        }
    }
}