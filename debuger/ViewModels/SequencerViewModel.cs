using System;
using debuger.Services;
using ReactiveUI.SourceGenerators;

namespace debuger.ViewModels;

public partial class SequencerViewModel : ViewModelBase
{
    private readonly SequencerService _sequencerService;
    
    public SequencerViewModel(SequencerService sequencerService)
    {
        _sequencerService = sequencerService;
    }
    
    [ReactiveCommand]
    private void SetClick()
    {
        Console.WriteLine("Set");
        _sequencerService.SendSet();
    }
    
    [ReactiveCommand]
    private void SetHold2SekClick()
    {
        Console.WriteLine("Set hold 2 seconds");
        _sequencerService.SendSetWithDelay(2000);
    }

    [ReactiveCommand]
    private void ForwardClick()
    {
        Console.WriteLine("Forward");
        _sequencerService.SendForward();
    }

    [ReactiveCommand]
    private void BackwardClick()
    {
        Console.WriteLine("Backward");
        _sequencerService.SendBackward();
    }

    [ReactiveCommand]
    private void NumberClick(string number)
    {
        int parsedNumber = int.Parse(number);
        Console.WriteLine($"Number: {parsedNumber}");
        _sequencerService.SendCombination(parsedNumber);
    }

    [ReactiveCommand]
    private void ClearClick()
    {
        Console.WriteLine("Clear");
        _sequencerService.SendClear();
    }

    [ReactiveCommand]
    private void DecimalUpClick()
    {
        Console.WriteLine("DecimalUp");
        _sequencerService.SendDeczimalUp();
    }

    [ReactiveCommand]
    private void DecimalDownClick()
    {
        Console.WriteLine("DecimalDown");
        _sequencerService.SendDeczimalDown();
    }
}