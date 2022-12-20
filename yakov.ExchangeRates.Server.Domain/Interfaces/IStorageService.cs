namespace yakov.ExchangeRates.Server.Domain.Interfaces
{
    public interface IStorageService
    {
        public void CreateFile(string relatePath);
        public Task WriteFileTextAsync(string relatePath, string textToAppend);
        public Task<string> ReadFileTextAsync(string relatePath);
        public List<string> GetAllPaths();
    }
}
