using System;
using System.Collections.Generic;

namespace YourSpot.Models;

public partial class Notification
{
    public int Id { get; set; }

    public string? NotificationType { get; set; }

    public int? UserId { get; set; }

    public DateTime? NotificationDate { get; set; }

    public string? NotificationMessage { get; set; }

    public virtual User? User { get; set; }
}
