using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Hastane.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hastane_Otomasyonu.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class GetController : ControllerBase
    {
        //private readonly ILogger<Get> _logger;
        private readonly HastaneContext _context;
        public GetController(HastaneContext context)
        {
            _context = context;
        }

        
        /*public Get(HastaneContext context)
        {
            _context = context;
        }*/
        [HttpGet]
        public IActionResult GetAll()
        {
            try{
                var doktor = _context.Doktors;

                var isimler = doktor
                .Where(q => q.Alan == "Kardiyoloji")
                .Select(q => q.İsim)
                .ToList();

                if (isimler.Count > 0)
                {
                    return Ok(isimler);
                    
                }
                else
                {
                    return NotFound(new { mesaj = "Kardiyoloji alanında herhangi bir doktor bulunamadı." });
                }
            }

            catch (Exception ex)
                {
                    // 5. Olası bir veritabanı bağlantı hatasında sistemin çökmesini engelledik.
                    return StatusCode(500, new { mesaj = "İşlem sırasında sunucuda bir hata oluştu.", detay = ex.Message });
                }
        }

        /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return default;
        }*/
    }
}