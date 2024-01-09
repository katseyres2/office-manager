using System;
using System.Collections.Generic;

namespace PGBD_Project.DB;

public partial class Contract
{
    public int ContractId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int TenantId { get; set; }

    public int OfficeId { get; set; }

    public virtual Office Office { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;
}
