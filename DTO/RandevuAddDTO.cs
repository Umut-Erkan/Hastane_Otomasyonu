using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.DTO
{
    public class RandevuAddDTO
    {
        public long Tc { get; set; }

        public int Id { get; set; }

        public string Şikayet { get; set; }

        public string DoktorName { get; set; }

        public string DoktorSurname { get; set; }

        public DateOnly Tarih { get; set; }

        public TimeOnly Saat { get; set; }

    }
}