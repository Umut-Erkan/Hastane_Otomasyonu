using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Hastane_Otomasyonu.Models;
using Microsoft.IdentityModel.Tokens;
using MyApiProject.Models;


namespace Hastane_Otomasyonu.Business
{
    public class TokenService
{
    private readonly HastaneContext _context;
    private readonly IConfiguration _config;
    public TokenService(IConfiguration config , HastaneContext context)
    {
        _config = config;

        _context = context;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        return Convert.ToBase64String(randomNumber);
    }

    public string GenerateAccessToken(IUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:jwtKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Eposta),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // JİT üretmek için
        };

        var token = new JwtSecurityToken(
            _config["JwtSettings:jwtIssuer"],
            _config["JwtSettings:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(90),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool VerifyAccessToken(JwtSecurityToken token)
    {
        if(token.ValidTo < DateTime.UtcNow)
        {
            var userIdString = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            
            var role = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var owner = _context.Users.Find(int.Parse(userIdString));
            

            if(role == "Doktor")
            {
                owner = _context.Doktors.Find(Id == int.Parse(userIdString));
            }
            else if(role == "Hasta")
            {
                owner = _context.Hasta.Find(Id == int.Parse(userIdString));
            }
            
            if(owner == null && owner.Token == null)
            {
                return false;
            }
            return true;
        }   
        return true;
    }


    // Access tokenin ömrü bitince 
    // sistem frontendde gelen Refresh token ile access tokenin sahibinin (ıd den bakar)
    // verei tabnındaki refresh tokenine bakar. ikisi eşleşirse yeni access token üretir.
    
    


    

}
}