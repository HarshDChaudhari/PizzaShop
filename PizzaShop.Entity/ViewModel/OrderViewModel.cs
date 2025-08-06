namespace PizzaShop.Entity.ViewModel;

public class OrderViewModel
{
     public int OrderId { get; set; }
     public int CustomerId { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? CustomerName { get; set; }
    public string? OrderStatus { get; set; }
    public string? OrderComment { get; set; }
    public string? PaymentMethod { get; set; }
    public float? TotalAmount { get; set; }
    public int? Rating { get; set; }
}


// public class OrderListViewModel
// {
//     public List<OrderViewModel> Orders { get; set; }
//     public int PageNumber { get; set; }
//     public int PageSize { get; set; }
//     public int TotalRecords { get; set; }
// }
