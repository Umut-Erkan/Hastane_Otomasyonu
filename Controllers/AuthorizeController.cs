using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hastane_Otomasyonu.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastane_Otomasyonu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizeController : ControllerBase
    {
        
        private UserModel GetCurrentUser()
        {
            Console.WriteLine();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
           
            if (identity != null)
            {
                var userClaims = identity.Claims; // Identity önemli bilgilerin bulunduğu yer, 
                                                // Claims bilgilerin tutulduğu sözlük (key-value) yapısıdır.
                
                return new UserModel
                {
                    Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }

        
          //For admin Only
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi you are an {currentUser.Role}");
        }    
    }
}