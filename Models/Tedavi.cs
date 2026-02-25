using System;
using System.Collections.Generic;

namespace Hastane.Models;

public partial class Tedavi : Hastum
{
    public string Ilaç { get; set; } = null!;

    public string? Tedavi1 { get; set; }

    public int? TedaviId { get; set; }

    public virtual Hastum? TedaviNavigation { get; set; }
}
