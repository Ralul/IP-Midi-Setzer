using System;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI.SourceGenerators;

namespace debuger.ViewModels;

public partial class StopViewModel : ViewModelBase
{
    private readonly Action<int> _enableStopAction;
    private readonly Action<int> _disableStopAction;
    
    
    private readonly IMessenger _messenger;
    public StopViewModel(IMessenger messenger,int noteToEnable, int noteToDisable, Action<int> enableStop, Action<int> disableStop)
    {
        _messenger = messenger;
        _noteToEnable = noteToEnable;
        _noteToDisable = noteToDisable;
        _enableStopAction = enableStop;
        _disableStopAction = disableStop;
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

    [Reactive] private bool _isUserOperatedStopLeverOn;
}