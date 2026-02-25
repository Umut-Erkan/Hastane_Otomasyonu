using System;
using System.Collections.Generic;

namespace Hastane.Models;

public partial class OnlineRandevu
{
    public int Id { get; set; }

    public int? HastaId { get; set; }

    public int? DoktorId { get; set; }

    public TimeOnly Saat { get; set; }

    public DateOnly Tarih { get; set; }
    // OnlineRandevu bir tane Doktor alabilir
    public virtual Doktor? Doktor { get; set; }
    // OnlineRandevu bir tane Hasta alabilir
    public virtual Hastum IdNavigation { get; set; } = null!;

    public virtual ICollection<Kayıt> Kayıts { get; set; } = new List<Kayıt>();
}
