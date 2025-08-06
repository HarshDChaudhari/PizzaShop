namespace PizzaShop.Entity.ViewModel;

public class CustomerViewModel
{
    public int CustomerId { get; set; }
    public int Orderid { get; set; }
    public int TotalPerson { get; set; }
    public string CustomerName { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int? TotalOrder { get; set; }

    public DateTime? Date { get; set; }


}
