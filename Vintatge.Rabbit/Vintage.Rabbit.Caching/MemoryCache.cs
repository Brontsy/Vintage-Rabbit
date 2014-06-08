using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Cache;

namespace Vintage.Rabbit.Caching
{
    internal class MemoryCache : ICacheService
    {
        System.Runtime.Caching.MemoryCache _cache;
        public MemoryCache()
        {
            _cache = new System.Runtime.Caching.MemoryCache("VintageRabbit");
        }

        public bool Exists(string cacheKey)
        {
            return false;
            //return this._cache[cacheKey] != null;
        }

        public T Get<T>(string cacheKey)
        {
            return default(T);
            //return (T)this._cache[cacheKey];
        }

        public void Add(string cacheKey, object obj)
        {
            //this._cache.Add(cacheKey, obj, new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromDays(7) });
        }
    }
}
