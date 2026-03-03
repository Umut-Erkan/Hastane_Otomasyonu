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

namespace Hastane_Otomasyonu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HastaController : ControllerBase
    {
        private readonly HastaneContext _context;

        public HastaController(HastaneContext context)
        {
            _context = context;
            
        }


        [HttpPost]
        // Dışarıdan gelen DTO'yu, veritabanına eklenecek Entity'e çeviren metot
        // Dışardan hep DTO tipinde gelir veri
        public IActionResult CreateHasta([FromBody] HastaDTO dto)
        {
            try
            {
                var NewEntity = new Hastum // DTO -> Entity
                {
                    Tc = dto.Tc,
                    İsim = dto.Name, // sağ taraf kullanıcıdan gelen DTO tipindeki veri
                    Soyisim = dto.Surname,
                    Password = dto.Password, // sol taraftaki veritabanına ekliyceğimiz Hastum'un sahip olduğu 
                    Eposta = dto.Eposta                        // veriye dönüşür.
                };
                bool TcKontrol = _context.Hasta.Any(h=> h.Tc == NewEntity.Tc);
                
                
                if (TcKontrol == true)
                {
                    return StatusCode(500, new
                    {
                        Baslik = "var olan entity",
                        mesaj = "Var olan hasta eklenmeye çalışıldı"
                    });
                }

                else
                {   
                _context.Hasta.Add(NewEntity);
                _context.SaveChanges();

                return Ok("Kayıt başarılı");
                }
            }

            catch(DbUpdateException ex) // Veri tabanı hatası
            {
                 return BadRequest(new { mesaj = "Hata.",hata = ex.StackTrace });
            }

            catch (Exception)
            {
                // Veritabanı dışındaki diğer genel sistem hataları için
                return StatusCode(500, new 
                { 
                    Baslik = "Sunucu Hatası", 
                    Mesaj = "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin." 
                });
            }
        
        }



        [HttpGet]
        public IActionResult RandevuGöster([FromBody] HastaDTO hastadto)
        {
            var Hastamız = _context.Hasta.FirstOrDefault(h => h.Tc == hastadto.Tc);
            if (Hastamız == null)
            {
                return StatusCode(500,"Kayıtlı hasta bulunamadı");
            }
            
            else if(Hastamız.RandevuId == null)
            {
                return StatusCode(200, "Hastanın randevusu yok");
            };
            
            return new ObjectResult ($"Hastanın randevularını ID'leri: {Hastamız.RandevuId}"){StatusCode = 200};
        }
    }
}