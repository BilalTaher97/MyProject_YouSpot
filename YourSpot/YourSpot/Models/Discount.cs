using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class Discount
{
    public int Id { get; set; }

    public decimal? DiscountPercentage { get; set; }

    public string? DiscountType { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? DiscountStatus { get; set; }
}
