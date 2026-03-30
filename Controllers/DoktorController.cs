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

        [HttpGet("Mesai/{userId}")]
        public IActionResult Mesai(int userId)
        {
            var Appointment = new List<AppointmentSlot>();



            return StatusCode(200, Appointment);
        }


        [ServiceFilter(typeof(RefreshTokenFilter))] // Refresh token kontrolü ile hangi doktor olduğunu anlıyoruz.
        [HttpPost("Tedavi yaz")]
        public IActionResult TedaviYaz([FromBody] TedaviRequestDTO request, [FromHeader(Name = "Authorization")] string token)
        {
            var receteDTO = request.ReceteDTO;
            var tedaviDTO = request.TedaviDTO;

            try
            {
                string accessToken = token.ToString().Replace("Bearer ", "");
                Doktor doktor = _context.Doktors.FirstOrDefault(h => h.AccessToken == accessToken); // işlemi yapan doktor
                Hastum hasta = _context.Hasta.FirstOrDefault(h => h.Id == request.HastaID); // İşlem yapılan hasta


                var Randevuları = _context.OnlineRandevus.Where(h => h.DoktorId == doktor.Id).ToList(); // doktorun randevuları ama List tipinde
                _logger.LogInformation($"Doktorun randevuları: {Randevuları.Count()}");
                List<int> HastaIDleri = new List<int>();
                //HastaIDleri = null;

                foreach (var randevu in Randevuları)
                {
                    HastaIDleri.Add(randevu.HastaId); // Doktorun randevularındaki hasta ID'lerini tek bir yere attık
                    _logger.LogInformation($"Hasta ID: {randevu.HastaId}");
                    _logger.LogInformation($"Randevu adeti: {HastaIDleri.Count()}");
                }

                if (!HastaIDleri.Contains(request.HastaID)) // Doktor doğru hasta Idsini yazdı mı diye kontrol ediyoruz.
                {
                    return BadRequest(new { mesaj = "Bu hastanın sizden randevusu yok." });
                }


                // TEDAVİ YAZILACAK
                Tedavi tedavi = new Tedavi
                {
                    Tanı = tedaviDTO.Tanı,
                    DoktorId = doktor.Id,
                    HastaId = hasta.Id
                };


                _context.Tedavis.Add(tedavi);
                _context.SaveChanges();

                //RECETE YAZACAK TEDAVİID = RECETEID

                Recete recete = new Recete
                {
                    Kullanım = receteDTO.Kullanım,
                    GecerlilikTarihi = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                };

                recete.ReceteId = tedavi.TedaviId;
                _context.Recetes.Add(recete);
                _context.SaveChanges();


                if (receteDTO.Ilaclar.Count != receteDTO.IlacAdet.Count)
                {
                    return BadRequest(new { mesaj = "İlaç sayısı ile adet sayısı eşleşmiyor." });
                }


                foreach (var (ilac, adet) in receteDTO.Ilaclar.Zip(receteDTO.IlacAdet)) // DTO'daki Ilaclar ile adetleri karşılıklı eşleniyor.
                {
                    var ilacID = _context.Ilacs.FirstOrDefault(h => h.IlacName == ilac.ToString());

                    if (ilacID != null)
                    {
                        _context.IlcaToRecetes.Add(new IlcaToRecete
                        {
                            Adet = adet,
                            ReceteFk = recete.ReceteId,
                            IlcaFk = ilacID.IlacId
                        });
                        _context.SaveChanges();
                    }
                    else
                    {
                        return BadRequest(new { mesaj = "İlaç bulunamadı" });
                    }
                }


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
