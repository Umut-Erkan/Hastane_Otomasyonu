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
                // 1. Önce sadece TC ile kullanıcıyı ara
                var resepsiyonist = _context.HospitalReceptionists.FirstOrDefault(h => h.Tc == dto.Tc);

                // 2. Kullanıcı hiç yoksa hemen çık (NullReferenceException engellenir)
                if (resepsiyonist == null)
                {
                    return BadRequest(new { mesaj = "Kullanıcı bulunamadı." });
                }

                // 3. Kullanıcı varsa şifreyi doğrula (Hash karşılaştırması için VerifyPassword şarttır)
                bool isPasswordValid = _Hash.VerifyPassword(dto.Password, resepsiyonist.Password);
                if (!isPasswordValid)
                {
                    return BadRequest(new { mesaj = "Şifre hatalı." });
                }

                // 4. Token kontrolü ve üretimi
                if (string.IsNullOrEmpty(resepsiyonist.AccessToken) || resepsiyonist.AccessToken == "PlaceHolder")
                {
                    resepsiyonist.AccessToken = _tokenService.GenerateAccessToken(resepsiyonist);
                }
                else
                {
                    try
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jwtToken = handler.ReadJwtToken(resepsiyonist.AccessToken);


                        if (jwtToken.ValidTo <= DateTime.UtcNow)
                        {
                            resepsiyonist.AccessToken = _tokenService.GenerateAccessToken(resepsiyonist);
                        }
                    }
                    catch
                    {
                        resepsiyonist.AccessToken = _tokenService.GenerateAccessToken(resepsiyonist);
                    }
                }

                _context.SaveChanges();

                return Ok(new
                {
                    mesaj = "Giriş başarılı.",
                    AccessToken = resepsiyonist.AccessToken,
                    Alan = resepsiyonist.Alan,
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
