using System;
using System.Collections.Generic;

namespace PGBD_Project.DB;

public partial class Address
{
    public int AddressId { get; set; }

    public string Number { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string PostCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Office> Offices { get; } = new List<Office>();

    public virtual ICollection<Tenant> Tenants { get; } = new List<Tenant>();
}
