using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class Tour
{
    public int TourId { get; set; }

    public string TourName { get; set; } = null!;

    public string Destination { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int MaxSlot { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
