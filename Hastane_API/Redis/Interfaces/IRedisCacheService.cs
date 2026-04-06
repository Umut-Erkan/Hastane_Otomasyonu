using System;
using Hastane_Otomasyonu.DTO;
using StackExchange.Redis;

namespace Hastane_Otomasyonu.Redis.Interfaces
{
    public interface IRedisCacheService
    {
        string GetValue(string key);
        bool SetDoktor(string key, DoktorDisplayDTO dto);
        void Clear(string key);
        void ClearAll();
    }
}