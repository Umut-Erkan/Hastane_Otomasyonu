using System;
using System.Collections.Generic;
using Hastane_Otomasyonu.Models;

namespace MyApiProject.Models;

public partial class Admin : IUser
{
    public string Password { get; set; }

    public string Eposta { get; set; }

    public string Role { get; set; }

    public string Token { get; set; }

    public int Id { get; set; }
}
