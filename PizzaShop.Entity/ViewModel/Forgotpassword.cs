using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Entity.ViewModel;

public class Forgotpassword
{
    [Required(ErrorMessage = "The email address is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    // [RegularExpression]
    public required string email { get; set; }
}
