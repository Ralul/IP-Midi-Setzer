using Core;
using IP_Midi_Setzer.Service;

namespace IP_Midi_Setzer.EventHandler;

public class HandleSequencerActions
{
    private readonly StopStates _stopState;
    private readonly SequencerCombinationService _sequencerCombinationService;
    private readonly Sender _sender;

    private bool _isSetHold;
    private int _currentPosition;

    public HandleSequencerActions(
        StopStates stopStates,
        SequencerCombinationService sequencerCombinationService,
        Sender sender)
    {
        _stopState = stopStates;
        _sequencerCombinationService = sequencerCombinationService;
        _sender = sender;
    }

    public void NoteOnHandler(object? sender, NoteEventArgs e)
    {
        if (e.Channel != SequencerDefinition.CHANEL_SEQUENCER)
        {
            return;
        }

        if (e.Note == SequencerDefinition.CLEAR)
        {
            DisableAllStops();

            _stopState.Clear();
            return;
        }

        if (e.Note == SequencerDefinition.SET)
        {
            _isSetHold = true;
        }

        if (e.Note == SequencerDefinition.FORWARD)
        {
            if (_currentPosition == 1000)
            {
                return;
            }
            _currentPosition++;

            if (_isSetHold)
            {
                var activeStops = _stopState.GetActiveStops();

                _sequencerCombinationService.SetCombination(activeStops, _currentPosition);
            }
            else
            {
                var desiredCombination = _sequencerCombinationService.GetCombination(_currentPosition);

                if (desiredCombination == null)
                {
                    return;
                }

                EnableSetOfStops(desiredCombination);
            }
        }

        if (e.Note == SequencerDefinition.BACKWARD)
        {
            if (_currentPosition == 0)
            {
                return;
            }
            _currentPosition--;

            if (_isSetHold)
            {
                var activeStops = _stopState.GetActiveStops();

                _sequencerCombinationService.SetCombination(activeStops, _currentPosition);
            }
            else
            {
                var desiredCombination = _sequencerCombinationService.GetCombination(_currentPosition);

                if (desiredCombination == null)
                {
                    return;
                }

                EnableSetOfStops(desiredCombination);
            }
        }

        // if (e.Note == SequencerDefinition.COMBINATION_0)
        // {
        //     var desiredCombination = _sequencerCombinationService.GetCombination(0);
        //
        //     if (desiredCombination == null)
        //     {
        //         return;
        //     }
        //     
        //     Parallel.ForEach(desiredCombination, stops =>
        //     {
        //         _sender.SendNoteOn(SequencerDefinition.CHANEL_STOPS_1_126, stops);
        //         Task.Delay(1000).Wait();
        //         _sender.SendNoteOff(SequencerDefinition.CHANEL_STOPS_1_126, stops);
        //     });
        // }
    }

    public void NoteOffHanlder(object? sender, NoteEventArgs e)
    {
        if (e.Channel != SequencerDefinition.CHANEL_SEQUENCER)
        {
            return;
        }

        if (e.Note == SequencerDefinition.SET)
        {
            _isSetHold = false;
        }
    }

    private async Task DisableAllStops()
    {
        var tasks = Enumerable.Range(1, 63).Select(async stop =>
        {
            _sender.SendNoteOn(SequencerDefinition.CHANEL_STOPS_1_126, stop * 2);
            await Task.Delay(1000);
            _sender.SendNoteOff(SequencerDefinition.CHANEL_STOPS_1_126, stop * 2);
        });

        await Task.WhenAll(tasks);
    }

    private async Task EnableSetOfStops(HashSet<int> desiredStops)
    {
        var tasks = Enumerable.Range(1, 63).Select(async stop =>
        {
            if (desiredStops.Contains(stop * 2 - 1))
            {
                _sender.SendNoteOn(SequencerDefinition.CHANEL_STOPS_1_126, stop * 2 - 1);
                await Task.Delay(1000);
                _sender.SendNoteOff(SequencerDefinition.CHANEL_STOPS_1_126, stop * 2 - 1);
            }
            else
            {
                _sender.SendNoteOn(SequencerDefinition.CHANEL_STOPS_1_126, stop * 2);
                await Task.Delay(1000);
                _sender.SendNoteOff(SequencerDefinition.CHANEL_STOPS_1_126, stop * 2);
            }
        });

        await Task.WhenAll(tasks);
    }
}