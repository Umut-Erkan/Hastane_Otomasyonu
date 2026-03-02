using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Hastane_Otomasyonu.DTO
{
    public class RegistirationDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}