using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hastane_Otomasyonu.DTO
{
    public class ReceteDisplayDTO
    {
        public string HastaName { get; set; }
        public string DoktorName { get; set; }
        public string DoktorSurname { get; set; }
        public string Tanı { get; set; }
    }
}