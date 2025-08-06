namespace PizzaShop.Entity.ViewModel;

public class OrderSummaryViewModel
{
    public long OrderId { get; set; }
    public long CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PaymentMethod { get; set; } = null!;
    public long OrderNo { get; set; }
    public string Phone { get; set; }
    public string? OrderStatus { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? Tax { get; set; }
    public long? NoOfPerson { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public TableAndSectionViewModel table = new TableAndSectionViewModel();
    public List<TaxesAndFeesViewModel> tax = new List<TaxesAndFeesViewModel>();
    public List<ItemsViewModel> items = new List<ItemsViewModel>();
}


public class TableAndSectionViewModel
{
    public string TableName { get; set; } = null!;
    public string SectionName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int NoOfPerson { get; set; }
    public int Capacity { get; set; }
}



public class ItemsViewModel
{
    public string ItemName { get; set; } = null!;
    public string? ItemType { get; set; }
    public int? Quantity { get; set; }
    public float? Rate { get; set; }
    public int? TaxPercentage { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<ModifierViewModel> modifier = new List<ModifierViewModel>();
}
