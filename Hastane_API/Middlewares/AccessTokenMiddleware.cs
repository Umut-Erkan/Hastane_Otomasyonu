using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Hastane_Otomasyonu.Business;
using Microsoft.AspNetCore.Http;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.Middlewares
{
    public class AccessTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AccessTokenMiddleware> _logger;

        public AccessTokenMiddleware(RequestDelegate next, ILogger<AccessTokenMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, HastaneContext dbContext, TokenService tokenService)
        {


            string authHeader = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader)) // Yeni kayıt oluyorsa bu middleware atlancak.
            {
                await _next(context);
                return;
            }

            string AccessToken = authHeader.Replace("Bearer ", "");

            try
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(AccessToken);
                var role = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (token.ValidTo < DateTime.UtcNow)
                {
                    if (role == "Hasta") //Hasta
                    {
                        var hasta = dbContext.Hasta.FirstOrDefault(h => h.AccessToken == AccessToken);
                        if (hasta != null)
                        {
                            hasta.AccessToken = tokenService.GenerateAccessToken(hasta);
                            dbContext.SaveChanges();
                        }
                    }
                    else if (role == "Doktor") //Doktor
                    {
                        var Doktor = dbContext.Doktors.FirstOrDefault(h => h.AccessToken == AccessToken);
                        if (Doktor != null)
                        {
                            Doktor.AccessToken = tokenService.GenerateAccessToken(Doktor);
                            dbContext.SaveChanges();
                        }
                    }
                    else if (role == "Resepsiyonist") //Resepsiyonist
                    {
                        var resepsiyonist = dbContext.HospitalReceptionists.FirstOrDefault(h => h.AccessToken == AccessToken);
                        if (resepsiyonist != null)
                        {
                            resepsiyonist.AccessToken = tokenService.GenerateAccessToken(resepsiyonist);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            catch
            {
                _logger.LogInformation("TokenMiddleware çalışmıyor.");
            }

            await _next(context);
        }
    }
}