using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class WaitingList
{
    public int WaitingId { get; set; }

    public int? SectionId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public int? TotalPerson { get; set; }

    public string? Phone { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Section? Section { get; set; }
}
