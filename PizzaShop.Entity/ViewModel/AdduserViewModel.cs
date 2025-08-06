using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization.Formatters;
using Microsoft.AspNetCore.Http;

namespace PizzaShop.Entity.ViewModel;

public class AdduserViewModel
{

    // public string? Email { get; set; }

    // public int? Role { get; set; }

    // public string? FirstName { get; set; }
    // public string? LastName { get; set; }

    // public string? Username { get; set; }

    // public string? Password { get; set; }

    // public int? Country { get; set; }


    // public int? State { get; set; }

    // public int? City { get; set; }

    // public string? Phone { get; set; }

    // public string? Address { get; set; }


    // public int? ZipCode { get; set; }
    public string? Imgurl { get; set; }
    public bool? Status { get; set; }
    public IFormFile FormFile { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public int? Role { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    // [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Country is required")]
    public int? Country { get; set; }

    [Required(ErrorMessage = "State is required")]
    public int? State { get; set; }

    [Required(ErrorMessage = "City is required")]
    public int? City { get; set; }

    [Required(ErrorMessage = "Zipcode is required")]
    [RegularExpression("^[0-9]{6}$", ErrorMessage = "Zip code must be a 6-digit number")]
    public int? ZipCode { get; set; }

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number must be a 10-digit number")]
    public string? Phone { get; set; }

}
