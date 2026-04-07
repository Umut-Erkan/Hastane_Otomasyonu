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
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConnectionMultiplexer redisConnection, ILogger<RedisCacheService> logger)
        {
            _redisConnection = redisConnection;
            _cache = redisConnection.GetDatabase();
            _logger = logger;
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

        public string GetValue()
        {
            try
            {
                var searchResult = _cache.FT().Search("idx:doktor_idx", new Query("@Role:{Doktor}"));

                var doktorlar = new List<DoktorDisplayDTO>();

                foreach (var doc in searchResult.Documents)
                {
                    // Her bir belgedeki propertyleri okuyup listeye ekliyoruz
                    var dto = new DoktorDisplayDTO
                    {
                        Id = (int)doc["id"],
                        Name = (string)doc["name"],
                        Surname = (string)doc["surname"],
                        Eposta = (string)doc["eposta"],
                        Alan = (string)doc["alan"]
                    };
                    doktorlar.Add(dto);
                }

                _logger.LogInformation("Redis uzerinden basariyla {Count} doktor okundu.", doktorlar.Count);

                return JsonSerializer.Serialize(doktorlar);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis'ten doktor verileri getirilirken hata olustu.");
                return new Exception(ex.Message).ToString();
            }
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
