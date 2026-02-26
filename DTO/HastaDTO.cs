using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hastane.Models;

namespace Hastane_Otomasyonu.DTO
{
    public class HastaDTO
    {
        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Şikayet { get; set; }

        public virtual OnlineRandevu? Randevu { get; set; } // Şimdilik burasını boş bırak


        public static HastaDTO ConvertToDTO(Hastum hasta) // Veri get edilirken entity -> DTO için.
        {
            return new HastaDTO
            {
                Name = hasta.İsim,
                Surname = hasta.Soyisim,
                Şikayet = hasta.Şikayet,
                Randevu = hasta.OnlineRandevu
            };
        }

    }
}