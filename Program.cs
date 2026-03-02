using MyApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Hastane_Otomasyonu.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;   
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Bağlantı metni bulunamadı! appsettings.json dosyasını kontrol et.");
}

builder.Services.AddDbContext<HastaneContext>(options =>
        options.UseSqlServer(connectionString));  


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddBearerToken(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Token'ı kimin dağıttığını (Issuer) kontrol et
            ValidateAudience = true, // Token'ın kime hitap ettiğini (Audience) kontrol et
            ValidateLifetime = true, // Token'ın süresi dolmuş mu kontrol et (Çok önemli!)
            ValidateIssuerSigningKey = true, // Şifreleme anahtarının doğru olup olmadığını kontrol et

            ValidIssuer = "jwtIssuer", // Bizim belirlediğimiz dağıtıcı adı
            ValidAudience = "jwtIssuer", // Basitlik için Audience'ı da aynı yapıyoruz
            
            // Gizli anahtarımızı byte dizisine çevirip sisteme veriyoruz
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    }
    );

builder.Services.AddControllers();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();