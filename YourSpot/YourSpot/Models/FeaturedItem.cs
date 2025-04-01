using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class FeaturedItem
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public string ItemType { get; set; } = null!;
}
