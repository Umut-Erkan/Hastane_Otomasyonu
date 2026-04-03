using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class Tedavi
{
    public int TedaviId { get; set; }

    public int DoktorId { get; set; }

    public string Tanı { get; set; }

    public int HastaId { get; set; }

    public virtual Doktor Doktor { get; set; }

    public virtual Hastum Hasta { get; set; }

    public virtual Recete Recete { get; set; }
}
