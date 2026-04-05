namespace IP_Midi_Setzer.Service;

public class SequencerCombinationService
{
    private readonly List<HashSet<int>> _sequencerCombinations = Enumerable.Repeat(new HashSet<int>(), 1000).ToList();

    public void SetCombination(HashSet<int> combination, int combinationNumber)
    {
        _sequencerCombinations[combinationNumber] = new HashSet<int>(combination);
    }

    public HashSet<int>? GetCombination(int sequencerCombinationNumber)
    {
        if (_sequencerCombinations.Count > sequencerCombinationNumber - 1)
        {
            return _sequencerCombinations[sequencerCombinationNumber];
        }

        return null;
    }
}