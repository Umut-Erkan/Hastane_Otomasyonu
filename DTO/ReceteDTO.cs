using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.DTO
{
    public class ReceteDTO
    {
        public string Kullanım { get; set; }

        //public DateOnly GecerlilikTarihi { get; set; }

        public List<int> IlacAdet { get; set; }

        public List<string> Ilaclar { get; set; }
    }
}