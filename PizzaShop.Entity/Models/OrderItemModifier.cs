using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class OrderItemModifier
{
    public int OrderItemModifierId { get; set; }

    public int? OrderItemId { get; set; }

    public int? ModifierItemId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ModifierItem? ModifierItem { get; set; }

    public virtual OrderItem? OrderItem { get; set; }
}
