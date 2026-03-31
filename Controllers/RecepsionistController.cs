using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Hastane_Otomasyonu.Business;
using Hastane_Otomasyonu.DTO;
using Hastane_Otomasyonu.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyApiProject.Models;


namespace Hastane_Otomasyonu.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class RecepsionistController : ControllerBase
    {

        private readonly HastaneContext _context;
        private readonly TokenService _tokenService;
        private PasswordHashing _Hash;
        private readonly ILogger<AppointmentManagement> _logger;

        public RecepsionistController(HastaneContext context, TokenService tokenService, ILogger<AppointmentManagement> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;

            _Hash = new PasswordHashing();

        }

        [ServiceFilter(typeof(RefreshTokenFilter))]
        [HttpGet("Doktorları Görüntüle")]
        public IActionResult DoktorlarıGörüntüle()
        {
            var token = Request.Headers.Authorization;
            string accessToken = token.ToString().Replace("Bearer ", "");


            var Recepsionist = _context.HospitalReceptionists.FirstOrDefault(r => r.AccessToken == accessToken);
            if (Recepsionist == null)
            {
                return BadRequest(new
                {
                    mesaj = "Bir hata oluştu.",
                    hataDetayi = "Recepsionist bulunamadı.",
                    ekBilgi = ""
                });
            }

            try
            {
                var doktorlar = _context.Doktors.Where(d => d.Alan == Recepsionist.Alan).ToList();
                return Ok(doktorlar.Count());
            }


            catch (NullReferenceException ex)
            {
                return BadRequest(new
                {
                    mesaj = "Bir şeyler null.",
                    hataDetayi = ex.Message,
                    ekBilgi = ex.InnerException?.Message
                });
            }


            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mesaj = "Bir hata oluştu.",
                    hataDetayi = ex.Message,
                    ekBilgi = ex.InnerException?.Message
                });
            }


        }


    }
}