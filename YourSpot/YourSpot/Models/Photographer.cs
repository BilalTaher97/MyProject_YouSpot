using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class Photographer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public int? Price { get; set; }

    public string? Status { get; set; }

    public bool? IsActive { get; set; }
}
