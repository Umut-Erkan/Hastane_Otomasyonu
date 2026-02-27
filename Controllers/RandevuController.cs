using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hastane_Otomasyonu.DTO;
using Microsoft.AspNetCore.Mvc;
using MyApiProject.Data;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RandevuController : ControllerBase
    {
        private readonly HastaneContext _context;

        public RandevuController(HastaneContext context)
        {
            _context = context;
        }
        
        // RANDEVU ALMA
        [HttpPost]
        public IActionResult RandevuAl([FromBody] HastaDTO dto , RandevuDTO Randevudto)
        // Kullanıcı bilgileri yine dışardan gelcek. DTO-> Entity (ama eklemek için değil, kullanmak için)
        // Randevu bilgileri Entity. Okuncak ise Entity-> DTO lazım.
       
        {   // DTO'dan gelen Tc ile veritabanımdaki istenen hastaya eriştim
            Hastum existingEntity = _context.Hasta.FirstOrDefault(h=> h.Tc == dto.Tc);
            
            //Şimdi Hastamın şikayeti, Girilen doktor ismi ile randevu oluşturcam.
            var Randevu = new OnlineRandevu
            {
                DoktorId = Randevudto.DoktorId,
                Saat = Randevudto.Saat,
                Tarih = Randevudto.Tarih,
                HastaId = existingEntity.Id
            };
            
            _context.OnlineRandevus.Add(Randevu);
            _context.SaveChanges();

            return StatusCode(200 , new { mesaj = "Başarılı"});
        }
        














    // RANDEVU DOKTORA ATANCAK






    }
}
