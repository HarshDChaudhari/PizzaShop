namespace PizzaShop.Entity.ViewModel;

public class RoleViewModel
{

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public List<PermissionViewModel> PermissionList { get; set; }  

}
