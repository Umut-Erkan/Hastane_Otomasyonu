using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
        private readonly ILogger<DoktorController> _logger;

        public DoktorController(HastaneContext context, TokenService tokenService, ILogger<DoktorController> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;

            _Hash = new PasswordHashing();
        }

        [HttpGet("Mesai")]
        public IActionResult Mesai()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            Doktor doktor = _context.Doktors.FirstOrDefault(h => h.Id == int.Parse(userId));

            var bosZaman = _context.Zamen.Where(h => h.DoktorId == int.Parse(userId)).ToList();

            if (bosZaman.Count() == 0)
            {
                return StatusCode(404, "Mesai bulunamadı");
            }

            // bosZaman direkt her şeyi veriyor anlamadım !!!!!!!!!!!!!!!!!!! 
            var sonuc = bosZaman.Select(z => new
            {
                z.Id,
                z.Zaman1
            });

            return StatusCode(200, sonuc);
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
                    DoktorId = doktor.Id,
                    HastaId = hasta.Id
                };

                _context.Tedavis.Add(tedavi);
                _context.SaveChanges();

                //Bu yeni entity’in ID’sini (TedaviID) seçtiği hastanınn TedaviID sütununa ekliycek


                // Tedavi yazılında mevcut randevuyu sil

                var silinecekRandevular = _context.OnlineRandevus
                    .FirstOrDefault(r => r.DoktorId == doktor.Id && r.HastaId == hasta.Id);

                if (silinecekRandevular != null)
                {
                    _context.OnlineRandevus.Remove(silinecekRandevular);
                    _context.SaveChanges();
                }

                return Ok(new { mesaj = $"Hasta: {hasta.İsim} {hasta.Soyisim} tedavisi {doktor.İsim} {doktor.Soyisim} tarafından yazıldı, Randevusu sistemden silindi." });
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




        [ServiceFilter(typeof(RefreshTokenFilter))]
        [Authorize(Roles = "Doktor")]
        [HttpGet("RandevuGoster")]
        public IActionResult RandevuGöster()
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;  // Doktorun id'si
            Doktor user = _context.Doktors.FirstOrDefault(c => c.Id == int.Parse(userId)); // Randevularını görüntüleyeceğimiz doktor

            if (user == null)
            {
                return StatusCode(404, "Doktor bulunamadı");
            }

            var randevular = _context.OnlineRandevus.Where(c => c.DoktorId == int.Parse(userId))
                .Select(r => new
                {
                    r.HastaŞikayet,
                    r.HastaName,
                    r.HastaSurname,
                    r.Tarih,
                    r.Saat
                }).ToList();

            if (randevular.Count() == 0)
            {
                return StatusCode(404, "Randevu bulunamadı");
            }
            return StatusCode(200, randevular);
        }

    }


}
