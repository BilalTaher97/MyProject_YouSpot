using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class TimeSlot
{
    public int SlotId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public bool IsBooked { get; set; }

    public int DateId { get; set; }

    public virtual AvailableDate Date { get; set; } = null!;
}
