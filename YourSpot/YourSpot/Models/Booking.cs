using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class Booking
{
    public int Id { get; set; }

    public DateOnly BookingDate { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public int? Price { get; set; }

    public int? NumberOfAttendees { get; set; }

    public string? Message { get; set; }

    public string Status { get; set; } = null!;

    public int? VenueId { get; set; }

    public int UserId { get; set; }

    public string TypeBook { get; set; } = null!;

    public int? PhotographerId { get; set; }

    public int? DressId { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User User { get; set; } = null!;

    public virtual Venue? Venue { get; set; }






  
    public virtual Photographer Photographer { get; set; }


    public virtual Dress Dress { get; set; }


}
