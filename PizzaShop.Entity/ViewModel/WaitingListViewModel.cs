using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Entity.ViewModel;

public class WaitingListViewModel
{

    [Required]
    public int WaitingId {get; set;}
    [Required(ErrorMessage = "Email is required.")]
    public string? Email {get; set;}
    [Required (ErrorMessage = "UserName is required.")]
    public string? UserName {get; set;}
    [Required(ErrorMessage = "Phone Number is required.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Phone Number format.")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber {get; set;}
    [Required(ErrorMessage = "No of Person is required.")]
    [Range(1, 20, ErrorMessage = "No of Person must be between 1 and 20.")]
    [RegularExpression(@"^\d{1,2}$", ErrorMessage = "Invalid No of Person format.")]
    public int? NoOfPerson {get; set;}
    public string? SectionName {get; set;}
    [Required(ErrorMessage = "Section is required.")]
    public int? SectionId {get; set;}

    public DateTime CreatedAt {get; set;}
    public DateTime ModifiedAt {get; set;}

    
}
