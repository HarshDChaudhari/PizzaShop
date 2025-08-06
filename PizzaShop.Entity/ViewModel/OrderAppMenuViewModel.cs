

namespace PizzaShop.Entity.ViewModel;

public class OrderAppMenuViewModel
{

    public List<CategoryViewModel> Categories { get; set; }
    public List<ItemOrderMenu> Items { get; set; }
    public int SelectedCategoryId { get; set; }
    public string SearchQuery { get; set; }
    public List<ModifierListViewModel>? SelectedModifierGroups { get; set; } = new List<ModifierListViewModel>();
    public MenuOrderCardVm menuOrderDetails { get; set; } = new MenuOrderCardVm();
    public int? OrderId { get; set; }
}

public class ItemOrderMenu
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemType { get; set; }
    public float? ItemPrice { get; set; }
    public int? TaxPercentage { get; set; }
    public string ImgUrl { get; set; }
    public bool? IsFavorite { get; set; }
}


public class MenuOrderCardVm
{
    public string section { get; set; }
    public List<string> tables { get; set; }
}
