using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class MappingItemModifier
{
    public int MappingItem { get; set; }

    public int ModifierGroupId { get; set; }

    public int ModifierId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ModifierItem Modifier { get; set; } = null!;

    public virtual ModifierGroup ModifierGroup { get; set; } = null!;
}
