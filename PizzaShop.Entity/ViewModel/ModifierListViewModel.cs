using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Entity.ViewModel;

public class ModifierListViewModel
{
    public int? ModifierItemId { get; set; }

    public int ModifierGroupId { get; set; }
    public int? MaxValue { get; set; }
    public int? MinValue { get; set; }

    [Required(ErrorMessage = "Modifier Item Name is required")]
    public string ModifierItemName { get; set; }
    public string? ModifierGroupName { get; set; }

    [Required(ErrorMessage = "Rate is required")]
    public float Rate { get; set; }

    public int? UnitId { get; set; }
    public string? UnitName { get; set; }

    // public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    public int Quantity { get; set; }

    public string? Description { get; set; }


    public List<ModifierList> ModifierLists = new List<ModifierList>();


}

    public class ModifierList
    {
    public float? Rate { get; set; }
    public string ModifierItemName { get; set; }

    public int ModifierItemId { get; set; }
    }