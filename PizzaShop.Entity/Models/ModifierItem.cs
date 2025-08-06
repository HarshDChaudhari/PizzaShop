using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class ModifierItem
{
    public int ModifierItemId { get; set; }

    public string ModifierItemName { get; set; } = null!;

    public float? Rate { get; set; }

    public int? UnitId { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public int? Quantity { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<MappingItemModifier> MappingItemModifiers { get; set; } = new List<MappingItemModifier>();

    public virtual ICollection<OrderItemModifier> OrderItemModifiers { get; set; } = new List<OrderItemModifier>();

    public virtual Unit? Unit { get; set; }
}
