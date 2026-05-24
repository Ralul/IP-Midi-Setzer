using System.Device.Gpio;
using Iot.Device.CharacterLcd;

namespace IP_Midi_Setzer.Service;

public class DisplayService : IDisposable
{
    private readonly GpioController _controller;
    private readonly Lcd2004 _lcd;

    public DisplayService()
    {
        const int rs = 5;
        const int rw = 6;
        const int en = 13;

        int[] dataPins = { 19, 26, 21, 20, 16, 12, 1, 7 };

        const int backlight = -1;

        _controller = new GpioController();

        _lcd = new Lcd2004(
            registerSelectPin: rs,
            enablePin: en,
            dataPins: dataPins,
            backlightPin: backlight,
            backlightBrightness: 1.0f,
            readWritePin: rw,
            controller: _controller
        );

        _lcd.Clear();
        _lcd.DisplayOn = true;
        _lcd.UnderlineCursorVisible = false;
        _lcd.BlinkingCursorVisible = false;
        _lcd.SetCursorPosition(0, 0);
    }

    public void ShowNumber(int number)
    {
        Console.WriteLine($"Display service called: {number}");
        _lcd.Clear();
        
        _lcd.SetCursorPosition(0, 0);
        _lcd.Write(number.ToString());
    }

    public void Dispose()
    {
        _lcd.Dispose();
        _controller.Dispose();
        GC.SuppressFinalize(this);
    }
}