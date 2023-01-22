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
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Drawing;

namespace yakov.ExchangeRates.Client.FiatCurrency.ViewModels
{
    public class FiatRateContentViewModel : BindableBase
    {
        protected IRatesService RatesService { get; private set; }

        public FiatRateContentViewModel(IRatesService ratesService)
        {
            RatesService = ratesService;
            InitDateBorders();

            UpdateCurrencies(CurrencyType.Fiat);

            Rates = new ObservableCollection<ISeries>()
            {
                new LineSeries<DateTimePoint>()
                {
                    TooltipLabelFormatter = (chartPoint) => $"{new DateTime((long) chartPoint.SecondaryValue):dd.MM.yy}: {chartPoint.PrimaryValue}",
                    Values = _observableValues,
                    Fill = new SolidColorPaint(SKColor.Parse("#5F000000")),
                    LineSmoothness = 0,
                    GeometrySize = 5,
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#00000000")),
                    GeometryFill = new SolidColorPaint(SKColor.Parse("#4ADAEC")),
                    Stroke = new SolidColorPaint(SKColor.Parse("#07F3C0")) { StrokeThickness = 3 },
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

        #region Live chart control
        public Axis[] XAxes { get; set; } =
        {
            new Axis
            {
                Labeler = value => new DateTime((long) value).ToString("dd.MM.yy"),
                LabelsRotation = 0,
                LabelsPaint = new SolidColorPaint(SKColor.Parse("#D5CFF5")),
                UnitWidth = TimeSpan.FromDays(3).Ticks,
                MinStep = TimeSpan.FromDays(2).Ticks,
                ForceStepToMin = false,
            }
        }; 
        
        public Axis[] YAxes { get; set; } =
        {
            new Axis
            {
                Name = "BYN",
                NamePaint = new SolidColorPaint(SKColor.Parse("#D5CFF5")),
                LabelsPaint = new SolidColorPaint(SKColor.Parse("#D5CFF5")),
            }
        };

        public void ClearChart()
        {
            _observableValues.Clear();
            Axis x = XAxes.First();
            x.MinLimit = null;
            x.MaxLimit = null;
        }
        #endregion

        #region Currency type control
        private static Dictionary<CurrencyType, List<string>> _currencyTypeToCurrencyNames = new();
        public ObservableCollection<string> Currencies { get; set; } = new();

        private CurrencyType _currencyType;

        public CurrencyType CurrencyType
        {
            get => _currencyType;
            set
            {
                if (_currencyType == value)
                    return;

                switch (value)
                {
                    case CurrencyType.Fiat:
                        YAxes.FirstOrDefault().Name = "BYN";
                        break;
                    case CurrencyType.Crypto:
                        YAxes.FirstOrDefault().Name = "USD";
                        break;
                }

                ClearErrorMessage();
                UpdateCurrencies(value);

                SetProperty(ref _currencyType, value);
                RaisePropertyChanged(nameof(IsFiatCurrency));
                RaisePropertyChanged(nameof(IsCryptoCurrency));
            }
        }

        public bool IsFiatCurrency
        {
            get => CurrencyType == CurrencyType.Fiat;
            set => CurrencyType = value ? CurrencyType.Fiat : CurrencyType;
        } 
        public bool IsCryptoCurrency
        {
            get => CurrencyType == CurrencyType.Crypto;
            set => CurrencyType = value ? CurrencyType.Crypto : CurrencyType;
        }

        private async void UpdateCurrencies(CurrencyType currencyType)
        {
            if (!_currencyTypeToCurrencyNames.ContainsKey(currencyType))
                _currencyTypeToCurrencyNames.Add(currencyType, new());
            if (_currencyTypeToCurrencyNames[currencyType].Count == 0)
                _currencyTypeToCurrencyNames[currencyType] = await RatesService.GetCurrencyNames(currencyType);

            Currencies.Clear();
            Currencies.AddRange(_currencyTypeToCurrencyNames[currencyType]);
        }
        #endregion

        private string _currencyShortName;
        public string CurrencyShortName
        {
            get => _currencyShortName;
            set => SetProperty(ref _currencyShortName, value);
        }

        private DelegateCommand _getRatesCommand;
        public DelegateCommand GetRatesCommand => _getRatesCommand ??= new DelegateCommand(ExecuteGetRates);

        private bool _isRatesLoading = false;
        public bool IsRatesLoading
        {
            get => _isRatesLoading;
            set => SetProperty(ref _isRatesLoading, value);
        }

        private async void ExecuteGetRates()
        {
            Currency chosedCurrency = new() { ShortName = CurrencyShortName, Type = CurrencyType };
            List<Rate> rates = new();
            try
            {
                ClearErrorMessage();
                IsRatesLoading = true;
                rates = await RatesService.GetRates(chosedCurrency, DateOnly.FromDateTime(StartDate.Value), DateOnly.FromDateTime(EndDate.Value));
                IsRatesLoading = false;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            ClearChart();
            rates.ForEach(r => _observableValues.Add(r.ToDateTimePoint()));
        }

        #region Errors control

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private void ClearErrorMessage()
        {
            ErrorMessage = null;
        }

        #endregion
    }
}
