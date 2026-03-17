using MyApiProject.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;  
using System.Text;
using Hastane_Otomasyonu.Business;
using Microsoft.Extensions.Options;
using Hastane_Otomasyonu.Filters;

var builder = WebApplication.CreateBuilder(args);



// CONNECTION
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Bağlantı metni bulunamadı! appsettings.json dosyasını kontrol et.");
}

builder.Services.AddDbContext<HastaneContext>(options =>
        options.UseSqlServer(connectionString));  

// TOKEN
builder.Services.AddScoped<TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Token'ı kimin dağıttığını (Issuer) kontrol et
            ValidateAudience = true, // Token'ın kime hitap ettiğini (Audience) kontrol et
            ValidateLifetime = true, // Token'ın süresi dolmuş mu kontrol et (Çok önemli!)
            ValidateIssuerSigningKey = true, // Şifreleme anahtarının doğru olup olmadığını kontrol et

            ValidIssuer = builder.Configuration["JwtSettings:jwtIssuer"], // Bizim belirlediğimiz dağıtıcı adı
            ValidAudience = builder.Configuration["JwtSettings:Audience"], // Basitlik için Audience'ı da aynı yapıyoruz
            
            // Gizli anahtarımızı byte dizisine çevirip sisteme veriyoruz
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:jwtKey"])),

            // Varsayılan 5 dakikalık toleransı sıfırlıyoruz → Token TAM olarak belirtilen anda sona erer
            ClockSkew = TimeSpan.Zero
        };
        
        
    }
    );
    
builder.Services.AddControllers();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<PasswordHashing>();
builder.Services.AddScoped<ActionFilter>();

builder.Services.AddHttpContextAccessor();
// SWAGGER
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();



var app = builder.Build();



app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.UseSwaggerUI();
}


app.Run();