using System.Threading.Tasks;
using Core;

namespace debuger.Services;

public class SequencerService
{
    private readonly Sender _sender;

    public SequencerService(Sender sender)
    {
        _sender = sender;
    }

    public void SendSet()
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL, SequencerDefinition.SET);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL, SequencerDefinition.SET);
    }

    public void SendSetWithDelay(int delay)
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL, SequencerDefinition.SET);
        Task.Delay(delay).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL, SequencerDefinition.SET);
    }

    public void SendForward()
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL, SequencerDefinition.FORWARD);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL, SequencerDefinition.FORWARD);
    }

    public void SendBackward()
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL, SequencerDefinition.BACKWARD);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL, SequencerDefinition.BACKWARD);
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

        _sender.SendNoteOn(SequencerDefinition.CHANEL, selectedCombination);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL, selectedCombination);
    }

    public void SendClear()
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL, SequencerDefinition.CLEAR);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL, SequencerDefinition.CLEAR);
    }

    public void SendDeczimalUp()
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL, SequencerDefinition.DECIMAL_UP);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL, SequencerDefinition.DECIMAL_UP);
    }

    public void SendDeczimalDown()
    {
        _sender.SendNoteOn(SequencerDefinition.CHANEL, SequencerDefinition.DECIMAL_DOWN);
        Task.Delay(500).Wait();
        _sender.SendNoteOff(SequencerDefinition.CHANEL, SequencerDefinition.DECIMAL_DOWN);
    }
}