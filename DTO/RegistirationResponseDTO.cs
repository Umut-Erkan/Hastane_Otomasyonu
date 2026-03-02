using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hastane_Otomasyonu.DTO
{
    public class RegistirationResponseDTO
    {
        public bool IsSuccesful { get; set; }
        public IEnumerable<string> MyProperty { get; set; }
    }
}