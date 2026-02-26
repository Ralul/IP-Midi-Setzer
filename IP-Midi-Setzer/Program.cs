// See https://aka.ms/new-console-template for more information

using Core;

Console.WriteLine("Hello, World!");

using var sender = new Sender();


using var receiver = new Receiver(); // default 225.0.0.37:21928

receiver.NoteOn += (_, e) =>
    Console.WriteLine($"Note On  | Ch {e.Channel} | Note {e.Note} | Vel {e.Velocity}");

receiver.NoteOff += (_, e) =>
    Console.WriteLine($"Note Off | Ch {e.Channel} | Note {e.Note}");

receiver.ControlChange += (_, e) =>
    Console.WriteLine($"CC       | Ch {e.Channel} | CC {e.Controller} = {e.Value}");

receiver.PitchBend += (_, e) =>
    Console.WriteLine($"Pitch    | Ch {e.Channel} | Value {e.Value}");

receiver.UnknownMessageReceived += (_, e) =>
    Console.WriteLine($"Unknown  | {BitConverter.ToString(e.RawData)}");

receiver.Start();

Console.WriteLine("Listening... Press Enter to stop.");
Console.ReadLine();