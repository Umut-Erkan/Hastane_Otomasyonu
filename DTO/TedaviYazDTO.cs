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
        public int HastaID { get; set; }
    }

    public class TedaviRequestDTO
    {
        public ReceteDTO ReceteDTO { get; set; }
        public TedaviYazDTO TedaviDTO { get; set; }
        public int HastaID { get; set; }
    }
}
