using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class Section
{
    public int SectionId { get; set; }

    public string SectionName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<TableDetail> TableDetails { get; set; } = new List<TableDetail>();

    public virtual ICollection<WaitingList> WaitingLists { get; set; } = new List<WaitingList>();
}
