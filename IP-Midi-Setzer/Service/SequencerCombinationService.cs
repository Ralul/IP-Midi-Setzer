using Core;

namespace IP_Midi_Setzer.Service;

public class SequencerCombinationService
{
    private readonly List<Stop[]?> _sequencerCombinations =
        Enumerable.Repeat<Stop[]?>(null, 1000).ToList();

    public void SetCombination(Stop[] stops, int combinationNumber)
    {
        // Deep copy: clone each Stop individually
        var clonedStops = stops.Select(s => (Stop)s.Clone()).ToArray();
        
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