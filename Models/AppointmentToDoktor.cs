using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class AppointmentToDoktor
{
    public int Id { get; set; }

    public int DoktorFk { get; set; }

    public int AppointmentFk { get; set; }

    public virtual Appointment AppointmentFkNavigation { get; set; }

    public virtual Doktor DoktorFkNavigation { get; set; }
}
