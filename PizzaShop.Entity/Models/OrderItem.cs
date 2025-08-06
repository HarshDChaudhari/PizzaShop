using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int? OrderId { get; set; }

    public int? CategoryItemId { get; set; }

    public int Quantity { get; set; }

    public string? Instruction { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public int? ReadyQuantity { get; set; }

    public string? Status { get; set; }

    public virtual CategoryItem? CategoryItem { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<OrderItemModifier> OrderItemModifiers { get; set; } = new List<OrderItemModifier>();
}
