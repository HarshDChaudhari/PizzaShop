namespace PizzaShop.Entity.ViewModel;

public class OrderAppOrderDetailsViewModel
{        public int OrderId { get; set; }
 
        public int InvoiceId { get; set; }
     
 
        public string? CustomerPhone { get; set; }
 
        public int TotalPerson { get; set; }
        public int CustomerId { get; set; }
 
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
 
        public string section { get; set; }
        public float TotalTax { get; set; }
 
        public string OrderStatus { get; set; }
 
        public string OrderComment { get; set; }
 
        public string OrderItemQuntity { get; set; }
        public string TableName { get; set; }
        public string SectionName { get; set; }
        public string PaymentMode { get; set; }
 
        public float OrderItemPrice { get; set; }
        public float Subtotal { get; set; }
        public float OrderAmount { get; set; }
        public float Other { get; set; }
 
        public float OrderItemAmount { get; set; }
 
        public DateTime StartDate { get; set; }
 
        public DateTime EndDate { get; set; }
 
        public List<string> TableList = new List<string>();
 
        // public AddTableViewModel table = new AddTableViewModel();
        public List<TableViewModel> ManyTableList = new List<TableViewModel>();
        public List<TaxesAndFeesViewModel> tax = new List<TaxesAndFeesViewModel>();
        //  public List<AddItemViewModel> OrderItem = new List<AddItemViewModel>();
        public List<OrderManyItem> OrderItem = new List<OrderManyItem>();



    // public int PaymentId { get; set; }
    // public string PaymentMethod { get; set; }

    // public int? Rating { get; set; }

    // public float? TotalAmount { get; set; }

    // public string? OrderStatus { get; set; }

    // public DateTime OrderDate { get; set; }

    // public string Date { get; set; }



}
 
public class UpdateOrder{
        public int detailsid{get; set;}
 
        public int  quantity{get; set;}
}
 
 
public class OrderManyItem
{
        public float TaxPercentage { get; set; }
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public string ItemName { get; set; }
 
        public int categoryid{get; set;}
        public int Quantity { get; set; }
        public int Prepared { get; set; }
        public float Price { get; set; }
 
        public string ItemStatus{get; set;}
 
        public string ItemByComment { get; set; }
        public List<ModifierOrderAppViewModel> modifier = new List<ModifierOrderAppViewModel>();
}

 
public class ModifierOrderAppViewModel
{
        public string ModifierName { get; set; }
        public int? ModifierId { get; set; }
        public int? Quantity { get; set; }
        public float ModifierPrice { get; set; }
 
}