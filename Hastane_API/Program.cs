using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotenv.net;
using Hastane_Otomasyonu.Business;
using Hastane_Otomasyonu.Filters;
using Hastane_Otomasyonu.Middlewares;
using Hastane_Otomasyonu.Redis.Interfaces;
using Hastane_Otomasyonu.Redis.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyApiProject.Models;
using StackExchange.Redis;

DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { "../.env" }));


var builder = WebApplication.CreateBuilder(args);


var AppPolicy = "AppPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AppPolicy,
        policy =>
        {
            policy.SetIsOriginAllowed(origin => true)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// CONNECTION
var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException(
        "Connection string bulunamadı. `ConnectionStrings:DefaultConnection` değerini " +
        "User Secrets (Development) veya ortam değişkeni `ConnectionStrings__DefaultConnection` ile sağlayın.");
}

builder.Services.AddDbContext<HastaneContext>(options =>
        options.UseSqlServer(connectionString));



//REDIS
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisEndPoints = builder.Configuration["RedisConnection:EndPoints"];
    if (string.IsNullOrWhiteSpace(redisEndPoints))
    {
        throw new InvalidOperationException(
            "Redis ayarları bulunamadı. `RedisConnection:EndPoints` değerini " +
            "User Secrets (Development) veya ortam değişkeni `RedisConnection__EndPoints` ile sağlayın.");
    }

    var configurationOptions = new ConfigurationOptions
    {
        EndPoints = { redisEndPoints },
        User = builder.Configuration["RedisConnection:Username"],
        Password = builder.Configuration["RedisConnection:Password"]
    };

    return ConnectionMultiplexer.Connect(configurationOptions);
});


// TOKEN
builder.Services.AddScoped<TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    .AddJwtBearer(options =>
    {
        var jwtKey = Environment.GetEnvironmentVariable("JwtSettings__jwtKey") ?? builder.Configuration["JwtSettings:jwtKey"];
        if (string.IsNullOrEmpty(jwtKey)) throw new Exception("JWT Key is missing from configuration and environment!");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Token'ı kimin dağıttığını (Issuer) kontrol et
            ValidateAudience = true, // Token'ın kime hitap ettiğini (Audience) kontrol et
            ValidateLifetime = true, // Token'ın süresi dolmuş mu kontrol et (Çok önemli!)
            ValidateIssuerSigningKey = true, // Şifreleme anahtarının doğru olup olmadığını kontrol et

            ValidIssuer = builder.Configuration["JwtSettings:jwtIssuer"], // Bizim belirlediğimiz dağıtıcı adı
            ValidAudience = builder.Configuration["JwtSettings:Audience"], // Basitlik için Audience'ı da aynı yapıyoruz

            // Gizli anahtarımızı byte dizisine çevirip sisteme veriyoruz
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

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

// SEED VERI
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HastaneContext>();
    if (!context.Ilacs.Any()) // Eğer tablo boşsa
    {
        context.Ilacs.AddRange(new List<Ilac>
        {
            new Ilac { IlacName = "Roaccutane",                  KullanımAlanı = "Cildiye" },
            new Ilac { IlacName = "Aspirin",                     KullanımAlanı = "Kardiyoloji" },
            new Ilac { IlacName = "Lansor",                      KullanımAlanı = "Gastroenteroloji" },
            new Ilac { IlacName = "Metformin",                   KullanımAlanı = "Endokrinoloji" },
            new Ilac { IlacName = "Parasetamol (Minoset/Parol)", KullanımAlanı = "Dahiliye" },
            new Ilac { IlacName = "Ventolin",                    KullanımAlanı = "Göğüs Hastalıkları" },
            new Ilac { IlacName = "Xanax",                       KullanımAlanı = "Psikiyatri" },
            new Ilac { IlacName = "Amoksisilin (Antibiyotik)",   KullanımAlanı = "Enfeksiyon Hastalıkları" },
            new Ilac { IlacName = "Voltaren",                    KullanımAlanı = "Ortopedi" },
            new Ilac { IlacName = "Zyrtec",                      KullanımAlanı = "Alerji" },
            new Ilac { IlacName = "Beloc",                       KullanımAlanı = "Kardiyoloji" },
            new Ilac { IlacName = "Euthyrox",                    KullanımAlanı = "Endokrinoloji" },
            new Ilac { IlacName = "Arveles",                     KullanımAlanı = "Genel Cerrahi" },
            new Ilac { IlacName = "Fucidin",                     KullanımAlanı = "Cildiye" },
            new Ilac { IlacName = "Metformin",                   KullanımAlanı = "Dahiliye" },
        });
        context.SaveChanges();
    }
}


if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseCors(AppPolicy);

app.UseAuthentication();

app.UseAuthorization();

//app.UseMiddleware<AccessTokenMiddleware>();

app.MapControllers();
app.UseMiddleware<AuditLoggingMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


app.Run();