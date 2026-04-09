using System.Threading.Tasks;
using Core;
using Core.Definitions;

namespace debuger.Services;

public class SequencerService
{
    private readonly Sender _sender;

    public SequencerService(Sender sender)
    {
        _sender = sender;
    }

    public void SendSetIsPressed()
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.SET);
    }

    public void SendSetIsReleased()
    {
        _sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.SET);
    }

    public void SendSetWithDelay(int delay)
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.SET);
        Task.Delay(delay).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.SET);
    }

    public void SendForward()
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.FORWARD);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.FORWARD);
    }

    public void SendBackward()
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.BACKWARD);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.BACKWARD);
    }

    public void SendCombination(int numberOfCombinations)
    {
        var selectedCombination = numberOfCombinations switch
        {
            0 => SequencerDefinition.COMBINATION_0,
            1 => SequencerDefinition.COMBINATION_1,
            2 => SequencerDefinition.COMBINATION_2,
            3 => SequencerDefinition.COMBINATION_3,
            4 => SequencerDefinition.COMBINATION_4,
            5 => SequencerDefinition.COMBINATION_5,
            6 => SequencerDefinition.COMBINATION_6,
            7 => SequencerDefinition.COMBINATION_7,
            8 => SequencerDefinition.COMBINATION_8,
            9 => SequencerDefinition.COMBINATION_9,
            _ => 0
        };

        _sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, selectedCombination);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, selectedCombination);
    }

    public void SendClear()
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.CLEAR);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.CLEAR);
    }

    public void SendDeczimalUp()
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.DECIMAL_UP);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.DECIMAL_UP);
    }

    public void SendDeczimalDown()
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.DECIMAL_DOWN);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL_SEQUENCER, SequencerDefinition.DECIMAL_DOWN);
    }
}