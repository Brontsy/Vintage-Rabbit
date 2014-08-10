using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Cache;

namespace Vintage.Rabbit.Caching
{
    internal class RequestCache : ICacheService
    {
        private static Dictionary<string, object> _cache;

        static RequestCache()
        {
            _cache = new Dictionary<string, object>();
        }

        public RequestCache()
        {
            
        }

        public IList<string> Keys()
        {
            return _cache.Keys.ToList();
        }

        public bool Exists(string cacheKey)
        {
            return _cache.ContainsKey(cacheKey);
        }

        public T Get<T>(string cacheKey)
        {
            if (_cache.ContainsKey(cacheKey))
            {
                return (T)_cache[cacheKey];
            }

            return default(T);
        }

        public void Add(string cacheKey, object obj)
        {
            if (_cache.ContainsKey(cacheKey))
            {
                _cache[cacheKey] = obj;
            }
            else
            {
                _cache.Add(cacheKey, obj);
            }
        }

        public void Remove(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }
    }
}
