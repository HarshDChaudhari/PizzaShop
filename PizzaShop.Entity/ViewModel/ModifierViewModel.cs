namespace PizzaShop.Entity.ViewModel;

public class ModifierViewModel
{
    public int ModifierGroupId { get; set; }

    public string ModifierName { get; set; } = null!;

    public string? Description { get; set; }

    public int? Quantity { get; set; }

    public float? Rate { get; set; }


}
