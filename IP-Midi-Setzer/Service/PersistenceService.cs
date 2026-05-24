using System.Text.Json;
using Core;
using IP_Midi_Setzer.Models;

namespace IP_Midi_Setzer.Service;

public class PersistenceService
{
    private readonly StopsCreateUtil _stopsCreateUtil;
    private const string ApplicationName = "IP-Midi-Setzer";

    private string DirectoryPath
    {
        get => Path.Combine(field, ApplicationName);
    } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    public PersistenceService(StopsCreateUtil stopsCreateUtil)
    {
        _stopsCreateUtil = stopsCreateUtil;
        Directory.CreateDirectory(DirectoryPath);
    }

    public Stop[]? GetCombination(int sequencerCombinationNumber)
    {
        try
        {
            var path = Path.Combine(DirectoryPath, $"{sequencerCombinationNumber}.json");

            if (!File.Exists(path))
            {
                return null;
            }

            var fileContent = File.ReadAllText(path);
            var stopsDtos = JsonSerializer.Deserialize<StopDto[]>(fileContent);

            if (stopsDtos is null)
            {
                return null;
            }

            var stops = _stopsCreateUtil.GetCustomizedStops(stopsDtos);

            return stops;
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
            StopDto[] stopDtos = new StopDto[stops.Length];

            for (var i = 0; i < stops.Length; i++)
            {
                stopDtos[i] = new StopDto
                {
                    IsStopEnabled = stops[i].IsEnabled
                };
            }

            var path = Path.Combine(DirectoryPath, $"{sequencerCombinationNumber}.json");
            var str = JsonSerializer.Serialize(stopDtos);
            File.WriteAllText(path, str);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ex {e}");
        }
    }
}