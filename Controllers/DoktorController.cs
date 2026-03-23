using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hastane_Otomasyonu.Business;
using Hastane_Otomasyonu.DTO;
using Hastane_Otomasyonu.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoktorController : ControllerBase
    {

        private readonly HastaneContext _context;
        private readonly TokenService _tokenService;
        private PasswordHashing _Hash;
        public DoktorController(HastaneContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;

            _Hash = new PasswordHashing();
        }


        [ServiceFilter(typeof(RefreshTokenFilter))] // Refresh token kontrolü ile hangi doktor olduğunu anlıyoruz.
        [HttpPost("Tedavi yaz")]
        public IActionResult TedaviYaz([FromBody] TedaviYazDTO tedaviDTO, [FromHeader(Name = "Authorization")] string token, HastaneContext context)
        {
            try
            {
                string accessToken = token.ToString().Replace("Bearer ", "");
                Doktor doktor = _context.Doktors.FirstOrDefault(h => h.AccessToken == accessToken); // işlemi yapan doktor
                Hastum hasta = _context.Hasta.FirstOrDefault(h => h.Id == tedaviDTO.HastaID); // İşlem yapılan hasta


                var Randevuları = _context.OnlineRandevus.Where(h => h.DoktorId == doktor.Id).ToList(); // doktorun randevuları ama List tipinde

                List<int> HastaIDleri = new List<int>();

                foreach (var randevu in Randevuları)
                {
                    HastaIDleri.Add(randevu.HastaId); // Doktorun randevularındaki hasta ID'lerini tek bir yere attık
                }

                if (!HastaIDleri.Contains(tedaviDTO.HastaID)) // Doktor doğru hasta Idsini yazdı mı diye kontrol ediyoruz.
                {
                    return BadRequest(new { mesaj = "Bu hastanın sizden randevusu yok." });
                }

                //Doktor tedavi sınıfından yeni entity oluşturacak
                Tedavi tedavi = new Tedavi
                {
                    Tanı = tedaviDTO.Tanı,
                    Tedavi1 = tedaviDTO.Tedavi,
                    Recete = tedaviDTO.Recete,
                    DoktorId = doktor.Id
                };

                _context.Tedavis.Add(tedavi);
                _context.SaveChanges();

                //Bu yeni entity’in ID’sini (TedaviID) seçtiği hastanınn TedaviID sütununa ekliycek

                hasta.TedaviId = tedavi.TedaviId;
                _context.SaveChanges();




                return Ok(new { mesaj = $"Hasta: {hasta.İsim} {hasta.Soyisim} tedavisi yazıldı." });
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


        [HttpPost("key-al")]
        public IActionResult DoktorKeyAl([FromBody] DoktorDTO dto)
        {
            try
            {
                Doktor Key = _context.Doktors.FirstOrDefault(r => r.Eposta == dto.Eposta);

                var token = _tokenService.GenerateAccessToken(Key);
                Console.Write("Token oluşturuldu");

                return Ok(token);

            }

            catch (DbUpdateException ex) // Veri tabanı hatası
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





        [Authorize(Roles = "Doktor")]
        [HttpPost("DoktorRandevu")]
        public IActionResult RandevuGöster([FromBody] DoktorDTO doktordto)
        {
            var Doktorumuz = _context.Doktors.FirstOrDefault(h => h.İsim == doktordto.Name);
            if (Doktorumuz == null)
            {
                return StatusCode(500, "Kayıtlı hasta bulunamadı");
            }


            return new ObjectResult("Doktorun randevularını ID'leri") { StatusCode = 200 };
        }

    }


}
