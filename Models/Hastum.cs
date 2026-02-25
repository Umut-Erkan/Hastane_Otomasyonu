using System;
using System.Collections.Generic;

namespace Hastane.Models;

public partial class Hastum
{
    public int Tc { get; set; }

    public string? İsim { get; set; }

    public string? Soyisim { get; set; }

    public string? Şikayet { get; set; }

    public int? Id { get; set; }
    // OnlineRandevu ile one to one ilişki
    public virtual OnlineRandevu? OnlineRandevu { get; set; }
}
