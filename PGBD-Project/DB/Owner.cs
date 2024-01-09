using System;
using System.Collections.Generic;

namespace PGBD_Project.DB;

public partial class Owner
{
    public int OwnerId { get; set; }

    public string? Label { get; set; }

    public string? Tva { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Office> Offices { get; } = new List<Office>();
}
