using Avalonia.Controls;
using debuger.ViewModels;
using FluentAvalonia.UI.Controls;

namespace debuger.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void NavView_SelectionChanged(object? sender,
        NavigationViewSelectionChangedEventArgs e)
    {
        if (e.SelectedItem is NavigationViewItem item
            && DataContext is MainWindowViewModel vm)
        {
            vm.NavigateTo(item.Tag?.ToString());
        }
    }
}