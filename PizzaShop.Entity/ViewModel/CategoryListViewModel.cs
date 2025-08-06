using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PizzaShop.Entity.ViewModel;

public class CategoryListViewModel
{
    public int CategoryItemId { get; set; }
    public int ModifierGroupId { get; set; }

    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "Item Name is required")]
    public string ItemName { get; set; } = null!;

    public string? Description { get; set; }
    public bool DefaultTax { get; set; }

    public int? UnitId { get; set; }

    [Required(ErrorMessage = "TaxPercentage is required")]
    public int? TaxPercentage { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    public int? Quantity { get; set; }

    [Required(ErrorMessage = "Price is required")]
    public float? Price { get; set; }

    public string ItemType { get; set; }
    public bool IsAvailable { get; set; }

    public string? ShortCode { get; set; }

    public string? ImageUrl { get; set; }

     public IFormFile FormFile { get; set; }

     public string tempFormFile {get; set;}

    public List<CategoryModifierMappingsViewModel> SelectedModifiers { get; set; }
    // public List<CategoryModifierMappingsEditViewModel> SelectedModifiersEdit { get; set; }
    public string SelectedModifiersString { get; set; }




}

public class CategoryModifierMappingsViewModel
{

    public int? CategoryItemId { get; set; }
    public int? ModifierGroupId { get; set; }
    public string? ModifierGroupName { get; set; }
    public int ModifierId { get; set; }
    public int? MaxValue { get; set; }
    public int? MinValue { get; set; }
}
// public class CategoryModifierMappingsEditViewModel
// {

//     public int ModifierId { get; set; }
//     public int? MaxValue { get; set; }
//     public int? MinValue { get; set; }
// }