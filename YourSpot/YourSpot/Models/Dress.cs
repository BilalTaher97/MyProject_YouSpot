using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class Dress
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public decimal? Price { get; set; }

    public string? Size { get; set; }

    public string? Brand { get; set; }

    public string? Status { get; set; }

    public bool? IsActive { get; set; }
}
