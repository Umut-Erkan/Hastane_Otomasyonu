using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.Filters
{
    public class RefreshTokenFilter : IActionFilter
    {
        private readonly HastaneContext _context;

        public RefreshTokenFilter(HastaneContext context)
        {
            _context = context;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // "Bearer " kısmını çıkar
            string accessToken = context.HttpContext.Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(accessToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Token içindeki Role bilgisini oku
            string role = new JwtSecurityTokenHandler()
                .ReadJwtToken(accessToken)
                .Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value; // Access Token'ın içindeki role 

            if (role == "Doktor")
            {
                Doktor doktor = _context.Doktors.FirstOrDefault(h => h.AccessToken == accessToken); // Access Token'in sahibi
                if (doktor.RefreshTokenEndDate < DateTime.Now) // Refresh token ömrü değerlendiriliyor
                {
                    context.Result = new ObjectResult("Refresh Token süresi dolmuş") { StatusCode = 403 };
                }
            }
            else if (role == "Hasta")
            {
                Hastum hasta = _context.Hasta.FirstOrDefault(h => h.AccessToken == accessToken);
                if (hasta.RefreshTokenEndDate < DateTime.Now)
                {
                    context.Result = new ObjectResult("Refresh Token süresi dolmuş") { StatusCode = 403 };
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}