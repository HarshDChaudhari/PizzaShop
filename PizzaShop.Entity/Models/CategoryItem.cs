using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class CategoryItem
{
    public int CategoryItemId { get; set; }

    public int? CategoryId { get; set; }

    public string ItemName { get; set; } = null!;

    public string? Description { get; set; }

    public int? Quantity { get; set; }

    public float? Price { get; set; }

    public bool? IsAvailable { get; set; }

    public string? ShortCode { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? IsFavourite { get; set; }

    public bool DefaultTax { get; set; }

    public int? UnitId { get; set; }

    public int? TaxPercentage { get; set; }

    public string? ItemType { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<CategoryModifierMapping> CategoryModifierMappings { get; set; } = new List<CategoryModifierMapping>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Unit? Unit { get; set; }
}
