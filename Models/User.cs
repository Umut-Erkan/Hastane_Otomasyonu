using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;


namespace Hastane_Otomasyonu.Models
{
    public class User
    {

        private readonly IConfiguration _config;
        public User(IConfiguration config)
        {
            _config = config;
        }

        public string İsim { get; set; }
        public string Soyisim { get; set; }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:jwtKey"])); 
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier , user.İsim),
                new Claim(ClaimTypes.Surname , user.Soyisim)
            };
            var token = new JwtSecurityToken(
                _config["JwtSettings:jwtIssuer"], 
                _config["JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            var Token = new JwtSecurityTokenHandler().WriteToken(token);

            return Token;

        }
    }
}