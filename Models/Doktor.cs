using System;
using System.Collections.Generic;

namespace Hastane.Models;

public partial class Doktor
{
    public string İsim { get; set; } = null!;

    public string Soyisim { get; set; } = null!;

    public string Alan { get; set; } = null!;

    public string? Randevuları { get; set; }

    public int Id { get; set; }

    public virtual ICollection<OnlineRandevu> OnlineRandevus { get; set; } = new List<OnlineRandevu>();
}
