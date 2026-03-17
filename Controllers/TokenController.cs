using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hastane_Otomasyonu.Business;
using MyApiProject.Models;
using Hastane_Otomasyonu.Models;
using Hastane_Otomasyonu.DTO;   

namespace Hastane_Otomasyonu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {

        private readonly TokenService _tokenService;
        private readonly HastaneContext _context;
        public TokenController(TokenService tokenService, HastaneContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("AccessTokenRefresh")]
        public IActionResult RegenerateAccessToken([FromBody] string refreshToken)
        {
            // Doktor ve Hastum farklı tipler olduğundan '??' kullanabilmek için ikisi de IUser'a cast ediliyor
            IUser user = (IUser)_context.Doktors.FirstOrDefault(x => x.RefreshToken == refreshToken) ?? 
                         (IUser)_context.Hasta.FirstOrDefault(x => x.RefreshToken == refreshToken);

            if (user == null) 
                return Unauthorized(new { mesaj = "Geçersiz Refresh Token" });

            // REFRESH TOKEN SÜRESİ KOTNROL
            if (user.RefreshTokenEndDate < DateTime.Now)
            {
                return Unauthorized(new { mesaj = "Refresh Token süresi dolmuş" });
            }

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


        [HttpPost("RefreshToken")]
        public IActionResult RegenerateRefreshToken([FromBody] GetRefreshTokenDTO dto)
        {
            if (dto.Role == "Doktor")
            {
                var doktor = _context.Doktors.FirstOrDefault(x => x.Eposta == dto.Eposta && x.Password == dto.Password);
                if (doktor == null)
                    return Unauthorized(new { mesaj = "Geçersiz E-posta veya Şifre" });

                var (refreshToken, refreshTokenEndDate) = _tokenService.GenerateRefreshToken();
                doktor.RefreshToken = refreshToken;
                doktor.RefreshTokenEndDate = refreshTokenEndDate;
                _context.SaveChanges();

                return Ok(new 
                {
                    refreshToken = refreshToken,
                    refreshTokenEndDate = refreshTokenEndDate
                });
            }
            else if (dto.Role == "Hasta")
            {
                var hasta = _context.Hasta.FirstOrDefault(x => x.Eposta == dto.Eposta && x.Password == dto.Password);
                if (hasta == null)
                    return Unauthorized(new { mesaj = "Geçersiz E-posta veya Şifre" });

                var (refreshToken, refreshTokenEndDate) = _tokenService.GenerateRefreshToken();
                hasta.RefreshToken = refreshToken;
                hasta.RefreshTokenEndDate = refreshTokenEndDate;
                _context.SaveChanges();

                return Ok(new 
                {
                    refreshToken = refreshToken,
                    refreshTokenEndDate = refreshTokenEndDate
                });
            }
            else
            {
                return BadRequest(new { mesaj = "Geçersiz Rol" });
            }
        }
    }
}