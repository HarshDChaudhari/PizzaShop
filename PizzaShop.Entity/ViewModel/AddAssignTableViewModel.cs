namespace PizzaShop.Entity.ViewModel;

public class AddAssignTableViewModel
{
    public string? UserName { get; set; }

    public string? Email { get; set; }

    public int? TotalPerson { get; set; }

    public string? Phone { get; set; }

    public int WaitingId { get; set; }

    public int? SectionId { get; set; }

    public List<int>? SelectedTable {get; set;} 

    public List<WaitingListViewModel>? WaitingUserList  { get; set; } = new List<WaitingListViewModel>();

    
}
