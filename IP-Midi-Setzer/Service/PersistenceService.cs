using System.Text.Json;
using Core;

namespace IP_Midi_Setzer.Service;

public class PersistenceService
{
    private readonly string _applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private const string ApplicationName = "IP-Midi-Setzer";

    public PersistenceService()
    {
    }

    public Stop[]? GetCombination(int sequencerCombinationNumber)
    {
        try
        {
            var path = Path.Combine(_applicationDataPath, ApplicationName, $"{sequencerCombinationNumber}.json");
            var streamReader = new StreamReader(path);

            var fileContent = streamReader.ReadToEnd();
            
            Console.WriteLine(fileContent);

            var stops = JsonSerializer.Deserialize<Stop[]>(fileContent);

            var returnStops = new List<Stop>();
            foreach (var stop in stops)
            {
                returnStops.Add(new Stop(stop.NoteToEnable,  stop.NoteToDisable, stop.Channel, new Sender()));
            }
            return returnStops.ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            return null;
        }
    }

    public void StoreCombination(int sequencerCombinationNumber, Stop[] stops)
    {
        try
        {
            var str = JsonSerializer.Serialize(stops);

            var path = Path.Combine(_applicationDataPath, ApplicationName, $"{sequencerCombinationNumber}.json");

            var streamWriter = new StreamWriter(path);

            streamWriter.Write(str);
            
            

            streamWriter.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ex {e}");
        }
    }
}