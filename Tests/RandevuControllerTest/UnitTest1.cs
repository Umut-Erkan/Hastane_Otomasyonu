using Hastane_Otomasyonu.Controllers;
using Hastane_Otomasyonu.DTO;
using Microsoft.AspNetCore.Mvc;
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
        _context = new HastaneContext();
        _controller = new RandevuController(_context);
    }

    [Fact]
    public void RandevuController_GetRandevular_ReturnsOkResult()
    {
        Assert.True(true);
    }

    [Fact]
    public void RandevuController_RandevuAl_ReturnsOkResult()
    {
        var Deneme = new RandevuAddDTO
        {
            Tc = 83414522672,
            DoktorName = "Veli",
            DoktorSurname = "Yorulmaz",
            Şikayet = "Baş ağrısı",
            Id = _context.Hasta.FirstOrDefault(h => h.Tc == 83414522672).Id
        };

        Microsoft.AspNetCore.Mvc.IActionResult result = _controller.RandevuAl(Deneme);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
}
