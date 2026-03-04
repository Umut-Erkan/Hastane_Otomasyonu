using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class Doktor
{
    public string İsim { get; set; }

    public string Soyisim { get; set; }

    public string Alan { get; set; }

    public string RandevuId { get; set; }

    public int Id { get; set; }

    public virtual ICollection<OnlineRandevu> OnlineRandevus { get; set; } = new List<OnlineRandevu>();

    public string Role { get; set; } = "Doktor";

}
