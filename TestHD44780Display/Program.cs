using System.Device.Gpio;

namespace TestHD44780Display;

public class Program
{
    private const int RsPin = 5;
    private const int RwPin = 6;
    private const int EPin = 13;

    private const int Db0Pin = 19;
    private const int Db1Pin = 26;
    private const int Db2Pin = 21;
    private const int Db3Pin = 20;
    private const int Db4Pin = 16;
    private const int Db5Pin = 12;
    private const int Db6Pin = 1;
    private const int Db7Pin = 7;

    public static void Main()
    {
        using GpioController controller = new();

        controller.OpenPin(RsPin, PinMode.Output);
        controller.OpenPin(RwPin, PinMode.Output);
        controller.OpenPin(EPin, PinMode.Output);

        controller.OpenPin(Db0Pin, PinMode.Output);
        controller.OpenPin(Db1Pin, PinMode.Output);
        controller.OpenPin(Db2Pin, PinMode.Output);
        controller.OpenPin(Db3Pin, PinMode.Output);
        controller.OpenPin(Db4Pin, PinMode.Output);
        controller.OpenPin(Db5Pin, PinMode.Output);
        controller.OpenPin(Db6Pin, PinMode.Output);
        controller.OpenPin(Db7Pin, PinMode.Output);

        controller.Write(RsPin, PinValue.High);
        controller.Write(RwPin, PinValue.High);
        controller.Write(EPin, PinValue.High);
        
        controller.Write(Db0Pin, PinValue.High);
        controller.Write(Db1Pin, PinValue.High);
        controller.Write(Db2Pin, PinValue.High);
        controller.Write(Db3Pin, PinValue.High);
        controller.Write(Db4Pin, PinValue.High);
        controller.Write(Db5Pin, PinValue.High);
        controller.Write(Db6Pin, PinValue.High);
        controller.Write(Db7Pin, PinValue.High);

        _ = WriteInstruction(0x02, controller);
        
        Task.Delay(50).Wait();
        
        Console.ReadLine();
    }

    private static async Task WriteInstruction(
        byte value,
        GpioController controller
    )
    {
        controller.Write(RsPin, PinValue.Low);
        controller.Write(RwPin, PinValue.Low);
        controller.Write(EPin, PinValue.Low);

        controller.Write(
            Db0Pin,
            (value & 0x01) != 0 ? PinValue.High : PinValue.Low
        );
        controller.Write(
            Db1Pin,
            (value & 0x02) != 0 ? PinValue.High : PinValue.Low
        );
        controller.Write(
            Db2Pin,
            (value & 0x04) != 0 ? PinValue.High : PinValue.Low
        );
        controller.Write(
            Db3Pin,
            (value & 0x08) != 0 ? PinValue.High : PinValue.Low
        );
        controller.Write(
            Db4Pin,
            (value & 0x16) != 0 ? PinValue.High : PinValue.Low
        );
        controller.Write(
            Db5Pin,
            (value & 0x32) != 0 ? PinValue.High : PinValue.Low
        );
        controller.Write(
            Db6Pin,
            (value & 0x64) != 0 ? PinValue.High : PinValue.Low
        );
        controller.Write(
            Db7Pin,
            (value & 0x128) != 0 ? PinValue.High : PinValue.Low
        );
    }
}