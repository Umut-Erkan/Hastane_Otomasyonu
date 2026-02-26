using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MyApiProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hastane_Otomasyonu.DTO;
using Microsoft.VisualBasic;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.Controllers
{
    [Route("api/[controller]")]
    public class CreateHastaController : ControllerBase
    {
        private readonly HastaneContext _context;

        public CreateHastaController(HastaneContext context)
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
                var yeniEntity = new Hastum // DTO -> Entity
                {
                    Tc = dto.Tc,
                    İsim = dto.Name, // sağ taraf kullanıcıdan gelen DTO tipindeki veri
                    Soyisim = dto.Surname, // sol taraftaki veritabanına ekliyceğimiz Hastum'un sahip olduğu 
                    Şikayet = dto.Şikayet,// veriye dönüşür.
                    //OnlineRandevu = dto.Randevu
                };

                _context.Hasta.Add(yeniEntity);
                _context.SaveChanges();

                return Ok("Kayıt başarılı");

            }
            catch(Exception ex)
            {
                 return BadRequest(new { mesaj = "Hata.",hata = ex.StackTrace });
            }
        
        }
    }
}