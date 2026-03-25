using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class Zaman
{
    public int Id { get; set; }

    public int? DoktorId { get; set; }

    public string Zaman1 { get; set; }

    public virtual Doktor Doktor { get; set; }
}
