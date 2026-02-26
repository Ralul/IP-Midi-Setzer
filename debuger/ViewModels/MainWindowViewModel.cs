using Core;
using ReactiveUI.SourceGenerators;

namespace debuger.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly Sender _sender;
    
    public MainWindowViewModel(Sender sender)
    {
        _sender = sender;
    }
    
    [ReactiveCommand]
    public void ButtonClick()
    {
        _sender.SendNoteOn(0, 60, 100);
    }
}