using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class Booking
{
    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public int ScheduleId { get; set; }

    public DateTime? BookingDate { get; set; }

    public int NumberOfPeople { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public string? CancelReason { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Schedule Schedule { get; set; } = null!;
}
