using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class Unit
{
    public int UnitId { get; set; }

    public string UnitName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<CategoryItem> CategoryItems { get; set; } = new List<CategoryItem>();

    public virtual ICollection<ModifierItem> ModifierItems { get; set; } = new List<ModifierItem>();
}
