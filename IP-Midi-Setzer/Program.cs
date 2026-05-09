using Core;
using IP_Midi_Setzer.EventHandler;
using IP_Midi_Setzer.Service;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        using var sender = new Sender();
        using var receiver = new Receiver(); // default 225.0.0.37:21928

        var stops = new Stop[64];

        const int channel = 10;
        for (var i = 0; i <= 63; i++)
        {
            var noteToDisable = i * 2;
            var noteToEnable = i * 2 + 1;
            stops[i] = new Stop(noteToEnable, noteToDisable, channel, sender);
        }

        var sequencerCombinaitonService = new SequencerCombinationService();
        var stopAction = new HandleStopActions(stops);
        var sequencerAction = new HandleSequencerActions(stops, sequencerCombinaitonService, sender);

        receiver.NoteOn += stopAction.NoteOnHandler;

        receiver.NoteOn += sequencerAction.NoteOnHandler;
        receiver.NoteOff += sequencerAction.NoteOffHanlder;

        receiver.NoteOn += (_, e) => Console.WriteLine($"Note On  | Ch {e.Channel} | Note {e.Note} | Vel {e.Velocity}");
        receiver.NoteOff += (_, e) => Console.WriteLine($"Note Off | Ch {e.Channel} | Note {e.Note}");

        receiver.Start();

        Console.WriteLine("Listening... Press Enter to stop.");
        Console.ReadLine();
    }
}