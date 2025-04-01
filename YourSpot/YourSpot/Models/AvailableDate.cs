using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class AvailableDate
{
    public int DateId { get; set; }

    public DateOnly BookingDate { get; set; }

    public int VenueId { get; set; }

    public virtual ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();

    public virtual Venue Venue { get; set; } = null!;
}
