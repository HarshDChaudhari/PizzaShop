namespace PizzaShop.Entity.ViewModel;

public class OrderAppWaitingListViewModel
{
    public List<SectionsViewModel> SectionList { get; set; } = new List<SectionsViewModel>();
    public List<WaitingListViewModel> WaitingList { get; set; } = new List<WaitingListViewModel>();

}
