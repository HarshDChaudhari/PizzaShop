namespace PizzaShop.Entity.ViewModel;

public class ExistingModifierViewModel
{
    public int ModifierItemId { get; set; }
     public string ModifierItemName { get; set; } = null!;
     public string UnitName { get; set; } = null!;

    public float? Rate { get; set; }

    public int? UnitId { get; set; }

     public int? Quantity { get; set; }

    public string? Description { get; set; }
}
