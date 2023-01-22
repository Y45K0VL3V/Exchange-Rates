using Prism.Mvvm;

namespace yakov.ExchangeRates.Client.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Exchange rates";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public MainWindowViewModel()
        {

        }
    }
}
