using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.DTO
{
    public class HastaAddDTO
    {
        public long Tc { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Password { get; set; }

        public string Eposta { get; set; }

        public string? Şikayet { get; set; } //get; init; bak
        public string Role { get; set; }

        public virtual OnlineRandevu? Randevu { get; set; }
        public virtual Tedavi? Tedavi { get; set; }

    }

}
