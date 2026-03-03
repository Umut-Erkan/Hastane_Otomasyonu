using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class OnlineRandevu
{
    public int Id { get; set; }

    public string HastaŞikayet { get; set; }

    public TimeOnly Saat { get; set; }

    public DateOnly Tarih { get; set; }

    public string DoktorName { get; set; }

    public string DoktorSurname { get; set; }

    public string HastaName { get; set; }

    public string HastaSurname { get; set; }

    public int DoktorId { get; set; }

    public int HastaId { get; set; }

    public virtual Doktor Doktor { get; set; }

    public virtual Hastum Hasta { get; set; }

    public virtual ICollection<Kayıt> Kayıts { get; set; } = new List<Kayıt>();
}
