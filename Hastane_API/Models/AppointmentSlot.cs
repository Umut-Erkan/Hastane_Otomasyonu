using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class AppointmentSlot
{
    public int Id { get; set; }

    public DateOnly SlotDate { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsAvailable { get; set; }

    public int? DoktorId { get; set; }

    public virtual Doktor Doktor { get; set; }
}
