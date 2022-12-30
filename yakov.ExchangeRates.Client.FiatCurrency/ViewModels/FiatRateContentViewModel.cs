using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;

namespace yakov.ExchangeRates.Client.FiatCurrency.ViewModels
{
    public class FiatRateContentViewModel : BindableBase
    {
        //public ISeries[] Series { get; set; } =
        //{
        //new LineSeries<ObservablePoint>
        //{
        //    Values = new ObservablePoint[]
        //    {
        //        new ObservablePoint(0, 4),
        //        new ObservablePoint(1, 3),
        //        new ObservablePoint(3, 8),
        //        new ObservablePoint(18, 6),
        //        new ObservablePoint(20, 12)
        //    }
        //}
        //};
        private readonly ObservableCollection<ObservableValue> _observableValues;

        public FiatRateContentViewModel(ObservableCollection<ObservableValue> observableValues, ObservableCollection<ISeries> series)
        {
            _observableValues = new ObservableCollection<ObservableValue>
            {
                new ObservableValue(2),
                new(5),
                new(4),
                new(5),
                new(2),
                new(6),
                new(6),
                new(6),
                new(4),
                new(2),
                new(3),
                new(4),
                new(3)
            };

            Series = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservablePoint>()
                {
                    Values = new ObservablePoint[]
                    {
                        new ObservablePoint(0, 4),
                        new ObservablePoint(1, 3),
                        new ObservablePoint(3, 8),
                        new ObservablePoint(18, 6),
                        new ObservablePoint(20, 12)
                    },
                    Fill = null
                }
            };
        }

        public ObservableCollection<ISeries> Series { get; set; }

    }
}
