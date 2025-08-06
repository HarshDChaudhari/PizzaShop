using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? IsDeleted1 { get; set; }

    public virtual ICollection<CategoryItem> CategoryItems { get; set; } = new List<CategoryItem>();
}
