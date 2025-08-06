namespace PizzaShop.Entity.ViewModel;

public class OrderItemFlat
{
    public int order_id { get; set; }
    public DateTime created_at { get; set; }
    public string? item_name { get; set; }
    public int quantity { get; set; }
    public int ready_quantity { get; set; }
    public string? status { get; set; }
    public int order_item_id { get; set; }
    public string? modifier_name { get; set; }
}
