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

    public (string Token, DateTime Expiration) GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        // Türkiye saati (UTC+3) için;
        DateTime turkeyTime = DateTime.UtcNow.AddHours(3);

        return (Token: Convert.ToBase64String(randomNumber), Expiration: turkeyTime.AddDays(7));
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
            expires: DateTime.UtcNow.AddHours(3).AddMinutes(5), 
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool VerifyAccessToken(JwtSecurityToken token)
    {
        if(token.ValidTo < DateTime.UtcNow) //Tokenin tarihi geçtiyse
        {
            var userIdString = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            

            if(role == "Doktor")
            {
                var owner = _context.Doktors.FirstOrDefault(x => x.Id == int.Parse(userIdString));
                
                if(owner.AccessToken == null) // Token sahibinin token kısmı Null ise
            {
                return false;
            }
            }

            else if(role == "Hasta")
            {
                var owner = _context.Hasta.FirstOrDefault(x => x.Id == int.Parse(userIdString));
                
                if(owner.AccessToken == null)
            {
                return false;
            }
            }
            return true;
        }   
        return true;
    }

    public string RegenerateAccessToken(IUser user)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(user.AccessToken);

        if (VerifyAccessToken(jwtToken) == false) // AccessToken geçerli değilse
        {
            if(user.RefreshToken != null) // Normalde frontendden gelen refresh token ile veritabanındaki refresh token eşleşirse
                                        // ama ben backendde  sadece null mu diye kontrol ettim
            {    
                // Yeni access token üret
                return GenerateAccessToken(user);
            }
        }
        return null;
    }


    // Access tokenin ömrü bitince 
    // sistem frontendde gelen Refresh token ile access tokenin sahibinin (ıd den bakar)
    // verei tabnındaki refresh tokenine bakar. ikisi eşleşirse yeni access token üretir.
    
    


    

}
}