using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Entity.ViewModel;

public class MenuViewModel
{
    public int CategoryId {get; set;}
    
    [Required(ErrorMessage = "Category Name is required")]
    public string CategoryName {get; set;}
    public string Description {get; set;}

     public int ModifierGroupId { get; set; }

    public string ModifierName { get; set; } = null!;

    public string? DescriptionModifier { get; set; }

    public List<int> SelectedModifierIds {get; set;}
    public List<string> SelectedModifierName {get; set;}


    public List<CategoryViewModel> CategoriesList { get; set; } = new List<CategoryViewModel>();
    public List<ModifierViewModel> ModifierList { get; set; } = new List<ModifierViewModel>();

    // public List<CategoryListViewModel> ItemsList { get; set; }
    public PaginationViewModel<CategoryListViewModel> ItemsList { get; set; }
    public List<ModifierListViewModel> ModifierItemsList { get; set; } 

    public List<SelectedModifierList> SelectedModifierList { get; set; }

}

public class SelectedModifierList
{
    public int ModifierItemId { get; set; }

    public string ModifierItemName { get; set; } = null!;

}