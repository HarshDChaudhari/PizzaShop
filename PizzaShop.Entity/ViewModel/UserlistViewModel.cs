using System;

namespace PizzaShop.Entity.ViewModel;

public class UserlistViewModel
{

    public string? FirstName { get; set; }
    public string SortOrder { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }

    public string? Imgurl { get; set; }

    public string? Phone { get; set; }

    public bool? Status { get; set; }

    public string? RoleName { get; set; }
    public int? RoleId { get; set; }
    public int? UserId { get; set; }

    
}
