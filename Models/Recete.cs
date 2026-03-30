using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class Recete
{
    public string Kullanım { get; set; }

    public DateOnly GecerlilikTarihi { get; set; }

    public int ReceteId { get; set; }

    public virtual ICollection<IlcaToRecete> IlcaToRecetes { get; set; } = new List<IlcaToRecete>();

    public virtual Tedavi ReceteNavigation { get; set; }
}
