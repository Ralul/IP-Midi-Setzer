using Core;

namespace IP_Midi_Setzer.Service;

public class SequencerCombinationService
{
    private readonly List<Stop[]?> _sequencerCombinations =
        Enumerable.Repeat<Stop[]?>(null, 1000).ToList();

    private readonly PersistenceService _persistenceService = new PersistenceService();

    public void SetCombination(Stop[] stops, int combinationNumber)
    {
        // Deep copy: clone each Stop individually
        var clonedStops = stops.Select(s => (Stop)s.Clone()).ToArray();

        _sequencerCombinations[combinationNumber] = clonedStops;

        _persistenceService.StoreCombination(combinationNumber, clonedStops);
    }

    public Stop[]? GetCombination(int sequencerCombinationNumber)
    {
        if (_sequencerCombinations.Count > sequencerCombinationNumber - 1)
        {
            if (_sequencerCombinations[sequencerCombinationNumber] != null)
            {
                return _sequencerCombinations[sequencerCombinationNumber];
            }
            // find out why does that not work
            var storedSequencerCombination = _persistenceService.GetCombination(sequencerCombinationNumber);

            if (storedSequencerCombination != null)
            {
                return storedSequencerCombination;
            }
        }

        return null;
    }
}