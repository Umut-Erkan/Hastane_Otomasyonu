using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hastane_Otomasyonu.DTO
{
    public class DoktorAddDTO
    {
        public long Tc { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Eposta { get; set; } //get; init; bak
        public string Alan { get; set; }
    }
}