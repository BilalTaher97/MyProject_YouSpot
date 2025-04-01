using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class Cancellation
{
    public int Id { get; set; }

    public DateOnly? CancellationDate { get; set; }

    public string? Reason { get; set; }

    public string? CancellationStatus { get; set; }

    public int? BookingId { get; set; }

    public virtual Booking? Booking { get; set; }
}
