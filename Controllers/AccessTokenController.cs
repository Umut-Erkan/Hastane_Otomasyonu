using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hastane_Otomasyonu.Business;
using MyApiProject.Models;
using Hastane_Otomasyonu.Models;

namespace Hastane_Otomasyonu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessTokenController : ControllerBase
    {

        private readonly TokenService _tokenService;
        private readonly HastaneContext _context;
        public AccessTokenController(TokenService tokenService, HastaneContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("Regenerate")]
        public IActionResult RegenerateAccessToken([FromBody] string refreshToken)
        {
            // Doktor ve Hastum farklı tipler olduğundan '??' kullanabilmek için ikisi de IUser'a cast ediliyor
            IUser user = (IUser)_context.Doktors.FirstOrDefault(x => x.RefreshToken == refreshToken) ?? 
                         (IUser)_context.Hasta.FirstOrDefault(x => x.RefreshToken == refreshToken);

            if (user == null)
                return Unauthorized(new { mesaj = "Geçersiz Refresh Token" });

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            
            // Kullanıcının AccessToken'ını güncelle
            if (user is Doktor doktor)
                doktor.AccessToken = newAccessToken;
                
            else if (user is Hastum hastum)
                hastum.AccessToken = newAccessToken;

            _context.SaveChanges();

            return Ok(new 
            {
                accessToken = newAccessToken,
                expiresIn = "30 dakika"
            });
        }   
    }
}