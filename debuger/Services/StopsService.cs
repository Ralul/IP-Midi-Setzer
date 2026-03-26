using System;
using CommunityToolkit.Mvvm.Messaging;
using Core;
using debuger.Message;

namespace debuger.Services;

public class StopsService
{
    private readonly Receiver _receiver;
    private readonly IMessenger _messenger;

    public StopsService(
        Receiver receiver,
        IMessenger messenger
    )
    {
        _receiver = receiver;
        _messenger = messenger;

        _receiver.NoteOn += NoteOnHandler;
        _receiver.NoteOff += NoteOffHanlder;

        receiver.Start();
    }

    private void NoteOnHandler(object? sender, NoteEventArgs e)
    {
        Console.WriteLine($"Note On  | Ch {e.Channel} | Note {e.Note} | Vel {e.Velocity}");

        _messenger.Send(new NoteOnMessage(e));
    }
    
    private void NoteOffHanlder(object? sender, NoteEventArgs e)
    {
        Console.WriteLine($"Note On  | Ch {e.Channel} | Note {e.Note} | Vel {e.Velocity}");

        _messenger.Send(new NoteOnMessage(e));
    }
}