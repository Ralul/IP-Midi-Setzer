using Core;

namespace IP_Midi_Setzer.Service;

public class SequencerCombinationService
{
    private readonly List<Stop[]> _sequencerCombinations = new(1000);

    public void SetCombination(Stop[] stops, int combinationNumber)
    {
        // make sure to create copy not just references
        var clonedStops = stops.Clone() as Stop[];
        
        _sequencerCombinations[combinationNumber] = clonedStops;
    }

    public Stop[]? GetCombination(int sequencerCombinationNumber)
    {
        if (_sequencerCombinations.Count > sequencerCombinationNumber - 1)
        {
            return _sequencerCombinations[sequencerCombinationNumber];
        }

        return null;
    }
}

