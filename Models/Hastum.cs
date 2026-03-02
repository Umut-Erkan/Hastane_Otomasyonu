using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class Hastum
{
    public int Tc { get; set; }

    public string İsim { get; set; } = null!;

    public string Soyisim { get; set; } = null!;

    public string Şikayet { get; set; } = null!;

    public int Id { get; set; }

    public string? RandevuId { get; set; }

    public int? TedaviId { get; set; }

    public virtual ICollection<OnlineRandevu> OnlineRandevus { get; set; } = new List<OnlineRandevu>();

    public virtual Tedavi? Tedavi { get; set; }
}
