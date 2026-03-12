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

namespace Hastane_Otomasyonu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoktorController : ControllerBase
    {

        private readonly HastaneContext _context;
        private readonly TokenService _tokenService;
        private PasswordHashing _Hash;
        public DoktorController(HastaneContext context , TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;

            _Hash = new PasswordHashing();
        }
        



        [HttpPost ("key-al")]
        public IActionResult DoktorKeyAl([FromBody] DoktorDTO dto)
        {
            try
            {
                Doktor Key = _context.Doktors.FirstOrDefault(r => r.Eposta == dto.Eposta);

                var token = _tokenService.GenerateToken(Key);
                Console.Write("Token oluşturuldu");

                return Ok(token);
                
            }

            catch(DbUpdateException ex) // Veri tabanı hatası
            {
                 return BadRequest(new 
    { 
                mesaj = "Veritabanına kaydederken hata oluştu.",
                hata = ex.Message,
                detay = ex.InnerException?.Message 
            });
            }

            catch(NullReferenceException ex) 
            {
                return BadRequest(new { mesaj = "Hata.", hata = ex }); // <-- Hata burada!
            }

            catch (Exception ex)
                {
                return StatusCode(500, new 
                        { 
                            Baslik = "Sunucu Hatası", 
                            Mesaj = $"Bilinmeyen bir hata: {ex.Message}",
                            Detay = ex.InnerException?.Message,
                            Stack = ex.StackTrace 
                        }
                );
            }
            }

            
        


        [Authorize (Roles = "Doktor")]
        [HttpPost ("DoktorRandevu")]
        public IActionResult RandevuGöster([FromBody] DoktorDTO doktordto)
        {
            var Doktorumuz = _context.Doktors.FirstOrDefault(h => h.İsim == doktordto.Name);
            if (Doktorumuz == null)
            {
                return StatusCode(500,"Kayıtlı hasta bulunamadı");
            }
            
            
            return new ObjectResult ("Doktorun randevularını ID'leri"){StatusCode = 200};
        }

        }

    
}
