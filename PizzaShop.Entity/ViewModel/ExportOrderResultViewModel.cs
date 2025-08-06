namespace PizzaShop.Entity.ViewModel;

public class ExportOrderResultViewModel
{

    public string status { get; set; }
    public string search { get; set; }
    public string Date { get; set; }
    public string record { get; set; }
    public List<OrderViewModel> orderData { get; set; }
    public List<CustomerViewModel> CustomerData { get; set; }
}

