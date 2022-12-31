using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ExchangeRates.Client.Services.Interfaces
{
    public interface ITimePeriodValidator
    {
        public void Validate(DateOnly dateStart, DateOnly dateEnd);
    }
}
