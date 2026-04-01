using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Core;
using debuger.Message;

namespace debuger.Services;

public class StopsService : IRecipient<NoteMessage>
{
    private readonly Sender _sender;
    private readonly Receiver _receiver;
    private readonly IMessenger _messenger;

    public StopsService(
        Sender sender,
        Receiver receiver,
        IMessenger messenger
    )
    {
        _sender = sender;
        _receiver = receiver;
        _messenger = messenger;

        _receiver.NoteOn += NoteOnHandler;
        _receiver.NoteOff += NoteOffHandler;

        receiver.Start();
    }

    private void NoteOnHandler(object? sender, NoteEventArgs e)
    {
        _messenger.Send(new NoteMessage(e, NoteType.On, MessageState.Receiveed));
    }
    
    private void NoteOffHandler(object? sender, NoteEventArgs e)
    {
        _messenger.Send(new NoteMessage(e, NoteType.Off, MessageState.Receiveed));
    }

    public void Receive(NoteMessage message)
    {
        if (message.MessageState != MessageState.Sending) return;
        
        switch (message.NoteType)
        {
            case NoteType.On:
                _sender.SendNoteOn(message.Channel, message.Note);
                break;
            case NoteType.Off:
                _sender.SendNoteOff(message.Channel, message.Note);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task SendNote(int note)
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL_STOPS_1_126, note);
        await Task.Delay(1000);
        _sender.SendNoteOff(SequencerDefinition.CHANEL_STOPS_1_126, note);
    }
}