using Hastane.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<HastaneContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();