using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Hastane_Otomasyonu.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace Hastane_Otomasyonu.Models
{
    public class User 
    {
        public string İsim { get; set; }
        public string Soyisim { get; set; }
        public string Password { get; set; }
        public string Eposta { get; set; }
        public long Tc { get; set; }
        public string Role { get; set; }
    }
}
