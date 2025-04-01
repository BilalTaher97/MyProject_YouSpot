using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class Review
{
    public int Id { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public int? UserId { get; set; }

    public int? ServiceId { get; set; }

    public string? ServiceType { get; set; }

    public DateTime? ReviewDate { get; set; }

    public virtual User? User { get; set; }
}
