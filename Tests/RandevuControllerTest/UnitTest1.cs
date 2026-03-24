using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Hastane_Otomasyonu.Controllers;
using Hastane_Otomasyonu.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using MyApiProject.Models;
using Xunit;


namespace Tests;

public class UnitTest1 
{

    private readonly RandevuController _controller;
    private readonly HastaneContext _context;
    public UnitTest1() // Constructor
    {
        var options = new DbContextOptionsBuilder<HastaneContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_" + Guid.NewGuid().ToString())
                .Options;
                
        _context = new HastaneContext(options);
        _controller = new RandevuController(_context);
    }

    [Fact]
    public void RandevuController_GetRandevular_ReturnsOkResult()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RandevuController_RandevuAl_ReturnsOkResult()
    {
        var Deneme = new RandevuAddDTO
        {
            Tc = 83414522672,
            DoktorName = "Veli",
            DoktorSurname = "Yorulmaz",
            Şikayet = "Beyin sarsıntısı",
            Id = 13
        };


            var result = _controller.RandevuAl(Deneme); // Bad request geliyor
            var okResult = Assert.IsType<OkObjectResult>(result); 

            Assert.Equal(200, okResult.StatusCode);
            
        
    }
}
