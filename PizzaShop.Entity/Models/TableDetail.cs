using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class TableDetail
{
    public int TableId { get; set; }

    public int? SectionId { get; set; }

    public string TableName { get; set; } = null!;

    public int? Capacity { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Section? Section { get; set; }

    public virtual ICollection<TableOrderMapping> TableOrderMappings { get; set; } = new List<TableOrderMapping>();
}
