using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class Venue
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public decimal? Price { get; set; }

    public int? Capacity { get; set; }

    public string? EstablishmentName { get; set; }

    public string? EstablishmentType { get; set; }

    public string? Description { get; set; }

    public string? WeddingVenueType { get; set; }

    public string? Status { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
