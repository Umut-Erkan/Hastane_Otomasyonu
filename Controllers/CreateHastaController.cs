using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hastane.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hastane_Otomasyonu.Controllers
{
    [Route("api/[controller]")]
    public class CreateHastaController : ControllerBase
    {
        private readonly HastaneContext _context;

        public CreateHastaController(HastaneContext context)
        {
            _context = context;
            
        }
        [HttpPost]
        public IActionResult CreateHasta(Hastum hasta)
        {
            try
            {
                _context.Hasta.Update(hasta);
                
                return default;

            }
            catch(Exception ex)
            {
                 return NotFound(new { mesaj = "Hata.",hata = ex });
            }
        
        }

        /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }*/
    }
}