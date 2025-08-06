namespace PizzaShop.Entity.ViewModel;

public class CustomerHistoryViewModel
{
    public string Name { get; set; }
    public string PhoneNo { get; set; }
    public string Email { get; set; }
    public float MaxOrderBill { get; set; }
    public float Avgbill { get; set; }
    public DateTime? CreatedOn { get; set; }
    public int Visits { get; set; }

    public List<OrderHistory> OrderHistory { get; set; } 

}

public class OrderHistory
{
    public DateTime OrderDate { get; set; }
    public string OrderType { get; set; } 
    public int Items { get; set; } 

    public string PaymentMethod { get; set; }

    public float TotalAmount { get; set; }

}