using AsciiTreeMaker.Domain;
using AsciiTreeMaker.Domain.Repositories;
using AsciiTreeMaker.Infrastructure.Json;
using AsciiTreeMaker.UI.ViewModels;
using AsciiTreeMaker.UI.Views;
using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using System.Windows;

namespace AsciiTreeMaker.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            AppSettings? appSettings = configuration.Get<AppSettings>();
            if (appSettings == null)
            {
                return;
            }

            Shared.Initialize(appSettings);
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<RenameView, RenameViewModel>();
        }
    }
}
