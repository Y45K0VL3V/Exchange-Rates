namespace yakov.ExchangeRates.Client.Services.Interfaces
{
    public interface ITimePeriodValidator
    {
        public void Validate(DateOnly dateStart, DateOnly dateEnd);
    }
}
