using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Description { get; set; }
}
