using System;
using System.Collections.Generic;
using Hastane_Otomasyonu.DTO;
using StackExchange.Redis;

namespace Hastane_Otomasyonu.Redis.Interfaces
{
    public interface IRedisCacheService
    {
        List<DoktorDisplayDTO> GetValue();
        bool SetDoktor(string key, DoktorDisplayDTO dto);
        void Clear(string key);
        void ClearAll();
        List<DoktorDisplayDTO> GetValueByAlan(string alan);
    }
}