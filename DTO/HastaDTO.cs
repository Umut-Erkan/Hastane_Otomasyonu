using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.DTO
{
    public class HastaDTO
    {
        public int Tc { get; set; }
        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string Password { get; set; }

        public string Eposta { get; set; }

        public string? Şikayet { get; set; } //get; init; bak

        public virtual OnlineRandevu? Randevu { get; set; } // Şimdilik burasını boş bırak
        public virtual Tedavi? Tedavi { get; set; } // Şimdilik burasını boş bırak

        }

    }
