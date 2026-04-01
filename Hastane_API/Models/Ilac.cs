using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class Ilac
{
    public int IlacId { get; set; }

    public string IlacName { get; set; }

    public string KullanımAlanı { get; set; }

    public virtual ICollection<IlcaToRecete> IlcaToRecetes { get; set; } = new List<IlcaToRecete>();
}
