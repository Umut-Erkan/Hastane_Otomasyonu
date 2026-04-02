using System;
using StackExchange.Redis;

using Hastane_Otomasyonu.Redis.Interfaces;

namespace Hastane_Otomasyonu.Redis.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redisConnection; //Redis sunucusu ile aradaki bağlantıyı yönetir. kopmalar falan
        private readonly IDatabase _cache; //Redis veritabanına erişip işlem yapmamızı sağlar.

        public RedisCacheService(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection; 
            _cache = redisConnection.GetDatabase();
        }

        public void Clear(string key)
        {
            _cache.KeyDelete(key);
        }

        public void ClearAll()
        {
           var redisEndpoints = _redisConnection.GetEndPoints(true);
            foreach (var redisEndpoint in redisEndpoints)
            {
                var redisServer = _redisConnection.GetServer(redisEndpoint);
                redisServer.FlushAllDatabases();
            }
        }

        public string GetValue(string key)
        {
            return _cache.StringGet(key);
        }

        public bool SetValue(string key, string value)
        {
            return _cache.StringSet(key, value, TimeSpan.FromHours(1));
        }
    }
}