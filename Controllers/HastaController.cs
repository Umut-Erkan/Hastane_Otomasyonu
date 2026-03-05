using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hastane_Otomasyonu.DTO;
using Microsoft.VisualBasic;
using MyApiProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Hastane_Otomasyonu.Business;

namespace Hastane_Otomasyonu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HastaController : ControllerBase
    {
        private readonly HastaneContext _context;
        private readonly TokenService _tokenService;
        private readonly ILogger<HastaController> _logger;
        private PasswordHashing _Hash;

        public HastaController(HastaneContext context , TokenService tokenService , ILogger<HastaController> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;

            PasswordHashing _Hash = new PasswordHashing();
        }


        [HttpPost]
        public IActionResult CreateHasta([FromBody] HastaDTO dto) 
        {
            try
            {
                var NewEntity = new Hastum // DTO -> Entity
                {
                    Tc = dto.Tc,
                    İsim = dto.Name, 
                    Soyisim = dto.Surname,
                    Password = _Hash.HashPassword(dto.Password), // sol taraftaki veritabanına ekliyceğimiz Hastum'un sahip olduğu 
                    Eposta = dto.Eposta,                        // veriye dönüşür.
                    Role = "Hasta",
                    Token = "PlaceHolder"
                };

               
               
                if (NewEntity.Token == null)
                {
                  _logger.LogInformation("Token'e koyduğum place holder çalışmıyor");   
                }
                else
                {
                    _logger.LogInformation("Token'de geçici olarak place holder duruyor.");
                }

                bool TcKontrol = _context.Hasta.Any(h=> h.Tc == NewEntity.Tc);
                Console.Write("Hasta oluşturuldu ve TC si kontrol edildi" , TcKontrol);
               
                if (TcKontrol == true)
                {
                    return StatusCode(500, new
                    {
                        Baslik = "var olan entity",
                        mesaj = "Var olan hasta eklenmeye çalışıldı"
                    });
                }

                else if (dto.Tc.ToString().Length != 11)
                {
                    return new ObjectResult("Tc'nin 11 haneli olması lazım"){StatusCode = 400};
                }
                else if (dto.Password.Count() < 10 || !dto.Password.Any(char.IsUpper))
                {
                    return new ObjectResult("Şifre 10 haneden küçük ya da Büyük karakter yok"){StatusCode = 400};
                }

                else
                {   
                var token = _tokenService.GenerateToken(NewEntity);

                NewEntity.Token = token;
                _context.Hasta.Add(NewEntity);
                _context.SaveChanges();
                
                
                Console.Write("TOKEN: " , token);
                _logger.LogInformation(token);

                return Ok("Token oluşturuldu, Hastanın özelliklerinden erişebilirsiniz");
                }
            }



            catch (DbUpdateException ex) 
            {
                // InnerException null olabilir, o yüzden ?. kullanıyoruz
                return BadRequest(new 
                { 
                    mesaj = "Veritabanına kaydederken hata oluştu.",
                    hata = ex.Message,
                    detay = ex.InnerException?.Message 
                });
            }

            catch (NullReferenceException ex) 
            {
                // ex.Message alıyoruz, ex'in kendisini değil
                return BadRequest(new { mesaj = "Beklenmedik bir veri boşluğu oluştu.", hata = ex.Message });
            }

           catch (Exception ex)
{
            // InnerException'ı değil, onun mesajını alıyoruz
            return BadRequest(new { 
                mesaj = "Bir hata oluştu.", 
                hataDetayi = ex.Message,
                ekBilgi = ex.InnerException?.Message
            });
            }
            }

            
        


        [Authorize (Roles = "Hasta")]
        [HttpGet]
        public IActionResult RandevuGöster([FromBody] HastaDTO hastadto)
        
        {
            var Hastamız = _context.Hasta.FirstOrDefault(h => h.Tc == hastadto.Tc);
            if (Hastamız == null)
            {
                return StatusCode(500,"Kayıtlı hasta bulunamadı");
            }
            
            else if(Hastamız.RandevuId == null)
            {
                return StatusCode(200, Hastamız.Eposta);
            };
            
            return new ObjectResult ($"Hastanın randevularını ID'leri: {Hastamız.RandevuId}"){StatusCode = 200};
        }
    }}