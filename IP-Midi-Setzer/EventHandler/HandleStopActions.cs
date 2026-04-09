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
        
        if (e.Note % 2 == 1)
        {
            _stopState.SetStopByNote(e.Note, true);
        }
        else
        {
            _stopState.SetStopByNote(e.Note -1, false);
        }
    }
}