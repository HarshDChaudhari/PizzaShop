namespace PizzaShop.Entity.ViewModel;

public class PermissionViewModel
{
    public int? PermissionId { get; set; }
    public string ModuleName { get; set; }
    public bool CanView { get; set; }
    public bool CanAddEdit { get; set; }
    public bool CanDelete { get; set; }

}
