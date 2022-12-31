using yakov.ExchangeRates.Client.Services.Interfaces;

namespace yakov.ExchangeRates.Client.Services
{
    public class TimePeriodValidatorService : ITimePeriodValidator
    {
        // Difference set in years
        private readonly int _maxOldDifference = 5;
        public void Validate(DateOnly dateStart, DateOnly dateEnd)
        {
            DateOnly oldestDateAllowed = DateOnly.FromDateTime(DateTime.Now.AddYears(-1 * _maxOldDifference));
            DateOnly latestDateAllowed = DateOnly.FromDateTime(DateTime.Now);
            if (dateStart < oldestDateAllowed ||
                dateStart > DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException("Invalid start date");

            if (dateEnd < dateStart)
                throw new ArgumentException("Invalid end date");

            if ((dateEnd.ToDateTime(new()) - dateStart.ToDateTime(new())).TotalDays > 365)
                throw new ArgumentException("Maximum time period = 1 year");

            if (dateEnd > DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException("Invalid end date");
        }
    }
}
