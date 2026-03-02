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
        public IActionResult RandevuAl([FromBody]  RandevuAddDTO AddDTO)
        // Kullanıcı bilgileri yine dışardan gelcek. DTO-> Entity (ama eklemek için değil, kullanmak için)
        // Randevu bilgileri Entity. Okuncak ise Entity-> DTO lazım (Get).
        {
            Doktor ExistingDoktor = DoktoruBul(AddDTO.DoktorName, AddDTO.DoktorSurname); 
            try
            {
                // DTO'dan gelen Tc ile veritabanımdaki istenen hastaya eriştim
                Hastum ExistingHasta = _context.Hasta.FirstOrDefault(h=> h.Tc == AddDTO.Tc);

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
                    DoktorName = AddDTO.DoktorName,
                    DoktorSurname = AddDTO.DoktorSurname,
                    DoktorId = ExistingDoktor.Id,
                    Saat = TimeOnly.FromDateTime(DateTime.Now),
                    Tarih = DateOnly.FromDateTime(DateTime.Now),
                    HastaŞikayet = ExistingHasta.Şikayet,

                };

                _context.OnlineRandevus.Add(Randevu);
                _context.SaveChanges();

                ExistingDoktor.RandevuId += "," + Randevu.Id.ToString();
                ExistingHasta.RandevuId += "," + Randevu.Id.ToString();

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
        

        // İSTENEN KAYIT SİLİNECEK
        [HttpDelete]
        public IActionResult KayitSil([FromBody] RandevuDelDTO DelDTO)
        {
            OnlineRandevu DelKayıt = _context.OnlineRandevus.FirstOrDefault(r => r.Id == DelDTO.RandevuID);

            if (DelKayıt == null)
            {
                return StatusCode(404, "Silinecek randevu bulunamadı");
            }
            try
            {
            // DOKTORDAN VE HASTADAN SİLİNEN RANDEVUNUN ID'SİNİ SİL
                Hastum IdSilincekHasta = _context.Hasta.FirstOrDefault(r => r.Id == DelDTO.RandevuID);
                Doktor IdSilincekDoktor = _context.Doktors.FirstOrDefault(r => r.Id == DelDTO.RandevuID);
                
                if (IdSilincekDoktor == null && IdSilincekDoktor == null)
                    {
                        return StatusCode(500,"Doktor ya da Hastadan Id silinemedi");
                    }

                IdSilincekDoktor.RandevuId.Remove(DelDTO.RandevuID);
                IdSilincekHasta.RandevuId.Remove(DelDTO.RandevuID);
                _context.OnlineRandevus.Remove(DelKayıt);


                return StatusCode(200, "randevu başarıyla silindi");


                _context.SaveChanges();
            }

            catch (NullReferenceException ex)
            {
                return StatusCode(500, new{mesaj = "Hata", hata = ex.Message});
            }
        }

    }
}
