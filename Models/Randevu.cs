using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class Randevu
{
    public string? ŞikayetFk { get; set; }

    public string? Poliklinik { get; set; }

    public string? DoktorFk { get; set; }

    public string? HastaBilgisiFk { get; set; }

    public TimeOnly Saat { get; set; }

    public int Id { get; set; }
}
