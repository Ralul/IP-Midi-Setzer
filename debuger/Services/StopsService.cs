using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Definitions;
using debuger.ViewModels;

namespace debuger.Services;

public class StopsService
{
    private readonly Sender _sender;
    private readonly Receiver _receiver;

    public readonly Dictionary<int, StopViewModel> Stops;

    public StopsService(
        Sender sender,
        Receiver receiver
    )
    {
        _sender = sender;
        _receiver = receiver;

        Stops = Enumerable.Range(1, 63)
            .ToDictionary(
                key => key * 2 -1,
                value => new StopViewModel(value * 2 - 1, value * 2, ActionSendNote, ActionSendNote)
            );

        _receiver.NoteOn += NoteOnHandler;
        _receiver.NoteOff += NoteOffHandler;

        receiver.Start();
    }

    private void NoteOnHandler(object? sender, NoteEventArgs e)
    {
        if (e.Channel != SequencerDefinition.CHANEL_STOPS_1_126)
        {
            return;
        }

        if (Stops.TryGetValue(e.Note, out var stopToEnable))
        {
            stopToEnable.IsSolenoidToEnableStopOn = true;
        }
        else if (Stops.TryGetValue(e.Note - 1, out var stopToDisable))
        {
            stopToDisable.IsSolenoidToDisableStopOn = true;
        }
    }

    private void NoteOffHandler(object? sender, NoteEventArgs e)
    {
        if (e.Channel != SequencerDefinition.CHANEL_STOPS_1_126)
        {
            return;
        }

        if (Stops.TryGetValue(e.Note, out var stopToEnable))
        {
            stopToEnable.IsSolenoidToEnableStopOn = false;
        }
        else if (Stops.TryGetValue(e.Note - 1, out var stopToDisable))
        {
            stopToDisable.IsSolenoidToDisableStopOn = false;
        }
    }

    private void ActionSendNote(int note)
    {
        SendNote(note);
    }

    public async Task SendNote(int note)
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL_STOPS_1_126, note);
        await Task.Delay(1000);
        _sender.SendNoteOff(SequencerDefinition.CHANEL_STOPS_1_126, note);
    }
}