using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int? OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int? TableId { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? TaxId { get; set; }

    public decimal? FinalAmount { get; set; }

    public DateTime? IssuedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Order? Order { get; set; }

    public virtual TableDetail? Table { get; set; }

    public virtual TaxesAndFee? Tax { get; set; }
}
