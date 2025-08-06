
namespace PizzaShop.Entity.ViewModel;
public class KOTMainViewModel
{
     public List<string> Categories { get; set; }
    public string SelectedCategory { get; set; }
    public string SelectedStatus { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPage { get; set; }
    public List<KOTOrderCardViewModel> Orders { get; set; } 
    
}
public class KOTOrderItemModifierViewModel
{
    public string ModifierName { get; set; }
}

public class KOTOrderItemViewModel
{
    public int OrderItemId { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public int ReadyQuantity { get; set; }
    public string Instruction { get; set; }
    public string Status { get; set; }
    public List<KOTOrderItemModifierViewModel> Modifiers { get; set; }
}

public class KOTOrderCardViewModel
{
    public int OrderId { get; set; }
    public List<string> TableName { get; set; }
    public string SectionName { get; set; }
    public string OrderInstruction { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<KOTOrderItemViewModel> Items { get; set; }
}
