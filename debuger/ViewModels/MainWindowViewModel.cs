using Core;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI.SourceGenerators;

namespace debuger.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly Sender _sender;

    [Reactive]
    private ViewModelBase _currentPage = null!;

    public MainWindowViewModel(Sender sender)
    {
        _sender = sender;
        NavigateTo("Sequencer"); // default page
    }

    public void NavigateTo(string? tag)
    {
        CurrentPage = tag switch
        {
            "Sequencer" => Program.Services.GetRequiredService<SequencerViewModel>(),
            // add more cases here later
            _ => CurrentPage
        };
    }

    [ReactiveCommand]
    public void ButtonClick()
    {
        _sender.SendNoteOn(0, 60, 100);
    }
}