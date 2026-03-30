using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class IlcaToRecete
{
    public int ReceteFk { get; set; }

    public int IlcaFk { get; set; }

    public int Id { get; set; }

    public int Adet { get; set; }

    public virtual Ilac IlcaFkNavigation { get; set; }

    public virtual Recete ReceteFkNavigation { get; set; }
}
