using System;
using System.Collections.Generic;
using Hastane_Otomasyonu.Models;

namespace MyApiProject.Models;

public partial class HospitalReceptionist : IUser
{
    public long Tc { get; set; }

    public string İsim { get; set; }

    public string Soyisim { get; set; }

    public string Eposta { get; set; }

    public string Password { get; set; }

    public string Role { get; set; }

    public string Alan { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenEndDate { get; set; }

    public int Id { get; set; }
}
