using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class Image
{
    public int Id { get; set; }

    public string? ServiceType { get; set; }

    public int? ServiceId { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }
}
