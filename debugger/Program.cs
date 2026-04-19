using Avalonia;
using ReactiveUI.Avalonia;
using System;
using CommunityToolkit.Mvvm.Messaging;
using debuger.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Core;
using debuger.Services;
using DotNetEnv;

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
        Env.Load();

        if (Environment.GetEnvironmentVariable("IS_RUNNING_IN_DEVELOPMENT") == "true")
        {
            services.AddTransient<Sender>(sp => new Sender(isDveModeOn: true));
        }
        else
        {
            services.AddTransient<Sender>();
        }

        // Core Library
        services.AddTransient<Receiver>();
        
        // ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddSingleton<SequencerViewModel>();
        services.AddTransient<StopsViewModel>();

        // Messenger
        services.AddSingleton<IMessenger, WeakReferenceMessenger>();
        
        // Services
        services.AddSingleton<StopsService>();
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