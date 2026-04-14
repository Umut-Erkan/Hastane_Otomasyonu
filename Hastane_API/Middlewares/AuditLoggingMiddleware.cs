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
            // Proceed with the request first
            await _next(context);

            // Fetch the required information
            string serviceName = context.Request.Path.Value ?? "Unknown";
            string browserInfo = context.Request.Headers["User-Agent"].ToString();

            if (string.IsNullOrEmpty(browserInfo))
            {
                browserInfo = "Unknown";
            }

            // Truncate strings to match the database constraints of 50 chars
            if (serviceName.Length > 50)
                serviceName = serviceName.Substring(0, 50);

            if (browserInfo.Length > 50)
                browserInfo = browserInfo.Substring(0, 50);

            int userId = 0; // Default when unauthenticated

            // Extract the user ID from the claims if authenticated
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int parsedId))
                {
                    userId = parsedId;
                }
            }


            // Map and save to the Scaffolded AuditLog model


            var auditLog = new AuditLog
            {
                ServiceName = serviceName,
                BrowserInfo = browserInfo,
                Userıd = userId,
                Role = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown"
            };

            //dbContext.AuditLogs.Add(auditLog);
            //await dbContext.SaveChangesAsync();


            // F5 atıldığında tekrar kaydetmesin die

            var auditLogs = dbContext.AuditLogs.ToList();

            foreach (var item in auditLogs)
            {
                if (item == auditLog)
                {
                    _logger.LogInformation("Sayfa yenilendiği için veri eklenmedi.");
                    return;
                }
            }

            dbContext.AuditLogs.Add(auditLog);

            // Save the logs at the end of the request


            await dbContext.SaveChangesAsync();



        }
    }
}
