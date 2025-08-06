

namespace PizzaShop.Entity.ViewModel;

public class SectionsViewModel
{
    public int SectionId { get; set; }

    public string SectionName { get; set; } = null!;

    public string? Description { get; set; }

    

    public List<SectionsViewModel> SectionList { get; set; }
    public List<TableViewModel> TableDetails { get; set; }



}
