using ReactiveUI.SourceGenerators;

namespace debuger.ViewModels;

public partial class StopViewModel : ViewModelBase
{
    public StopViewModel(int noteToEnable, int noteToDisable)
    {
        _noteToEnable = noteToEnable;
        _noteToDisable = noteToDisable;
    }
    
    [Reactive] private int _noteToEnable;
    [Reactive] private int _noteToDisable;

    [Reactive] private bool _isSolenoidToEnableStopOn;

    [Reactive] private bool _isSolenoidToDisableStopOn;

    [Reactive] private bool _isStopOn;

    [Reactive] private bool _isUserOperatedStopLeverOn;
    
}