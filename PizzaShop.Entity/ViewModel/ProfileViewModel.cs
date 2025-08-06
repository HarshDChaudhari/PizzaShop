using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization.Formatters;
using Microsoft.AspNetCore.Http;

namespace PizzaShop.Entity.ViewModel;

public class ProfileViewModel
{

    public int Id { get; set; }
    public string? Email { get; set; }

    public string? Role { get; set; }
    [Required(ErrorMessage = "First Name is required.")]
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "Last Name is required.")]
    public string? LastName { get; set; }
    [Required(ErrorMessage = "UserName is required.")]
    public string? Username { get; set; }

    public int? Country { get; set; }

    public int? State { get; set; }

    public int? City { get; set; }

    public string? Phone { get; set; }
    public string? Imgurl { get; set; }

    public bool Status { get; set; }

    public string? Address { get; set; }

    [Required(ErrorMessage = "ZipCode is required.")]
    [RegularExpression(@"^\d{6}(?:[-\s]\d{5})?$", ErrorMessage = "Invalid ZipCode format.")]
    // [Range(10000, 99999, ErrorMessage = "ZipCode must be a 6-digit number.")]
    public int? ZipCode { get; set; }

    public IFormFile FormFile { get; set; }

}
