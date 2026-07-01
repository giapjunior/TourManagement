using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int TourId { get; set; }

    public DateOnly DepartureDate { get; set; }

    public DateOnly ReturnDate { get; set; }

    public int AvailableSlot { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Tour Tour { get; set; } = null!;
}
