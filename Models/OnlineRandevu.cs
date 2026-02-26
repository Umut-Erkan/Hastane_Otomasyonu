using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class OnlineRandevu
{
    public int Id { get; set; }

    public int? HastaId { get; set; }

    public int? DoktorId { get; set; }

    public TimeOnly Saat { get; set; }

    public DateOnly Tarih { get; set; }

    public virtual Doktor? Doktor { get; set; }

    public virtual Hastum IdNavigation { get; set; } = null!;

    public virtual ICollection<Kayıt> Kayıts { get; set; } = new List<Kayıt>();
}
