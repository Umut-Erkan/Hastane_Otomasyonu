using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hastane_Otomasyonu.DTO;
using Microsoft.VisualBasic;
using MyApiProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Hastane_Otomasyonu.Business;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Hastane_Otomasyonu.Filters;

namespace Hastane_Otomasyonu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HastaController : ControllerBase
    {
        private readonly HastaneContext _context;
        private readonly TokenService _tokenService;
        private readonly ILogger<HastaController> _logger;
        private PasswordHashing _Hash;

        
        public HastaController(HastaneContext context , TokenService tokenService , ILogger<HastaController> logger , PasswordHashing hash)
        {
            _context = context;
            
            _tokenService = tokenService;
            
            _logger = logger;
            
            _Hash = hash;
        }


        [HttpPost ("Login")] // Hasta ilk kez kayıt oluyor
        public IActionResult CreateHasta([FromBody] HastaAddDTO dto) 
        {
            try
            {
                var NewEntity = new Hastum // DTO -> Entity
                {
                    Tc = dto.Tc,
                    İsim = dto.Name, 
                    Soyisim = dto.Surname,
                    Password = _Hash.HashPassword(dto.Password) , // sol taraftaki veritabanına ekliyceğimiz Hastum'un sahip olduğu 
                    Eposta = dto.Eposta,                        // veriye dönüşür.
                    Role = "Hasta",
                    AccessToken = "PlaceHolder",
                    RefreshToken = "PlaceHolder",
                    RefreshTokenEndDate = DateTime.Now
                };               

                bool TcKontrol = _context.Hasta.Any(h=> h.Tc == NewEntity.Tc);
                Console.Write("Hasta oluşturuldu ve TC si kontrol edildi" , TcKontrol);
               
                if (TcKontrol == true)
                {
                    return StatusCode(500, new
                    {
                        Baslik = "var olan entity",
                        mesaj = "Var olan hasta eklenmeye çalışıldı"
                    });
                }

                else if (dto.Tc.ToString().Length != 11)
                {
                    return new ObjectResult("Tc'nin 11 haneli olması lazım"){StatusCode = 400};
                }

                else
                {   
                
                _context.Hasta.Add(NewEntity);
                _context.SaveChanges();

                var AccessToken = _tokenService.GenerateAccessToken(NewEntity);
                NewEntity.AccessToken = AccessToken;
                
                // ValidTo her zaman UTC döndürür. Türkiye saatinde göstermek için +3 ekliyoruz
                var tarih = new JwtSecurityTokenHandler().ReadJwtToken(AccessToken).ValidTo.AddHours(3).ToString("dd.MM.yyyy HH:mm:ss");

                var RefreshToken = _tokenService.GenerateRefreshToken();
                NewEntity.RefreshToken = RefreshToken.Token;
                NewEntity.RefreshTokenEndDate = RefreshToken.Expiration;

                _context.SaveChanges();
                
                

                return Ok($"{tarih} kullanıcının Acces token bitiş tarihidir, Hastanın özelliklerinden erişebilirsiniz");
                }
            }

            catch (DbUpdateException ex) 
            {
                
                return BadRequest(new 
                { 
                    mesaj = "Veritabanına kaydederken hata oluştu.",
                    hata = ex.Message,
                    detay = ex.InnerException?.Message 
                });
            }

            catch (NullReferenceException ex) 
            {
                return BadRequest(new { mesaj = "Beklenmedik bir veri boşluğu oluştu.", hata = ex.Message });
            }

           catch (Exception ex)
           
            {
            // InnerException'ı değil, onun mesajını alıyoruz
            return BadRequest(new { 
                mesaj = "Bir hata oluştu.", 
                hataDetayi = ex.Message,
                ekBilgi = ex.InnerException?.Message
            });
            }
        }


        [HttpDelete("Logout")]
        [Authorize(Roles = "Hasta")]
        public IActionResult Logout(int id)
        {
            
            var token = new JwtSecurityTokenHandler().ReadJwtToken(context.HttpContext.Request.Headers["Authorization"])
            var NameIdentifier = token.Claims.FirstOrDefault(c => c.Type == "NameIdentifier").Value;
            var Hasta = _context.Hasta.FirstOrDefault(h => h.Id == NameIdentifier);
            
            if (Hasta == null)
            {
                return StatusCode(404, "Hasta bulunamadı");
            }
            _context.Remove(Hasta);
            _context.SaveChanges();
            return Ok("Logout başarılı");
        }

           
        [ServiceFilter(typeof(RefreshTokenFilter))]
        [Authorize(Roles = "Hasta")] // Çalışıyor // Token sahibinin id si dönüyor
        [HttpGet ("HastaSorgula")]
        public IActionResult RandevuGöster()
        
        {
            var jti = User.Claims.FirstOrDefault(c => c.Type == "jti").Value;
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            return StatusCode(200,$"çalışıyor {jti}, {userId}");
        }
    }}