using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.DTO
{
    public class TedaviYazDTO
    {
        public string Tanı { get; set; }
        public string Tedavi { get; set; }
        public string Recete { get; set; }
        public int HastaID { get; set; }
    }
}