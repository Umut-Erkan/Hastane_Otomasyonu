using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class OnlineRandevu
{
    public int Id { get; set; }

    public string HastaŞikayet { get; set; } = null!;

    public int DoktorId { get; set; }

    public TimeOnly Saat { get; set; }

    public DateOnly Tarih { get; set; }

    public string DoktorName { get; set; } = null!;

    public string DoktorSurname { get; set; } = null!;

    public string HastaName { get; set; } = null!;

    public string HastaSurname { get; set; } = null!;

    public virtual Doktor Doktor { get; set; } = null!;

    public virtual Hastum IdNavigation { get; set; } = null!;

    public virtual ICollection<Kayıt> Kayıts { get; set; } = new List<Kayıt>();
}
