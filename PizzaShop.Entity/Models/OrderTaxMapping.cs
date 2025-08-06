using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class OrderTaxMapping
{
    public int OrderTaxId { get; set; }

    public int? OrderId { get; set; }

    public int? TaxId { get; set; }

    public float? TaxValue { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? TaxType { get; set; }

    public virtual Order? Order { get; set; }

    public virtual TaxesAndFee? Tax { get; set; }
}
