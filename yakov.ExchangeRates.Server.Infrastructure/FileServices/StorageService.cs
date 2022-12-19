using System.IO;
using System.Text.Json;
using yakov.ExchangeRates.Server.Domain.Entities;
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

        public void CreateFile(string relatePath)
        {
            relatePath = Path.Combine(CachePath, relatePath ?? string.Empty);
            var subdirChain = relatePath[..relatePath.LastIndexOf('\\')];

            try
            {
                Directory.CreateDirectory(subdirChain);
            }
            catch { }
            finally
            {
                File.Create(relatePath).Close();
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

        public async Task AppendFileTextAsync(string relatePath, string textToAppend)
        {
            await File.WriteAllTextAsync(relatePath, textToAppend);
        }

        public async Task<string> ReadFileTextAsync(string relatePath)
        {
            return await File.ReadAllTextAsync(relatePath);
        }
    }
}
