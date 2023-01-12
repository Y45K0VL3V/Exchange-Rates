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

namespace yakov.ExchangeRates.Client.FiatCurrency.ViewModels
{
    public class FiatRateContentViewModel : BindableBase
    {
        public FiatRateContentViewModel()
        {
            InitDateBorders();

            List<DateTimePoint> values = new()
            {
                new(new(2021,9,15), 8),
                new(new(2021,9,16), 9),
                new(new(2021,9,17), 10),
                new(new(2021,9,18), 9),
                new(new(2021,9,19), 9),
                new(new(2021,9,20), 9),
                new(new(2021,9,21), 9),
                new(new(2021,9,22), 9),
                new(new(2021,9,23), 9),
                new(new(2021,9,24), 9),
                new(new(2021,9,25), 9),
            };

            Rates = new ObservableCollection<ISeries>()
            {
                new LineSeries<DateTimePoint>()
                {
                    TooltipLabelFormatter = (chartPoint) => $"{new DateTime((long) chartPoint.SecondaryValue):dd.MM.yy}: {chartPoint.PrimaryValue}",
                    Values = values,
                    Fill = null,
                }
            };
        }

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

        //private readonly ObservableCollection<DateTimePoint> _observableValues;
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

    }
}
