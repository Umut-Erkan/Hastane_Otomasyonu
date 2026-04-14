using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Hastane_Otomasyonu.DTO
{
    public class RecepsionistAddDTO
    {
        public long Tc { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Eposta => $"{Name?.Replace(" ", "")}{Surname?.Replace(" ", "")}hastane@posta.com".ToLower();
        public string Password { get; set; }
        public string Alan { get; set; }
    }
}