using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class TaxesAndFee
{
    public int TaxId { get; set; }

    public string TaxName { get; set; } = null!;

    public string? TaxType { get; set; }

    public decimal? TaxValue { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDefault { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<OrderTaxMapping> OrderTaxMappings { get; set; } = new List<OrderTaxMapping>();
}
