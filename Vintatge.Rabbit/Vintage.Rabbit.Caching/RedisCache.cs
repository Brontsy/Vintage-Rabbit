using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Configuration;
using Vintage.Rabbit.Common.Serialization;
using Vintage.Rabbit.Interfaces.Cache;

namespace Vintage.Rabbit.Caching
{
    public class RedisCache : ICacheService
    {
        private ISerializer _serialzer;

 private ConnectionMultiplexer _connection;
        private IDatabase _database;
        private const int db = 4;
        private bool isFucked = false;

        public RedisCache(ISerializer serialzer)
        {
            this._serialzer = serialzer;
            if (_connection == null || _connection.IsConnected == false)
            {
                var redisConnection = ConfigurationManager.AppSettings["SettingsClient_RedisConnection"];
                _connection = ConnectionMultiplexer.Connect(redisConnection);
            }
            _database = _connection.GetDatabase();
            
        }

        public T Get<T>(string key)
        {
            if (isFucked)
            {
                return default(T);
            }

            try
            {
                string json = this._database.StringGet(key, CommandFlags.PreferSlave);

                T myCachedObject = this._serialzer.Deserialize<T>(json);

                return myCachedObject;
            }
            catch (Exception e)
            {
                isFucked = true;
            }

            return default(T);
        }

        public void Add(string key, object objectToCache)
        {
            if (isFucked)
            {
                return;
            }

            try
            {
                if (objectToCache != null)
                {

                    string json = this._serialzer.Serialize(objectToCache);

                    if (!string.IsNullOrEmpty(json))
                    {
                        this._database.StringSet(key, json, null, When.Always, CommandFlags.PreferMaster | CommandFlags.FireAndForget);
                    }
                }
            }
            catch (Exception e)
            {
                isFucked = true;
            }
        }

        public bool Add(string key, object objectToCache, DateTime absoluteExpiration)
        {
            if (isFucked)
            {
                return false;
            }

            try
            {
                if (objectToCache != null)
                {
                    string json = this._serialzer.Serialize(objectToCache);

                    if (!string.IsNullOrEmpty(json))
                    {
                        TimeSpan expiry = absoluteExpiration.Subtract(DateTime.Now);

                        return this._database.StringSet(key, json, expiry, When.Always, CommandFlags.PreferMaster | CommandFlags.FireAndForget);
                    }
                }
            }
            catch (Exception e)
            {
                isFucked = true;
            }

            return false;
        }

        public bool Add(string key, object objectToCache, CacheItemPolicy policy)
        {
            return false;
        }

        public void Remove(string key)
        {
            if (isFucked)
            {
                return;
            }

            try
            {
                this._database.KeyDelete(key, CommandFlags.PreferMaster);
            }
            catch (Exception e)
            {
                isFucked = true;
            }
        }

        public bool Exists(string key)
        {
            if (isFucked)
            {
                return false;
            }
            
            try
            {
                return this._database.KeyExists(key);
            }
            catch (Exception e)
            {
                isFucked = true;
            }

            return false;
        }

    }
}
