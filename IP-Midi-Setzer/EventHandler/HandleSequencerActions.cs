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

    private HashSet<int> _numericActions = new()
    {
        SequencerDefinition.COMBINATION_0,
        SequencerDefinition.COMBINATION_1,
        SequencerDefinition.COMBINATION_2,
        SequencerDefinition.COMBINATION_3,
        SequencerDefinition.COMBINATION_4,
        SequencerDefinition.COMBINATION_5,
        SequencerDefinition.COMBINATION_6,
        SequencerDefinition.COMBINATION_7,
        SequencerDefinition.COMBINATION_8,
        SequencerDefinition.COMBINATION_9
    };

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

            SetOrGetCombination();
        }

        if (e.Note == SequencerDefinition.BACKWARD)
        {
            if (_currentPosition == 0)
            {
                return;
            }

            _currentPosition--;

            SetOrGetCombination();
        }

        if (_numericActions.Contains(e.Note))
        {
            switch (e.Note)
            {
                case SequencerDefinition.COMBINATION_0:
                    HandleNumericAction(
                        (_currentPosition - _currentPosition % 10) + 0);
                    break;
                case SequencerDefinition.COMBINATION_1:
                    HandleNumericAction(
                        (_currentPosition - _currentPosition % 10) + 1);
                    break;
                case SequencerDefinition.COMBINATION_2:
                    HandleNumericAction(
                        (_currentPosition - _currentPosition % 10) + 2);
                    break;
                case SequencerDefinition.COMBINATION_3:
                    HandleNumericAction(
                        (_currentPosition - _currentPosition % 10) + 3);
                    break;
                case SequencerDefinition.COMBINATION_4:
                    HandleNumericAction(
                        (_currentPosition - _currentPosition % 10) + 4);
                    break;
                case SequencerDefinition.COMBINATION_5:
                    HandleNumericAction(
                        (_currentPosition - _currentPosition % 10) + 5);
                    break;
                case SequencerDefinition.COMBINATION_6:
                    HandleNumericAction(
                        (_currentPosition - _currentPosition % 10) + 6);
                    break;
                case SequencerDefinition.COMBINATION_7:
                    HandleNumericAction(
                        (_currentPosition - _currentPosition % 10) + 7);
                    break;
                case SequencerDefinition.COMBINATION_8:
                    HandleNumericAction(
                        (_currentPosition - _currentPosition % 10) + 8);
                    break;
                case SequencerDefinition.COMBINATION_9:
                    HandleNumericAction(
                        (_currentPosition - _currentPosition % 10) + 9);
                    break;
            }
        }

        if (e.Note == SequencerDefinition.DECIMAL_UP)
        {
            if (_currentPosition >= 991)
            {
                return;
            }
            
            _currentPosition += 10;

            SetOrGetCombination();
        }

        if (e.Note == SequencerDefinition.DECIMAL_DOWN)
        {
            if (_currentPosition <= 9)
            {
                return;
            }

            _currentPosition -= 10;

            SetOrGetCombination();
        }
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

    private void SetOrGetCombination()
    {
        Console.WriteLine(_currentPosition);
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

    private void HandleNumericAction(int number)
    {
        _currentPosition = number;

        SetOrGetCombination();
    }
}