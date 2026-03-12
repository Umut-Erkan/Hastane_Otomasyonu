using System;
using System.Collections.Generic;
using Hastane_Otomasyonu.Models;
namespace MyApiProject.Models;

public partial class Hastum : IUser
{
    public long Tc { get; set; }

    public string İsim { get; set; }

    public string Soyisim { get; set; }

    public int Id { get; set; }

    public int? TedaviId { get; set; }

    public string Password { get; set; }

    public string Eposta { get; set; }

    public string Role { get; set; } = "Hasta";

    public string Token { get; set; }

    public virtual ICollection<OnlineRandevu> OnlineRandevus { get; set; } = new List<OnlineRandevu>();

    public virtual Tedavi Tedavi { get; set; }
}
