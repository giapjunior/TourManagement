using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Address { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
