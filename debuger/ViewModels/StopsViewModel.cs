using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using debuger.Services;

namespace debuger.ViewModels;

public class StopsViewModel : ViewModelBase
{
    private readonly IMessenger _messenger;
    private readonly StopsService _stopsService;

    private const int FirstStopNote = 1;
    private const int LastStopNote = 126;
    public ObservableCollection<StopViewModel> Stops { get; } = [];

    public StopsViewModel(
        IMessenger messenger,
        StopsService stopsService
    )
    {
        _messenger = messenger;
        _stopsService = stopsService;

        int index = FirstStopNote;
        while (index <= LastStopNote)
        {
            Stops.Add(new StopViewModel(index, index + 1));

            index += 2;
        }
    }
}