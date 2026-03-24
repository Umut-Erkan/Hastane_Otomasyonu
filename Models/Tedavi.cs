using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class Tedavi
{
    public string Recete { get; set; }

    public string Tedavi1 { get; set; }

    public int TedaviId { get; set; }

    public int DoktorId { get; set; }

    public string Tanı { get; set; }

    public int HastaId { get; set; }

    public virtual Doktor Doktor { get; set; }

    public virtual Hastum Hasta { get; set; }
}
