using System;
using System.Collections.Generic;
using Hastane_Otomasyonu.Models;

namespace MyApiProject.Models;

public partial class Doktor : IUser
{
    public string İsim { get; set; }

    public string Soyisim { get; set; }

    public string Alan { get; set; }

    public string RandevuId { get; set; }

    public string Eposta { get; set; }

    public int Id { get; set; }

    public virtual ICollection<OnlineRandevu> OnlineRandevus { get; set; } = new List<OnlineRandevu>();

    public string Role { get; set; } = "Doktor";

}
