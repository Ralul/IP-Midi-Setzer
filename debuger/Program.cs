using Avalonia;
using ReactiveUI.Avalonia;
using System;
using debuger.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Core;
using debuger.Services;

namespace debuger;

sealed class Program
{
    public static IServiceProvider Services { get; private set; } = null!;
    
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var collection = new ServiceCollection();
        ConfigureServices(collection);
        Services = collection.BuildServiceProvider();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }
    
    private static void ConfigureServices(IServiceCollection services)
    {
        // ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<SequencerViewModel>();
        services.AddTransient<Sender>();
        services.AddTransient<Receiver>();
        services.AddTransient<SequencerService>();
    }
    
    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}