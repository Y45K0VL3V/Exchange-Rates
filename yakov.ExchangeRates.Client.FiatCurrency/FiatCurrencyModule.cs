using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using yakov.ExchangeRates.Client.Core;
using yakov.ExchangeRates.Client.FiatCurrency.ViewModels;
using yakov.ExchangeRates.Client.FiatCurrency.Views;
using yakov.ExchangeRates.Client.Services;
using yakov.ExchangeRates.Client.Services.Interfaces;

namespace yakov.ExchangeRates.Client.FiatCurrency
{
    public class FiatCurrencyModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public FiatCurrencyModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.RatesContent, typeof(FiatRateContent));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<FiatRateContent, FiatRateContentViewModel>();
            containerRegistry.RegisterForNavigation<FiatRateContent, FiatRateContentViewModel>();

            containerRegistry.RegisterSingleton<IRatesService, RatesService>();

        }
    }
}