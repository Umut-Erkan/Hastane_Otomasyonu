using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Hastane_Otomasyonu.DTO;
using Hastane_Otomasyonu.Redis.Interfaces;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using StackExchange.Redis;

namespace Hastane_Otomasyonu.Redis.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IDatabase _cache;

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
            List<string> doktorlar = new List<string>();

            var rows = _cache.Execute("FT.SEARCH", "idx:doktor_idx", "@role:{Doktor}");

            var results = (RedisResult[])rows;


            foreach (var result in results)
            {
                doktorlar.Add(result.ToString());
            }

            return JsonSerializer.Serialize(doktorlar);
        }

        public bool SetDoktor(string key, DoktorDisplayDTO jsonDto)
        {
            var redisKey = $"doktor:{key}";
            var hashEntries = new[]
            {
                new HashEntry("id", jsonDto.Id),
                new HashEntry("name", jsonDto.Name),
                new HashEntry("surname", jsonDto.Surname),
                new HashEntry("eposta", jsonDto.Eposta),
                new HashEntry("alan", jsonDto.Alan),
                new HashEntry("Role", "Doktor")
            };
            _cache.HashSet(redisKey, hashEntries);
            return true;
        }
    }
}
