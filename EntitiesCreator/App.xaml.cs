using Microsoft.Extensions.Hosting;
using System.Windows;
using EntitiesCreator.Interfaces;
using EntitiesCreator.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using XDMessaging;

namespace AeronauticsAssignment
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public App()
        {
            AppHost = Host
                .CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<IEntitiesCreatorViewModel, EntitiesCreatorViewModel>();
                    services.AddTransient<XDMessagingClient>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();

            base.OnExit(e);
        }
    }
}
