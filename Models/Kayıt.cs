using System;
using System.Collections.Generic;

namespace Hastane.Models;

public partial class Kayıt
{
    public int? RandevuFk { get; set; }

    public string? YönlendirmeFişi { get; set; }

    public int KayıtId { get; set; }

    public virtual OnlineRandevu? RandevuFkNavigation { get; set; }

    public OnlineRandevu Fis( )
    {
        return RandevuFkNavigation;
    }
}
