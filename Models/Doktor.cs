using System;
using System.Collections.Generic;

namespace Hastane.Models;

public partial class Doktor
{
    public string? İsim { get; set; }

    public string? Soyisim { get; set; }

    public string? Alan { get; set; }

    public string? Randevuları { get; set; }

    public int Id { get; set; }
    // OnlineRandevu ile One to Many ilişki
    // OnlineRandevu objesi alabilen içi boş bir liste oluşturulur.
    public virtual ICollection<OnlineRandevu> OnlineRandevus { get; set; } = new List<OnlineRandevu>();
}
