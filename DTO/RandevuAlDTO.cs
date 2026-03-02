using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.DTO
{
    public class RandevuAlDTO
    {
        public int Tc { get; set; }

        public int Id { get; set; }

        public int? HastaId { get; set; }

        public int DoktorId { get; set; }

        public string DoktorName { get; set; }

        public string DoktorSurname { get; set; }


        public virtual Doktor? Doktor { get; set; }
    }
}