using MyApiProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Bağlantı metni bulunamadı! appsettings.json dosyasını kontrol et.");
}

builder.Services.AddDbContext<HastaneContext>(options =>
        options.UseSqlServer(connectionString));  

builder.Services.AddControllers();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();