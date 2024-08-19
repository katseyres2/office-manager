using System;
using System.Collections.Generic;

namespace PGBD_Project.DB;

public partial class 
    Office
{
    public int OfficeId { get; set; }

    public double? Surface { get; set; }

    public string? Description { get; set; }

    public double? Rent { get; set; }

    public bool Active { get; set; }

    public int? Type { get; set; }

    public int OwnerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Contract> Contracts { get; } = new List<Contract>();

    public virtual Owner Owner { get; set; } = null!;
}
