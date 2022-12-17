namespace yakov.ExchangeRates.Server.Domain.Interfaces
{
    public interface IStorageService
    {
        public void CreateFile(string path);
        public List<string> GetAllPaths();
    }
}
