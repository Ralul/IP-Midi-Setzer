using Core;
using Core.Definitions;

namespace IP_Midi_Setzer.EventHandler;

public class HandleStopActions
{
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

    public HandleStopActions(Stop[] stops)
    {
        foreach (var stop in stops)
        {
            _stopsByChanelByNoteOn[stop.Channel].Add(stop.NoteToEnable, stop);
            _stopsByChanelByNoteOff[stop.Channel].Add(stop.NoteToDisable, stop);
        }
    }

    public void NoteOnHandler(object? sender, NoteEventArgs e)
    {
        if (e.Channel == SequencerDefinition.CHANEL_SEQUENCER)
        {
            return;
        }

        if (_stopsByChanelByNoteOn.TryGetValue(e.Channel, out var stopsByNoteOn))
        {
            if (stopsByNoteOn.TryGetValue(e.Note, out var stop))
            {
                stop.IsEnabled = true;
            }
        }
        
        if (_stopsByChanelByNoteOff.TryGetValue(e.Channel, out var stopsByNoteOff))
        {
            if (stopsByNoteOff.TryGetValue(e.Note, out var stop))
            {
                stop.IsEnabled = false;
            }
        }
}

}