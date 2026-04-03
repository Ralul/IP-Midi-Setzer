using System;
using DynamicData.Binding;
using ReactiveUI.SourceGenerators;

namespace debuger.ViewModels;

public partial class StopViewModel : ViewModelBase
{
    private readonly Action<int> _enableStopAction;
    private readonly Action<int> _disableStopAction;


    public StopViewModel(int noteToEnable, int noteToDisable, Action<int> enableStop, Action<int> disableStop)
    {
        NoteToEnable = noteToEnable;
        NoteToDisable = noteToDisable;
        _enableStopAction = enableStop;
        _disableStopAction = disableStop;

        this.WhenAnyPropertyChanged(nameof(IsSolenoidToEnableStopOn)).Subscribe(_ =>
        {
            if (IsSolenoidToEnableStopOn)
            {
                IsStopOn = true;
            }
        });
        
        this.WhenAnyPropertyChanged(nameof(IsSolenoidToDisableStopOn)).Subscribe(_ =>
        {
            if (IsSolenoidToDisableStopOn)
            {
                IsStopOn = false;
            }
        });
    }

    [ReactiveCommand]
    private void _enableStop()
    {
        _enableStopAction.Invoke(_noteToEnable);
    }

    [ReactiveCommand]
    private void _disableStop()
    {
        _disableStopAction.Invoke(_noteToDisable);
    }

    [Reactive] private int _noteToEnable;
    [Reactive] private int _noteToDisable;

    [Reactive] private bool _isSolenoidToEnableStopOn;

    [Reactive] private bool _isSolenoidToDisableStopOn;

    [Reactive] private bool _isStopOn;

}