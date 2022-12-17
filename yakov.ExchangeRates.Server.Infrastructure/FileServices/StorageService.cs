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
            var subdirChain = path[..path.LastIndexOf('\\')];

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

        public List<string> GetAllPaths()
        {
            List<string> path = new();

            try
            {
                path.AddRange(Directory.GetFiles(CachePath, "*", SearchOption.AllDirectories));
            }
            catch { }

            return path;
        }
    }
}
