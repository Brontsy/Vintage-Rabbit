using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Configuration;
using Vintage.Rabbit.Common.Serialization;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Logging;
using System.Collections.Generic;

namespace Vintage.Rabbit.Caching
{
    public class RedisCache : ICacheService
    {
        private ISerializer _serialzer;
        private ILogger _logger;
        private ConnectionMultiplexer _connection;
        private string _redisConnectionString;
        private string _redisHost;
        private string _redisPort;
        private const int db = 5;

        private readonly object _lock = new object();

        public RedisCache(ISerializer serialzer, ILogger logger)
        {
            this._serialzer = serialzer;
            this._logger = logger;
            this._redisConnectionString = ConfigurationManager.AppSettings["SettingsClient_RedisConnection"];
            this._redisHost = ConfigurationManager.AppSettings["SettingsClient_RedisHost"];
            this._redisPort = ConfigurationManager.AppSettings["SettingsClient_RedisPort"];
        }

        private void EnsureConnection()
        {
            if(_connection == null || !_connection.IsConnected)
            {
                lock(_lock)
                {
                    if(_connection == null)
                    {
                        _connection = ConnectionMultiplexer.Connect(this._redisConnectionString);
                    }
                    else if(!_connection.IsConnected)
                    {
                        _connection.Dispose();
                        _connection = ConnectionMultiplexer.Connect(this._redisConnectionString);
                    }
                }
            }
        }

        public IList<string> Keys()
        {
            this.EnsureConnection();
            IList<string> cacheKeys = new List<string>();

            var endpoints = this._connection.GetEndPoints();

            foreach(var endpoint in endpoints)
            {
                var keys = this._connection.GetServer(endpoint).Keys();
                foreach(var key in keys)
                {
                    if(!cacheKeys.Contains(key))
                    {
                        cacheKeys.Add(key);
                    }
                }
            }

            return cacheKeys;
        }

        public T Get<T>(string key)
        {
            try
            {
                this.EnsureConnection();
                var database = this._connection.GetDatabase(db);
                string json = database.StringGet(key, CommandFlags.PreferSlave);

                T myCachedObject = this._serialzer.Deserialize<T>(json);

                return myCachedObject;
            }
            catch (Exception e)
            {
                this._logger.Error(e, "Unable to get item from cache: " + key);
            }

            return default(T);
        }

        public void Add(string key, object objectToCache)
        {

            try
            {
                if (objectToCache != null)
                {

                    this.EnsureConnection();
                    var database = this._connection.GetDatabase(db);

                    string json = this._serialzer.Serialize(objectToCache);

                    if (!string.IsNullOrEmpty(json))
                    {
                        database.StringSet(key, json, TimeSpan.FromHours(24), When.Always, CommandFlags.PreferMaster | CommandFlags.FireAndForget);
                    }
                }
            }
            catch (Exception e)
            {
                this._logger.Error(e, "Unable to add item to cache: " + key);
            }
        }

        public bool Add(string key, object objectToCache, DateTime absoluteExpiration)
        {
            try
            {
                if (objectToCache != null)
                {
                    this.EnsureConnection();
                    var database = this._connection.GetDatabase(db);

                    string json = this._serialzer.Serialize(objectToCache);

                    if (!string.IsNullOrEmpty(json))
                    {
                        TimeSpan expiry = absoluteExpiration.Subtract(DateTime.Now);

                        database.StringSet(key, json, expiry, When.Always, CommandFlags.PreferMaster | CommandFlags.FireAndForget);
                    }
                }
            }
            catch (Exception e)
            {
                this._logger.Error(e, "Unable to add item to cache: " + key);
            }

            return false;
        }

        public void Remove(string key)
        {
            try
            {
                this.EnsureConnection();
                var database = this._connection.GetDatabase(db);
                database.KeyDelete(key);
            }
            catch (Exception e)
            {
                this._logger.Error(e, "Unable to remove item from cache: " + key);
            }
        }

        public bool Exists(string key)
        {
            try
            {
                this.EnsureConnection();
                var database = this._connection.GetDatabase(db);
                return database.KeyExists(key);
            }
            catch (Exception e)
            {
                this._logger.Error(e, "Unable to check if item exists in cache: " + key);
            }

            return false;
        }

    }
}
