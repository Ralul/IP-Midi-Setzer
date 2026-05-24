using System.Device.Gpio;
using Iot.Device.CharacterLcd;

namespace TestDisplay;

public class Program
{
    public static void Main()
    {
        // Pin assignments (BCM numbering)
        const int rs = 5;
        const int rw = 6;   // optional; pass -1 if you tied RW to GND
        const int en = 13;

        // DB0..DB7 — for 8-bit mode pass all 8 pins
        int[] dataPins = { 19, 26, 21, 20, 16, 12, 1, 7 };

        // Backlight pin (optional, -1 if not used)
        const int backlight = -1;

        using var controller = new GpioController();

        using var lcd = new Lcd2004(
            registerSelectPin: rs,
            enablePin: en,
            dataPins: dataPins,
            backlightPin: backlight,
            backlightBrightness: 1.0f,
            readWritePin: rw,
            controller: controller
        );

        lcd.Clear();
        lcd.DisplayOn = true;
        lcd.UnderlineCursorVisible = false;
        lcd.BlinkingCursorVisible = false;

        lcd.SetCursorPosition(0, 0);
        lcd.Write("Hello, Pi!");

        lcd.SetCursorPosition(0, 1);
        lcd.Write("HD44780 works!");

        Console.WriteLine("Text written to LCD. Press Enter to exit.");
        Console.ReadLine();

        lcd.Clear();
    }
}