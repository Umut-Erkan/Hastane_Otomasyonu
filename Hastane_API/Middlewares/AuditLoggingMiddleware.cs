using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.Middlewares
{
    public class AuditLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditLoggingMiddleware> _logger;

        public AuditLoggingMiddleware(RequestDelegate next, ILogger<AuditLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // DbContext is injected into the InvokeAsync method because it is a Scoped service.
        public async Task InvokeAsync(HttpContext context, HastaneContext dbContext)
        {
            try
            {
                string serviceName = context.Request.Path.Value ?? "Unknown";
                string browserInfo = context.Request.Headers["User-Agent"].ToString();

                string ipAddress = context.Connection.RemoteIpAddress?.ToString();
                if (ipAddress == null || ipAddress == "")
                    ipAddress = "Unknown";


                if (string.IsNullOrEmpty(browserInfo))
                {
                    browserInfo = "Unknown";
                }

                string authHeader = context.Request.Headers["Authorization"].ToString();
                int userId = 0;
                string Role = "Unknown";



                if (!string.IsNullOrEmpty(authHeader) && authHeader != "Null")
                {
                    string AccessToken = authHeader.Replace("Bearer ", "");
                    var token = new JwtSecurityTokenHandler().ReadJwtToken(AccessToken);
                    var nameIdClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                    if (nameIdClaim != null)
                    {
                        userId = int.Parse(nameIdClaim);
                        Role = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                    }

                }

                browserInfo = browserInfo.Length > 50 ? browserInfo.Substring(0, 50) : browserInfo;
                serviceName = serviceName.Length > 50 ? serviceName.Substring(0, 50) : serviceName;



                var auditLog = new AuditLog
                {
                    ServiceName = serviceName,
                    BrowserInfo = browserInfo,
                    Userıd = userId,
                    Role = Role,
                    IpAddress = ipAddress,
                    Time = DateTime.Now
                };

                // F5 atıldığında tekrar kaydetmesin 

                bool zatenVar = dbContext.AuditLogs.Any(item =>
                    item.ServiceName == auditLog.ServiceName &&
                    item.Userıd == auditLog.Userıd &&
                    item.Role == auditLog.Role &&
                    item.Time == auditLog.Time

                );

                if (zatenVar)
                {
                    _logger.LogInformation("Sayfa yenilendiği için Log kayıt edilmedi.");
                    await _next(context);
                    return;
                }

                dbContext.AuditLogs.Add(auditLog);

                await dbContext.SaveChangesAsync();

                await _next(context);
            }
            catch

            {
                _logger.LogError("AuditLogları uretilemedi.");
            }
        }
    }
}
