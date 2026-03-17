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

        return (Token: Convert.ToBase64String(randomNumber), Expiration: turkeyTime.AddMinutes(5));
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
            expires: DateTime.UtcNow.AddMinutes(2),
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);
    }

// Access Tokenin ömrü bitince zaten geçerliliği kalmıyor. Dolayısıyla revoke etmek (yetkisini düşürmek) gerekmez.



    // Access tokenin ömrü bitince 
    // sistem frontendde gelen Refresh token ile access tokenin sahibinin (ıd den bakar)
    // verei tabnındaki refresh tokenine bakar. ikisi eşleşirse yeni access token üretir.
    
    


    

}
}