using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class Hastum
{
    public int Tc { get; set; }

    public string? İsim { get; set; }

    public string? Soyisim { get; set; }

    public string? Şikayet { get; set; }

    public int Id { get; set; }

    public virtual ICollection<OnlineRandevu> OnlineRandevus { get; set; } = new List<OnlineRandevu>();

    public virtual Tedavi? Tedavi { get; set; }
}
