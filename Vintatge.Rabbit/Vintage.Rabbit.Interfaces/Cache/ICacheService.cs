using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Interfaces.Cache
{
    public interface ICacheService
    {
        bool Exists(string cacheKey);

        T Get<T>(string cacheKey);

        void Add(string cacheKey, object obj);

        void Remove(string cacheKey);
    }
}
