using System;
using ReactiveUI.SourceGenerators;

namespace debuger.ViewModels;

public partial class SequencerViewModel : ViewModelBase
{
    [ReactiveCommand]
    private void SetClick()
    {
        Console.WriteLine("Set");
    }
    
    [ReactiveCommand]
    private void ForwardClick()
    {
        Console.WriteLine("Forward");
    }

    [ReactiveCommand]
    private void BackwardClick()
    {
        Console.WriteLine("Backward");
    }
}