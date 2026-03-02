using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class Tedavi
{
    public string Ilaç { get; set; } = null!;

    public string Tedavi1 { get; set; } = null!;

    public int TedaviId { get; set; }

    public virtual ICollection<Hastum> Hasta { get; set; } = new List<Hastum>();
}
