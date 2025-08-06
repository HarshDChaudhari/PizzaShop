using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Entity.ViewModel;

public class Changepassword
{
    [Required(ErrorMessage = "Email is required")]
    public string email { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "NewPassword is required")]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "ConfirmPassword is required")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }
}
