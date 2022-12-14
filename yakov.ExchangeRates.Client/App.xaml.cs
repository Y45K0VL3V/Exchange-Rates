using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using yakov.ExchangeRates.Client.FiatCurrency;
using yakov.ExchangeRates.Client.Views;

namespace yakov.ExchangeRates.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<FiatCurrencyModule>();
        }
    }
}
