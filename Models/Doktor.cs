using System;
using System.Collections.Generic;
using Hastane_Otomasyonu.Models;

namespace MyApiProject.Models;

public partial class Doktor : IUser
{
    public string İsim { get; set; }

    public string Soyisim { get; set; }

    public string Alan { get; set; }

    public int Id { get; set; }

    public long Tc { get; set; }

    public string Password { get; set; }

    public string Eposta { get; set; }

    public string Role { get; set; }

    public string Token { get; set; }

    public virtual User IdNavigation { get; set; }

    public virtual ICollection<OnlineRandevu> OnlineRandevus { get; set; } = new List<OnlineRandevu>();
}
