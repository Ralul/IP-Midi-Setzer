using Core;
using IP_Midi_Setzer.Service;

namespace IP_Midi_Setzer.EventHandler;

public class HandleSequencerActions
{
    private readonly StopStates _stopState;
    private readonly Sender _sender;

    public HandleSequencerActions(StopStates stopStates, Sender sender)
    {
        _stopState =  stopStates;
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
            var activeStops = _stopState.GetActiveStops();

            Parallel.ForEach(activeStops, activeStop =>
            {
                _sender.SendNoteOn(SequencerDefinition.CHANEL_STOPS_1_126, activeStop + 1);
                Task.Delay(1000).Wait();
                _sender.SendNoteOff(SequencerDefinition.CHANEL_STOPS_1_126, activeStop + 1);
            });
            
            _stopState.Clear();
        }
 
    }
    
    
    
}