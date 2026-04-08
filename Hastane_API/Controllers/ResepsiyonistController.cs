using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Hastane_Otomasyonu.Business;
using Hastane_Otomasyonu.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResepsiyonistController : ControllerBase
    {
        private readonly HastaneContext _context;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _config;
        private PasswordHashing _Hash;

        public ResepsiyonistController(HastaneContext context, TokenService tokenService, IConfiguration config)
        {
            _context = context;
            _tokenService = tokenService;
            _config = config;
            _Hash = new PasswordHashing();
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            try
            {
                var resepsiyonist = _context.HospitalReceptionists.FirstOrDefault(h => h.Tc == dto.Tc && h.Password == _Hash.HashPassword(dto.Password));
                var AccessToken = new JwtSecurityTokenHandler().ReadJwtToken(resepsiyonist.AccessToken);
                var UzmanlıkAlanı = resepsiyonist.Alan;

                if (resepsiyonist == null)
                {
                    bool isPasswordValid = _Hash.VerifyPassword(dto.Password, resepsiyonist.Password);
                    if (!isPasswordValid)
                    {
                        return BadRequest(new { mesaj = "Şifre hatalı." });
                    }
                    else
                    {
                        return BadRequest(new { mesaj = "TC hatalı." });
                    }
                }


                if (AccessToken.ValidTo >= DateTime.UtcNow)
                {
                    resepsiyonist.AccessToken = _tokenService.GenerateAccessToken(resepsiyonist);
                    _context.SaveChanges();
                }

                _context.SaveChanges();


                return Ok(new
                {
                    mesaj = "Giriş başarılı.",
                    AccessToken = resepsiyonist.AccessToken,
                    Alan = UzmanlıkAlanı,
                    StatusCode = 200
                });
            }


            catch (Exception ex)
            {
                return BadRequest(new { mesaj = "Login sırasında bir hata oluştu.", hata = ex.Message });
            }
        }
    }
}
