// See https://aka.ms/new-console-template for more information

using IP_Midi_Setzer;

Console.WriteLine("Hello, World!");

using var sender = new Sender();

while (true)
{
    // Send a Note On (Channel 1, Middle C, Velocity 100)
    sender.SendNoteOn(0, 60, 100);

// Send a Note Off
    sender.SendNoteOff(0, 60);

// Send Control Change (e.g., Volume on Channel 1)
    sender.SendControlChange(0, 7, 100);

// Send Program Change
    sender.SendProgramChange(0, 0);

// Send Pitch Bend (center = 0)
    sender.SendPitchBend(0, 0);

// Send raw MIDI bytes directly
    sender.Send([0x90, 0x3C, 0x7F]);
    
    Task.Delay(1000).Wait();
}