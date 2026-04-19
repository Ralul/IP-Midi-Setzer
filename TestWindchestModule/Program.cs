using Core;
using DotNetEnv;

namespace TestWindchestModule;

public class Program
{
    static public void Main()
    {
        Env.Load();
        var isDevModeOn = Environment.GetEnvironmentVariable("IS_RUNNING_IN_DEVELOPMENT") == "true";

        using var sender = new Sender(isDveModeOn: isDevModeOn);

        using var receiver = new Receiver(); // default 225.0.0.37:21928

        receiver.NoteOn += (_, e) => Console.WriteLine($"Note On  | Ch {e.Channel} | Note {e.Note} | Vel {e.Velocity}");
        receiver.NoteOff += (_, e) => Console.WriteLine($"Note Off | Ch {e.Channel} | Note {e.Note}");
        receiver.Start();
        
        

        var cts = new CancellationTokenSource();

        while (!cts.Token.IsCancellationRequested)
        {
            Console.WriteLine("Press CTRL+C to exit");
            Console.WriteLine("Press e to send a note ON message");
            Console.WriteLine("Press d to send a note OFF message");

            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.E)
            {
                Console.WriteLine("Enter channel 0 to 15");
                var channelInput = Console.ReadLine();
                if (int.TryParse(channelInput, out var channel))
                {
                    if (channel >= 1 && channel <= 16)
                    {
                        Console.WriteLine("Enter note 0 to 127");
                        var noteInput = Console.ReadLine();
                        if (int.TryParse(noteInput, out var note))
                        {
                            if (note >= 1 && note <= 127)
                            {
                                sender.SendNoteOn(channel, note);
                                Console.WriteLine("Note On  | Ch {0} | Note {1}", channel, note);
                                Task.Delay(1000).Wait();
                            }
                        }
                    }
                }
            }

            if (key.Key == ConsoleKey.D)
            {
                Console.WriteLine("Enter channel 1 to 16");
                var channelInput = Console.ReadLine();
                if (int.TryParse(channelInput, out var channel))
                {
                    if (channel >= 1 && channel <= 16)
                    {
                        Console.WriteLine("Enter note 1 to 127");
                        var noteInput = Console.ReadLine();
                        if (int.TryParse(noteInput, out var note))
                        {
                            if (note >= 1 && note <= 127)
                            {
                                sender.SendNoteOff(channel, note);
                                Console.WriteLine("Note Off  | Ch {0} | Note {1}", channel, note);
                                Task.Delay(1000).Wait();
                            }
                        }
                    }
                }
            }
        }

        Console.ReadLine();
    }
}