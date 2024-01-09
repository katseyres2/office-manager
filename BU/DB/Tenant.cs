using System;
using System.Collections.Generic;

namespace PGBD_Project.DB;

public partial class Tenant
{
    public int TenantId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int AddressId { get; set; }

    public bool Active { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Contract> Contracts { get; } = new List<Contract>();
}
