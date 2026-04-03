namespace IP_Midi_Setzer.Service;

public class StopStates
{
    private readonly HashSet<int> _activeStops = [];

    public void SetStopByNote(int note, bool isActive)
    {
        if (isActive)
        {
            _activeStops.Add(note);
        }
        else
        {
            _activeStops.Remove(note);
        }
    }

    public HashSet<int> GetActiveStops()
    {
        return _activeStops;
    }

    public void Clear()
    {
        _activeStops.Clear();
    }
}