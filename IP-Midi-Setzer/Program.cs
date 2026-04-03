// See https://aka.ms/new-console-template for more information

using Core;
using IP_Midi_Setzer.EventHandler;
using IP_Midi_Setzer.Service;

Console.WriteLine("Hello, World!");

using var sender = new Sender();

using var receiver = new Receiver(); // default 225.0.0.37:21928

var stopStates = new StopStates();
var stopAction = new HandleStopActions(stopStates);
var sequencerAction = new HandleSequencerActions(stopStates, sender);

receiver.NoteOn += stopAction.NoteOnHandler;
receiver.NoteOn += sequencerAction.NoteOnHandler;


receiver.Start();

Console.WriteLine("Listening... Press Enter to stop.");
Console.ReadLine();