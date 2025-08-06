using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Entity.ViewModel;

public class Login
{
    [Required]
    [EmailAddress]
    public required string email { get; set; }

    [Required]

    public required string password { get; set; }

    public bool Rememberme { get; set; }

}
