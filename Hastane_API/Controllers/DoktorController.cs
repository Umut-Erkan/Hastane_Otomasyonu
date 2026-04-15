using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hastane_Otomasyonu.Business;
using Hastane_Otomasyonu.DTO;
using Hastane_Otomasyonu.Filters;
using Hastane_Otomasyonu.Redis.Interfaces;
using Hastane_Otomasyonu.Redis.Services;
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

        private List<DoktorDisplayDTO> SearchOnSQL()
        {
            var doktorlarDTO = _context.Doktors.Select(d => new DoktorDisplayDTO
            {
                Name = d.İsim,
                Surname = d.Soyisim,
                Eposta = d.Eposta,
                Alan = d.Alan,
                Id = d.Id
            }).ToList();

            if (doktorlarDTO.Count == 0)
            {
                Console.WriteLine("Doktor bulunamadı");
            }
            return doktorlarDTO;
        }

        private readonly IRedisCacheService _cache;
        private readonly HastaneContext _context;
        private readonly TokenService _tokenService;
        private PasswordHashing _Hash;
        private readonly ILogger<DoktorController> _logger;

        public DoktorController(IRedisCacheService cache, HastaneContext context, TokenService tokenService, ILogger<DoktorController> logger)
        {
            _cache = cache;
            _context = context;
            _tokenService = tokenService;
            _logger = logger;

            _Hash = new PasswordHashing();
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            try
            {
                var doktor = _context.Doktors.FirstOrDefault(h => h.Tc == dto.Tc);

                var AccessToken = new JwtSecurityTokenHandler().ReadJwtToken(doktor.AccessToken);
                DateTime AccessTokenEndDate = AccessToken.ValidTo;
                if (doktor == null)
                {
                    return BadRequest(new { mesaj = "TC veya şifre hatalı." });
                }

                bool isPasswordValid = _Hash.VerifyPassword(dto.Password, doktor.Password);
                if (!isPasswordValid)
                {
                    return BadRequest(new { mesaj = "Şifre hatalı." });
                }

                if (AccessTokenEndDate < DateTime.Now)
                {
                    doktor.AccessToken = _tokenService.GenerateAccessToken(doktor);
                    _context.SaveChanges();
                }
                if (doktor.RefreshTokenEndDate < DateTime.Now)
                {
                    doktor.RefreshToken = _tokenService.GenerateRefreshToken().Token;
                    doktor.RefreshTokenEndDate = _tokenService.GenerateRefreshToken().Expiration;
                    _context.SaveChanges();
                }

                return Ok(new
                {
                    mesaj = "Giriş başarılı.",
                    Id = doktor.Id,
                    AccessToken = doktor.AccessToken,
                    RefreshToken = doktor.RefreshToken,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mesaj = "Login sırasında bir hata oluştu.", hata = ex.Message });
            }
        }


        [ServiceFilter(typeof(RefreshTokenFilter))]
        [HttpGet("GetDoktorRedis")]
        public IActionResult GetDoktorRedis()
        {
            try
            {
                var doktorlar = _cache.GetValue();

                _logger.LogInformation($"Controller tarafı Doktorlar Tipi: {doktorlar.GetType()}");

                if (doktorlar == null)
                {
                    Console.WriteLine("Cache üzerinden doktor bulunamadı, SQL'e bakılıyor...");
                    var doktorlarSQL = SearchOnSQL();
                    return Ok(new { doktorlarSQL, mesaj = "Doktorlar SQL üzerinden getirildi", StatusCode = 200 });
                }

                return Ok(doktorlar);
            }

            catch (NullReferenceException nl)
            {
                return BadRequest(new { mesaj = "Doktor bulunamadı", hata = nl.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mesaj = "Doktor bulunamadı", hata = ex.Message });
            }
        }


        [ServiceFilter(typeof(RefreshTokenFilter))]
        [HttpGet("GetIlaclar")]
        public IActionResult GetIlaclar([FromHeader(Name = "Authorization")] string token)
        {
            string accessToken = token.ToString().Replace("Bearer ", "");
            Doktor doktor = _context.Doktors.FirstOrDefault(h => h.AccessToken == accessToken);

            string alan = doktor.Alan;
            var ilaclar = _context.Ilacs.Where(i => i.KullanımAlanı.Trim() == alan.Trim()).Select(i => new
            {
                i.IlacId,
                IlacName = i.IlacName.Trim(),
                KullanımAlanı = i.KullanımAlanı.Trim()
            }).ToList();

            if (ilaclar.Count == 0)
            {
                return BadRequest(new { mesaj = "Kayıtlı ilaç bulunamadı" });
            }
            return Ok(ilaclar);
        }


        [ServiceFilter(typeof(RefreshTokenFilter))]
        [HttpGet("GetAlanlar")]
        public IActionResult GetAlanlar()
        {
            var alanlar = _context.Doktors.Select(d => d.Alan).Distinct().ToList();
            if (alanlar.Count == 0)
            {
                return BadRequest(new { mesaj = "Kayıtlı uzmanlık alanı bulunamadı" });
            }
            return Ok(alanlar);
        }


        [ServiceFilter(typeof(RefreshTokenFilter))]
        [HttpPost("DisplayDoktor")]
        public IActionResult DisplayDoktor([FromBody] AlanRequestDTO req)
        {
            var doktorlarDTO = _cache.GetValueByAlan(req.Alan);

            if (doktorlarDTO.Count == null)
            {
                return BadRequest(new { mesaj = "Bu alanda doktor bulunamadı" });
            }
            return Ok(doktorlarDTO);
        }


        [ServiceFilter(typeof(RefreshTokenFilter))] // Refresh token kontrolü ile hangi doktor olduğunu anlıyoruz.
        [HttpPost("TedaviYaz")]
        public IActionResult TedaviYaz([FromBody] TedaviYazDTO dto, [FromHeader(Name = "Authorization")] string token)
        {
            try
            {
                string accessToken = token.ToString().Replace("Bearer ", "");
                Doktor doktor = _context.Doktors.FirstOrDefault(h => h.AccessToken == accessToken); // işlemi yapan doktor
                Hastum hasta = _context.Hasta.FirstOrDefault(h => h.Id == dto.HastaID); // İşlem yapılan hasta


                var Randevuları = _context.OnlineRandevus.Where(h => h.DoktorId == doktor.Id).ToList(); // doktorun randevuları ama List tipinde
                _logger.LogInformation($"Doktorun randevuları: {Randevuları.Count()}");
                List<int> HastaIDleri = new List<int>();


                foreach (var randevu in Randevuları)
                {
                    HastaIDleri.Add(randevu.HastaId); // Doktorun randevularındaki hasta ID'lerini tek bir yere attık
                }

                if (!HastaIDleri.Contains(dto.HastaID)) // Doktor doğru hasta Idsini yazdı mı diye kontrol ediyoruz.
                {
                    return BadRequest(new { mesaj = "Bu hastanın sizden randevusu yok." });
                }

                foreach (var item in dto.Ilaclar)
                {
                    var ilac = _context.Ilacs.FirstOrDefault(h => h.IlacName == item.ToString());
                    if (ilac.KullanımAlanı.Trim().ToLower() != doktor.Alan.Trim().ToLower())
                    {
                        return BadRequest(new { mesaj = "İlaç doktorun uzmanlık alanına uygun değil." });
                    }
                }


                // TEDAVİ YAZILACAK
                Tedavi tedavi = new Tedavi
                {
                    Tanı = dto.Tanı,
                    DoktorId = doktor.Id,
                    HastaId = hasta.Id
                };


                _context.Tedavis.Add(tedavi);
                _context.SaveChanges();

                //RECETE YAZACAK TEDAVİID = RECETEID

                Recete recete = new Recete
                {
                    Kullanım = dto.Kullanım,
                    GecerlilikTarihi = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                };

                recete.ReceteId = tedavi.TedaviId;
                _context.Recetes.Add(recete);
                _context.SaveChanges();


                if (dto.Ilaclar.Count != dto.IlacAdet.Count)
                {
                    return BadRequest(new { mesaj = "İlaç sayısı ile adet sayısı eşleşmiyor." });
                }


                foreach (var (ilac, adet) in dto.Ilaclar.Zip(dto.IlacAdet)) // DTO'daki Ilaclar ile adetleri karşılıklı eşleniyor.
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
        [Authorize(Roles = "Doktor,Resepsiyonist")]
        [HttpGet("RandevuGoster")]
        public IActionResult RandevuGöster(int userId)
        {
            var user = _context.Doktors.FirstOrDefault(c => c.Id == userId); // Randevularını görüntüleyeceğimiz doktor

            if (user == null)
            {
                return StatusCode(404, "Doktor bulunamadı");
            }

            var randevular = _context.OnlineRandevus.Where(c => c.DoktorId == userId)
                .Select(r => new
                {
                    r.Id,
                    r.HastaId,
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
