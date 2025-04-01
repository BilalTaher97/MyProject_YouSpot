using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string? CardNumber { get; set; }

    public string? Name { get; set; }

    public DateOnly? Date { get; set; }

    public int? BookingId { get; set; }

    public int? UserId { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual User? User { get; set; }
}
