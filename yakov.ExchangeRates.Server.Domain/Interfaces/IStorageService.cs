using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yakov.ExchangeRates.Server.Domain.Entities;

namespace yakov.ExchangeRates.Server.Domain.Interfaces
{
    public interface IStorageService
    {
        public void CreateFile(string path);
    }
}
