using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
        [ServiceFilter(typeof(RefreshTokenFilter))]
        [Authorize (Roles = "Hasta")]
        [HttpPost ("Randevu Al")]
        public IActionResult RandevuAl([FromBody]  RandevuAddDTO AddDTO)
        {
            Doktor ExistingDoktor = DoktoruBul(AddDTO.DoktorName, AddDTO.DoktorSurname); 

            try
            {
                // DTO'dan gelen Tc ile veritabanımdaki istenen hastaya eriştim
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                var rawToken = authHeader.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();
                var token = new JwtSecurityTokenHandler().ReadJwtToken(rawToken);
                string HastaId = token.Claims.FirstOrDefault(c=> c.Type == ClaimTypes.NameIdentifier).Value;
                Hastum ExistingHasta = _context.Hasta.FirstOrDefault(h=> h.Id.ToString() == HastaId);

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
        




        
        // ISTENEN RANDEVU SILME
        [ServiceFilter(typeof(RefreshTokenFilter))]
        [Authorize (Roles = "Hasta")]
        [HttpDelete]
        
        public IActionResult KayitSil([FromBody] RandevuDelDTO DelDTO)
        {
            OnlineRandevu DelKayıt = _context.OnlineRandevus.FirstOrDefault(r => r.Id == DelDTO.RandevuID);
            try
            {
                if (DelKayıt == null)
                {
                    return StatusCode(404, "Silinecek randevu bulunamadı");
                }

                _context.SaveChanges();
                return StatusCode(200, $"{DelDTO.RandevuID} nolu randevu başarıyla silindi");
            }

            catch (NullReferenceException ex)
            {
                return StatusCode(500, new{mesaj = "Hata", hata = ex.Message});
            }
        }

        }
    }

