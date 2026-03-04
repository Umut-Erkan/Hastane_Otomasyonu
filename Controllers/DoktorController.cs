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

        public DoktorController(HastaneContext context , TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        
        [HttpPost ("key-al")]
        // Dışarıdan gelen DTO'yu, veritabanına eklenecek Entity'e çeviren metot
        // Dışardan hep DTO tipinde gelir veri
        public IActionResult DoktorKeyAl([FromBody] DoktorDTO dto)
        {
            try
            {
                var NewEntity = new Doktor // DTO -> Entity
                { 
                    //Tc = dto.Tc,
                    Eposta = dto.Eposta,                        
                    Role = "Doktor"
                };

                
                   
                //_context.Hasta.Add(NewEntity);
                //_context.SaveChanges();
                
                var token = _tokenService.GenerateToken(NewEntity);
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
        [HttpGet ("bilgi")]
        public IActionResult RandevuGöster([FromBody] DoktorDTO doktordto)
        {
            var Hastamız = _context.Hasta.FirstOrDefault(h => h.Tc == doktordto.Tc);
            if (Hastamız == null)
            {
                return StatusCode(500,"Kayıtlı hasta bulunamadı");
            }
            
            else if(Hastamız.RandevuId == null)
            {
                return StatusCode(200, Hastamız.Eposta);
            };
            
            return new ObjectResult ($"Hastanın randevularını ID'leri: {Hastamız.RandevuId}"){StatusCode = 200};
        }
    }
}
