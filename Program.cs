using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hastane_Otomasyonu.Business;
using Hastane_Otomasyonu.Filters;
using Hastane_Otomasyonu.Redis.Interfaces;
using Hastane_Otomasyonu.Redis.Services;
using Hastane_Otomasyonu.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyApiProject.Models;

var builder = WebApplication.CreateBuilder(args);



// CONNECTION
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Bağlantı metni bulunamadı! appsettings.json dosyasını kontrol et.");
}

builder.Services.AddDbContext<HastaneContext>(options =>
        options.UseSqlServer(connectionString));



//REDİS

builder.Services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp => 
{
    var redisHost = builder.Configuration["RedisConnection:ConnectionString"];
    var redisPassword = builder.Configuration["RedisConnection:Password"];
    var connString = $"{redisHost},password={redisPassword}";
    return StackExchange.Redis.ConnectionMultiplexer.Connect(connString);
});


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
builder.Services.AddScoped<RefreshTokenFilter>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

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