using System;
using StackExchange.Redis;

namespace Hastane_Otomasyonu.Redis.Interfaces
{
    public interface IRedisCacheService
    {
        string GetValue(string key);
        bool SetValue(string key, string value);
        void Clear(string key);
        void ClearAll();
    }
}