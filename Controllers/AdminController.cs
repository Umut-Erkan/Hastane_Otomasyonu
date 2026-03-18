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
using Hastane_Otomasyonu.Filters;


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

        }


        
        [ServiceFilter(typeof(ActionFilter))]
        [HttpPost ("Create Doktor")]
        public IActionResult DoktorOluştur ([FromBody] DoktorDTO doktordto)
        {
            try
            {
                var NewEntity = new Doktor
                {
                    Tc = doktordto.Tc,
                    İsim = doktordto.Name, 
                    Soyisim = doktordto.Surname,
                    Password = _Hash.HashPassword(doktordto.Password).ToString() ,  
                    Eposta = doktordto.Eposta,   
                    Alan = doktordto.Alan,  

                    Role = "Doktor",
                    AccessToken = "PlaceHolder",
                    RefreshToken = "PlaceHolder",
                    RefreshTokenEndDate = DateTime.Now
                };
                

                bool TcKontrol = _context.Doktors.Any(h=> h.Tc == NewEntity.Tc);

                if (TcKontrol)
                {
                    return StatusCode(400 , "Zaten bu doktor sistemde tanımlı");
                }

                _context.Doktors.Add(NewEntity);
                _context.SaveChanges();

                var token = _tokenService.GenerateAccessToken(NewEntity);
                NewEntity.AccessToken = token;

                var RefreshToken = _tokenService.GenerateRefreshToken();
                NewEntity.RefreshToken = RefreshToken.Token;
                NewEntity.RefreshTokenEndDate = RefreshToken.Expiration;

                
                _context.SaveChanges();

                return Ok($"{NewEntity.İsim}, {NewEntity.Soyisim} sisteme eklendi.");
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
            return BadRequest(new { 
                mesaj = "Bir hata oluştu.", 
                hataDetayi = ex.Message,
                ekBilgi = ex.InnerException?.Message
            });
            }
       }
    }
}