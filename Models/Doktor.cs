using System;
using System.Collections.Generic;
using Hastane_Otomasyonu.Models;

namespace MyApiProject.Models;

public partial class Doktor : IUser
{

    public List<DateOnly> MesaiGunu = new List<DateOnly>()
        {
            DateOnly.Parse("30-03-2026"),
            DateOnly.Parse("31-03-2026"),
            DateOnly.Parse("01-04-2026"),
            DateOnly.Parse("02-04-2026"),
            DateOnly.Parse("03-04-2026")
        };

    public List<TimeOnly> MesaiSaati = new List<TimeOnly>()
        {
            TimeOnly.Parse("09:00"),
            TimeOnly.Parse("10:00"),
            TimeOnly.Parse("11:00"),
            TimeOnly.Parse("12:00"),
            TimeOnly.Parse("13:00"),
            TimeOnly.Parse("14:00"),
            TimeOnly.Parse("15:00"),
            TimeOnly.Parse("16:00"),
            TimeOnly.Parse("17:00")
        };

    public string İsim { get; set; }

    public string Soyisim { get; set; }

    public string Alan { get; set; }

    public int Id { get; set; }

    public long Tc { get; set; }

    public string Password { get; set; }

    public string Eposta { get; set; }

    public string Role { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenEndDate { get; set; }

    public virtual ICollection<AppointmentSlot> AppointmentSlots { get; set; } = new List<AppointmentSlot>();

    public virtual ICollection<OnlineRandevu> OnlineRandevus { get; set; } = new List<OnlineRandevu>();

    public virtual ICollection<Tedavi> Tedavis { get; set; } = new List<Tedavi>();
}
