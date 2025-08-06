using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class TableOrderMapping
{
    public int TableOrderMappingId { get; set; }

    public int? TableId { get; set; }

    public int? OrderId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Order? Order { get; set; }

    public virtual TableDetail? Table { get; set; }
}
