using Core;
using Core.Definitions;
using IP_Midi_Setzer.Service;

namespace IP_Midi_Setzer.EventHandler;

public class HandleSequencerActions
{
    private readonly Stop[] _stops;
    private readonly TimeSpan _delayBetweenCalls = new TimeSpan(0, 0, 0, 0, 0, 200);
    private const int MaxSequncerComibantions = 1000;

    // Dictionary for fast access to the stop first int represents the channel the second int represents the note
    // The Dictionary if first initialized with the known channels
    private readonly Dictionary<int, Dictionary<int, Stop>> _stopsByChanelByNoteOn =
        new()
        {
            { SequencerDefinition.CHANEL_STOPS_1_126, new Dictionary<int, Stop>() },
            { SequencerDefinition.CHANEL_STOPS_127_252, new Dictionary<int, Stop>() },
            { SequencerDefinition.CHANEL_STOPS_253_378, new Dictionary<int, Stop>() },
            { SequencerDefinition.CHANEL_STOPS_379_504, new Dictionary<int, Stop>() },
            { SequencerDefinition.CHANEL_STOPS_505_630, new Dictionary<int, Stop>() }
        };

    private readonly Dictionary<int, Dictionary<int, Stop>> _stopsByChanelByNoteOff =
        new()
        {
            { SequencerDefinition.CHANEL_STOPS_1_126, new Dictionary<int, Stop>() },
            { SequencerDefinition.CHANEL_STOPS_127_252, new Dictionary<int, Stop>() },
            { SequencerDefinition.CHANEL_STOPS_253_378, new Dictionary<int, Stop>() },
            { SequencerDefinition.CHANEL_STOPS_379_504, new Dictionary<int, Stop>() },
            { SequencerDefinition.CHANEL_STOPS_505_630, new Dictionary<int, Stop>() }
        };


    private readonly SequencerCombinationService _sequencerCombinationService;
    private readonly DisplayService _displayService;

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
        Stop[] stops,
        SequencerCombinationService sequencerCombinationService,
        DisplayService displayService)
    {
        _stops = stops;
        _sequencerCombinationService = sequencerCombinationService;
        _displayService = displayService;

        foreach (var stop in stops)
        {
            _stopsByChanelByNoteOn[stop.Channel].Add(stop.NoteToEnable, stop);
            _stopsByChanelByNoteOff[stop.Channel].Add(stop.NoteToDisable, stop);
        }
        
        _displayService.ShowNumber(0);
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

            return;
        }

        if (e.Note == SequencerDefinition.SET)
        {
            _isSetHold = true;
        }

        if (e.Note == SequencerDefinition.FORWARD)
        {
            if (_currentPosition == MaxSequncerComibantions)
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
            if (_currentPosition >= MaxSequncerComibantions -9)
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
        for (var i = 0; i < _stops.Length; i++)
        {
            await Task.Delay(_delayBetweenCalls * i);
            _ = _stops[i].DisableStop();
        }
    }

    private async Task EnableSetOfStops(Stop[] desiredStops)
    {
        for (var i = 0; i < desiredStops.Length; i++)
        {
            await Task.Delay(_delayBetweenCalls * i);
            if (desiredStops[i].IsEnabled)
            {
                _ = desiredStops[i].EnableStop();
                if (_stopsByChanelByNoteOn.TryGetValue(desiredStops[i].Channel, out var stopsByNoteOn))
                {
                    if (stopsByNoteOn.TryGetValue(desiredStops[i].NoteToEnable, out var stop))
                    {
                        stop.IsEnabled = true;
                    }
                }
            }
            else
            {
                _ = desiredStops[i].DisableStop();
                if (_stopsByChanelByNoteOff.TryGetValue(desiredStops[i].Channel, out var stopsByNoteOff))
                {
                    if (stopsByNoteOff.TryGetValue(desiredStops[i].NoteToDisable, out var stop))
                    {
                        stop.IsEnabled = false;
                    }
                }
            }
        }
    }

    private void SetOrGetCombination()
    {
        Console.WriteLine(_currentPosition);
        _displayService.ShowNumber(_currentPosition);
        if (_isSetHold)
        {
            _sequencerCombinationService.SetCombination(_stops, _currentPosition);
        }
        else
        {
            var desiredCombination = _sequencerCombinationService.GetCombination(_currentPosition);

            if (desiredCombination != null)
            {
                _ = EnableSetOfStops(desiredCombination);
                return;
            }

            _ = DisableAllStops();
        }
    }

    private void HandleNumericAction(int number)
    {
        _currentPosition = number;

        SetOrGetCombination();
    }
}