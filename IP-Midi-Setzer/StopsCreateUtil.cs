using Core;
using Core.Definitions;
using IP_Midi_Setzer.Models;

namespace IP_Midi_Setzer;

public class StopsCreateUtil
{
    private readonly Sender _sender;

    private const int StopsPerChannel = 64;
    private const int TotalStops = 320;

    private static readonly int[] StopChannels =
    {
        SequencerDefinition.CHANEL_STOPS_1_126,
        SequencerDefinition.CHANEL_STOPS_127_252,
        SequencerDefinition.CHANEL_STOPS_253_378,
        SequencerDefinition.CHANEL_STOPS_379_504,
        SequencerDefinition.CHANEL_STOPS_505_630,
    };

    public StopsCreateUtil(Sender sender)
    {
        _sender = sender;
    }

    public Stop[] GetDisabledStops()
    {
        var stops = new Stop[TotalStops];
        for (var i = 0; i < stops.Length; i++)
        {
            stops[i] = CreateStop(i);
        }

        return stops;
    }

    public Stop[] GetCustomizedStops(StopDto[] stopDtos)
    {
        if (stopDtos.Length != TotalStops)
        {
            throw new ArgumentException(
                $"Expected {TotalStops} StopDtos but got {stopDtos.Length}.",
                nameof(stopDtos));
        }

        var stops = new Stop[TotalStops];
        for (var i = 0; i < stops.Length; i++)
        {
            stops[i] = CreateStop(i);
            stops[i].IsEnabled = stopDtos[i].IsStopEnabled;
        }

        return stops;
    }

    private Stop CreateStop(int index)
    {
        var note = index % StopsPerChannel;
        var channel = StopChannels[index / StopsPerChannel];
        var noteToDisable = note * 2;
        var noteToEnable = note * 2 + 1;
        return new Stop(noteToEnable, noteToDisable, channel, _sender);
    }
}