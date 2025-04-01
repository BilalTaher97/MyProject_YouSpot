using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class FeaturedVenue
{
    public int Id { get; set; }

    public int? VenueId { get; set; }

    public DateOnly? FeaturedDate { get; set; }

    public virtual Venue? Venue { get; set; }
}
