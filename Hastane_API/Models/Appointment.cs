using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public DateOnly SlotDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public bool IsAvailable { get; set; }

    public virtual ICollection<AppointmentToDoktor> AppointmentToDoktors { get; set; } = new List<AppointmentToDoktor>();
}
