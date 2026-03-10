using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Hastane_Otomasyonu.Business;
using Hastane_Otomasyonu.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiProject.Models;
using Hastane_Otomasyonu.Admin;

namespace Hastane_Otomasyonu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {


        private readonly HastaneContext _context;
        private readonly TokenService _tokenService;
        private PasswordHashing _Hash;
        
        public AdminController(HastaneContext context , TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;

            _Hash = new PasswordHashing();

            AdminS admin = new AdminS(); 
        }



        client.DefauItRequestHeaders.Authorizat 
        deneme = new AuthenticationHeaderValue(Admin)


        [Authorize (Roles = "admin")]
        [HttpPost ("DoktorOluştur")]

        public IActionResult DoktorOluştur ([FromBody] DoktorDTO doktordto)
        {
            try
            {
                var NewEntity = new Doktor
                {
                    Tc = doktordto.Tc,
                    İsim = doktordto.Name, 
                    Soyisim = doktordto.Surname,
                    Password = _Hash.HashPassword(doktordto.Password) ,  
                    Eposta = doktordto.Eposta,                        
                    Role = "Doktor",
                    Token = "PlaceHolder"
                };

                bool TcKontrol = _context.Doktors.Any(h=> h.Tc == NewEntity.Tc);
                if (TcKontrol)
                {
                    return StatusCode(400 , "Zaten bu doktor sistemde tanımlı");
                }

                var token = _tokenService.GenerateToken(NewEntity);
                NewEntity.Token = token;

                _context.Doktors.Add(NewEntity);
                _context.SaveChanges();

                return Ok("Doktor oluşturuldu.");
            }



            catch (DbUpdateException ex) 
            {
                // InnerException null olabilir, o yüzden ?. kullanıyoruz
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
    }
}