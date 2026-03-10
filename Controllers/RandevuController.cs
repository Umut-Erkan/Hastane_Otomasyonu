using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hastane_Otomasyonu.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Authorize (Roles = "Hasta")]
        [HttpPost]
        public IActionResult RandevuAl([FromBody]  RandevuAddDTO AddDTO)
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
                    HastaŞikayet = AddDTO.Şikayet

                };

                _context.OnlineRandevus.Add(Randevu);
                _context.SaveChanges();

                ExistingDoktor.RandevuId.Add(Randevu.Id);
                ExistingHasta.RandevuId.Add(Randevu.Id);

                

                _context.SaveChanges();

                return StatusCode(200 , new { mesaj = "Başarılı"});
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
                string Hata = ex.Message;
                
                if (ex.InnerException != null)
                {
                    Hata += " | Detay: " + ex.InnerException.Message;
                }

                return StatusCode(500, Hata);
            }
        }
        




        // İSTENEN RANDEVU SİLME
        //[Authorize]
       /* [HttpDelete]
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
                Hastum IdSilincekHasta = _context.Hasta.FirstOrDefault(r => r.RandevuId.Concat(DelDTO.RandevuID));
                
                Doktor IdSilincekDoktor = _context.Doktors.FirstOrDefault(r => r.RandevuId.Concat(DelDTO.RandevuID));
                
                if (IdSilincekDoktor == null && IdSilincekHasta == null)
                    {
                        return StatusCode(500,"Doktor ve Hasta NULL");
                    }

                else if (IdSilincekHasta == null)
                    {
                        return StatusCode(500,"hasta NULL");
                    }

                else if (IdSilincekDoktor == null)
                    {
                        return StatusCode(500,"Doktor NULL");
                    }


                IdSilincekDoktor.RandevuId.Remove(DelDTO.RandevuID);
                IdSilincekHasta.RandevuId.Remove(DelDTO.RandevuID);
                _context.OnlineRandevus.Remove(DelKayıt);


                _context.SaveChanges();
                return StatusCode(200, "randevu başarıyla silindi");
            }

            catch (NullReferenceException ex)
            {
                return StatusCode(500, new{mesaj = "Hata", hata = ex.Message});
            }
        }*/

    }
}
