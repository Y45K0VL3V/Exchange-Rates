using yakov.ExchangeRates.Server.Domain.Interfaces;

namespace yakov.ExchangeRates.Server.Infrastructure.FileServices
{
    public class StorageService : IStorageService
    {
        public StorageService()
        {
            try
            {
                Directory.CreateDirectory(CachePath);
            }
            catch { throw; }
        }

        public readonly string CachePath = "Cache";

        public void CreateFile(string path)
        {
            path = Path.Combine(CachePath, path ?? string.Empty);
            var subdirChain = path.Substring(0, path.LastIndexOf('\\'));

            try
            {
                Directory.CreateDirectory(subdirChain);
            }
            catch { }
            finally
            {
                File.Create(path).Close();
            }
        }
    }
}
