using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hastane_Otomasyonu.DTO
{
    public class GetRefreshTokenDTO
    {
        public string Eposta { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}