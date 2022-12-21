using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yakov.ExchangeRates.Server.Domain.Interfaces;

namespace yakov.ExchangeRates.Server.Infrastructure.ValidationServices
{
    public class TimePeriodValidatorService : ITimePeriodValidator
    {
        // Difference set in years
        private readonly int _maxOldDifference = 5;
        public void Validate(ref DateOnly dateStart, ref DateOnly dateEnd)
        {
            DateOnly oldestDateAllowed = DateOnly.FromDateTime(DateTime.Now.AddYears(-1 * _maxOldDifference));
            if (dateStart < oldestDateAllowed)
                dateStart = oldestDateAllowed;

            DateOnly latestDateAllowed = DateOnly.FromDateTime(DateTime.Now);
            if (dateStart > DateOnly.FromDateTime(DateTime.Now))
                dateStart = latestDateAllowed;

            if (dateEnd < dateStart)
                dateEnd = dateStart;

            if ((dateEnd.ToDateTime(new()) - dateStart.ToDateTime(new())).TotalDays > 365)
                dateEnd = dateStart.AddDays(365);

            if (dateEnd > DateOnly.FromDateTime(DateTime.Now))
                dateEnd = latestDateAllowed;
        }
    }
}
