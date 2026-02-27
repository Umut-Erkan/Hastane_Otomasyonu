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
        public IActionResult RandevuAl([FromBody] RandevuDTO Randevudto)
        // Kullanıcı bilgileri yine dışardan gelcek. DTO-> Entity (ama eklemek için değil, kullanmak için)
        // Randevu bilgileri Entity. Okuncak ise Entity-> DTO lazım.
        {
            try
            {
                // DTO'dan gelen Tc ile veritabanımdaki istenen hastaya eriştim
                Hastum ExistingHasta = _context.Hasta.FirstOrDefault(h=> h.Tc == Randevudto.Tc);
                
                //Şimdi Hastamın şikayeti, Girilen doktor ismi ile randevu oluşturcam.
                var Randevu = new OnlineRandevu
                {
                    HastaName = ExistingHasta.İsim,
                    HastaSurname = ExistingHasta.Soyisim,

                    DoktorName = Randevudto.DoktorName,
                    DoktorSurname = Randevudto.DoktorSurname,

                    Saat = Randevudto.Saat,
                    Tarih = Randevudto.Tarih,
                    HastaŞikayet = ExistingHasta.Şikayet,

                    IdNavigation = ExistingHasta
                };
                
                _context.OnlineRandevus.Add(Randevu);
                _context.SaveChanges();

                return StatusCode(200 , new { mesaj = "Başarılı"});
            }
            
            catch (Exception ex)
            {
                // Hata mesajını düz bir metin (string) olarak alıyoruz
                string Hata = ex.Message;
                
                // Veritabanı ile ilgili alt detaylar varsa onu da metne ekliyoruz
                if (ex.InnerException != null)
                {
                    Hata += " | Detay: " + ex.InnerException.Message;
                }

                return StatusCode(500, Hata);
            }
        }
        














    // RANDEVU DOKTORA ATANCAK

    //Doktor ExistingDoktor = _context.Doktors.FirstOrDefault(d => d.İsim == Randevudto.DoktorName && d.Soyisim == Randevudto.DoktorSurname);


    //ExistingDoktor.OnlineRandevus.Add(Randevu);



    }
}
