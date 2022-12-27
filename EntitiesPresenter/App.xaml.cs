using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EntitiesPresenter.Interfaces;
using EntitiesPresenter.ViewModels;
using Microsoft.Extensions.Hosting;
using MessagePipe;
using MessagePipe.Interprocess;

namespace EntitiesPresenter
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
                    services.AddTransient<IEntitiesPresenterViewModel, EntitiesPresenterViewModel>();
                    services.AddMessagePipe();
                    services.AddMessagePipeTcpInterprocess("127.0.0.1", 5001);
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
