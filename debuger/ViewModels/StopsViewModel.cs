using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using debuger.Services;

namespace debuger.ViewModels;

public class StopsViewModel : ViewModelBase
{
    private readonly StopsService _stopsService;
    public ObservableCollection<StopViewModel> Stops { get; } = [];

    public StopsViewModel(IMessenger messenger, StopsService stopsService)
    {
        _stopsService = stopsService;

        messenger.RegisterAll(this);

        foreach (var key in _stopsService.Stops.Keys)
        {
            Stops.Add(_stopsService.Stops[key]);
        }
    }
}