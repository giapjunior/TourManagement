using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
