using Core;
using Core.Definitions;
using IP_Midi_Setzer.Service;

namespace IP_Midi_Setzer.EventHandler;

public class HandleStopActions
{
    private readonly StopStates _stopState;

    public HandleStopActions(StopStates stopState)
    {
        _stopState = stopState;
    }
    
    public void NoteOnHandler(object? sender, NoteEventArgs e)
    {
        if (e.Channel == SequencerDefinition.CHANEL_SEQUENCER)
        {
            return;
        }

        if (e.Channel == SequencerDefinition.CHANEL_STOPS_1_126)
        {
            if (e.Note % 2 == 1)
            {
                _stopState.SetStopByNote(e.Note, true);
            }
            else
            {
                _stopState.SetStopByNote(e.Note -1, false);
            }
        }
        
        if (e.Channel == SequencerDefinition.CHANEL_STOPS_127_252)
        {
            // todo add hanling
        }
        
        if (e.Channel == SequencerDefinition.CHANEL_STOPS_253_378)
        {
            // todo add hanling
        }
        
        if (e.Channel == SequencerDefinition.CHANEL_STOPS_379_504)
        {
            // todo add hanling
        }
        
        if (e.Channel == SequencerDefinition.CHANEL_STOPS_505_630)
        {
            // todo add hanling
        }
    }
}