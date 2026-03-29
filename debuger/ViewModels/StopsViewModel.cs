using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using debuger.Message;
using debuger.Services;

namespace debuger.ViewModels;

public class StopsViewModel : ViewModelBase, IRecipient<NoteMessage>
{
    private readonly IMessenger _messenger;
    private readonly StopsService _stopsService;

    private const int FirstStopNote = 1;
    private const int LastStopNote = 126;
    public ObservableCollection<StopViewModel> Stops { get; } = [];

    public StopsViewModel(IMessenger messenger, StopsService stopsService)
    {
        _messenger = messenger;
        _stopsService = stopsService;
        
        WeakReferenceMessenger.Default.RegisterAll(this);

        int index = FirstStopNote;
        while (index <= LastStopNote)
        {
            Stops.Add(new StopViewModel(messenger,index, index + 1, EnableStop, DisableStop));

            index += 2;
        }
    }

    private void EnableStop(int note)
    {
        _stopsService.SetNote(note);
    }

    private void DisableStop(int note)
    {
        _stopsService.SetNote(note);
    }

    public void Receive(NoteMessage message)
    {
        Console.WriteLine("afef");
    }
}