using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class CategoryModifierMapping
{
    public int CategoryModifierMappingId { get; set; }

    public int? CategoryItemId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public int? ModifierId { get; set; }

    public int? MaxValue { get; set; }

    public int? MinValue { get; set; }

    public virtual CategoryItem? CategoryItem { get; set; }

    public virtual ModifierGroup? Modifier { get; set; }
}
