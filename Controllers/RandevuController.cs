using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hastane_Otomasyonu.DTO;
using Microsoft.AspNetCore.Mvc;
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
        

        private Doktor DoktoruBul(string isim, string soyisim)
        {
            return _context.Doktors.FirstOrDefault(d => 
            d.İsim == isim && 
            d.Soyisim == soyisim);
        }


        // RANDEVU ALMA
        [HttpPost]
        public IActionResult RandevuAl([FromBody]  RandevuAlDTO Randevudto)
        // Kullanıcı bilgileri yine dışardan gelcek. DTO-> Entity (ama eklemek için değil, kullanmak için)
        // Randevu bilgileri Entity. Okuncak ise Entity-> DTO lazım (Get).
        {
            Doktor ExistingDoktor = DoktoruBul(Randevudto.DoktorName, Randevudto.DoktorSurname); 
            try
            {
                // DTO'dan gelen Tc ile veritabanımdaki istenen hastaya eriştim
                Hastum ExistingHasta = _context.Hasta.FirstOrDefault(h=> h.Tc == Randevudto.Tc);

                if (ExistingDoktor == null)
                {
                    return StatusCode(404, new { mesaj = "Belirtilen isim ve soyisimde bir doktor bulunamadı." });
                }

                
                // Randevu alıncak doktor   // Randevu listesinde ıd kısmı Mevcut hasta ile şeleşen randevunun doktoru
                bool zatenRandevusuVarMi = _context.OnlineRandevus.Any(r => 
                    r.HastaId == ExistingHasta.Id && 
                    r.DoktorId == ExistingDoktor.Id);
                    
                if (zatenRandevusuVarMi)
                {
                    return StatusCode(500 , new { mesaj = "Zaten bu doktordan randevunuz var"});
                }


                OnlineRandevu Randevu = new OnlineRandevu
                {
                    HastaName = ExistingHasta.İsim,
                    HastaSurname = ExistingHasta.Soyisim,
                    HastaId = ExistingHasta.Id,
                    DoktorName = Randevudto.DoktorName,
                    DoktorSurname = Randevudto.DoktorSurname,
                    DoktorId = ExistingDoktor.Id,
                    Saat = TimeOnly.FromDateTime(DateTime.Now),
                    Tarih = DateOnly.FromDateTime(DateTime.Now),
                    HastaŞikayet = ExistingHasta.Şikayet,

                };

                ExistingDoktor.RandevuId += "," + Randevu.Id.ToString();
                ExistingHasta.RandevuId += "," + Randevu.Id.ToString();

                _context.OnlineRandevus.Add(Randevu);
                _context.SaveChanges();

                return StatusCode(200 , new { mesaj = "Başarılı"});
            }
            
            catch (Exception ex)
            {
                string Hata = ex.Message;
                
                if (ex.InnerException != null)
                {
                    Hata += " | Detay: " + ex.InnerException.Message;
                }

                return StatusCode(500, Hata);
            }
        }
        



    // RANDEVU DOKTORA ATANCAK
    /*
    public IActionResult DoktoraRandevuEkle([FromBody] RandevuDTO Randevudto)
        {
            var ExistingDoktor = DoktoruBul(Randevudto.DoktorName, Randevudto.DoktorSurname);
            
            if(_context.Doktors.Any(d =>d.İsim == ExistingDoktor.İsim)) //
            {
                _context.Doktors.
            }
            return default;
        }
    


    //ExistingDoktor.OnlineRandevus.Add(Randevu);

*/

    }
}
