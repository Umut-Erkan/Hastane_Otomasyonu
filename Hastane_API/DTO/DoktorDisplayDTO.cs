using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hastane_Otomasyonu.DTO
{
    public class DoktorDisplayDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Eposta { get; set; }
        public string Alan { get; set; }
    }
}