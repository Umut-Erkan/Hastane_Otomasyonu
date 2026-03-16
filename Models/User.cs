using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class User
{
    public int Id { get; set; }

    public virtual Doktor Doktor { get; set; }

    public virtual Hastum Hastum { get; set; }
}
