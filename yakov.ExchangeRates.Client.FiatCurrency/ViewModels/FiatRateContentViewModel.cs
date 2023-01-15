using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.Configuration;
using yakov.ExchangeRates.Client.Business;
using yakov.ExchangeRates.Client.Services;
using yakov.ExchangeRates.Client.Services.Interfaces;
using System.Linq;
using yakov.ExchangeRates.Client.FiatCurrency.Extensions;

namespace yakov.ExchangeRates.Client.FiatCurrency.ViewModels
{
    public class FiatRateContentViewModel : BindableBase
    {
        protected IRatesService RatesService { get; private set; }

        public FiatRateContentViewModel(IRatesService ratesService)
        {
            RatesService = ratesService;
            InitDateBorders();

            Rates = new ObservableCollection<ISeries>()
            {
                new LineSeries<DateTimePoint>()
                {
                    TooltipLabelFormatter = (chartPoint) => $"{new DateTime((long) chartPoint.SecondaryValue):dd.MM.yy}: {chartPoint.PrimaryValue}",
                    Values = _observableValues,
                    Fill = null,
                    LineSmoothness = 0,
                    GeometrySize = 1,
                }
            };
        }

        #region Dates control
        private DateTime _minDate;
        public DateTime MinDate
        {
            get => _minDate;
            set => SetProperty(ref _minDate, value);
        }

        private DateTime _maxDate;
        public DateTime MaxDate
        {
            get => _maxDate;
            set => SetProperty(ref _maxDate, value);
        }

        private void InitDateBorders()
        {
            MinDate = DateTime.Now.AddYears(-5);
            MaxDate = DateTime.Now;
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }
        
        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }
        #endregion

        private readonly ObservableCollection<DateTimePoint> _observableValues = new();
        public ObservableCollection<ISeries> Rates { get; set; }

        public ObservableCollection<Currency> Currencies { get; set; }

        #region Live chart UI bindings
        public Axis[] XAxes { get; set; } =
        {
            new Axis
            {
                Labeler = value => new DateTime((long) value).ToString("dd.MM.yy"),
                LabelsRotation = 0,
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinStep = TimeSpan.FromDays(1).Ticks
            }
        };
        #endregion

        #region Currency type control
        private CurrencyType _currencyType = CurrencyType.Fiat;

        public CurrencyType CurrencyType
        {
            get { return _currencyType; }
            set
            {
                if (_currencyType == value)
                    return;

                SetProperty(ref _currencyType, value);
                RaisePropertyChanged(nameof(IsFiatCurrency));
                RaisePropertyChanged(nameof(IsCryptoCurrency));
            }
        }

        public bool IsFiatCurrency
        {
            get { return CurrencyType == CurrencyType.Fiat; }
            set { CurrencyType = value ? CurrencyType.Fiat : CurrencyType; }
        } 
        public bool IsCryptoCurrency
        {
            get { return CurrencyType == CurrencyType.Crypto; }
            set { CurrencyType = value ? CurrencyType.Crypto : CurrencyType; }
        }
        #endregion

        private string _currencyShortName;
        public string CurrencyShortName
        {
            get => _currencyShortName;
            set => SetProperty(ref _currencyShortName, value);
        }

        private DelegateCommand _getRatesCommand;
        public DelegateCommand GetRatesCommand =>
            _getRatesCommand ??= new DelegateCommand(ExecuteGetRates);
        
        private async void ExecuteGetRates()
        {
            Currency chosedCurrency = new() { ShortName = CurrencyShortName, Type = CurrencyType };
            var rates = await RatesService.GetRates(chosedCurrency, DateOnly.FromDateTime(StartDate.Value), DateOnly.FromDateTime(EndDate.Value));

            _observableValues.Clear();
            rates.ForEach(r => _observableValues.Add(r.ToDateTimePoint()));
        }
    }
}
