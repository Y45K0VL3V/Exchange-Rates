using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ExchangeRates.Server.Domain.Interfaces
{
    public interface ITimePeriodValidator
    {
        public void Validate(ref DateOnly dateStart, ref DateOnly dateEnd);
    }
}
