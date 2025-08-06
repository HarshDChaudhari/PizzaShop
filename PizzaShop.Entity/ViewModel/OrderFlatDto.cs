namespace PizzaShop.Entity.ViewModel;

public class OrderFlatDto
{
    public int OrderId { get; set; }
    public string? OrderStatus { get; set; }
    public string? OrderInstruction { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int TableId { get; set; }
    public string? TableName { get; set; }
    public int SectionId { get; set; }
    public string? SectionName { get; set; }
    public int OrderItemId { get; set; }
    public int Quantity { get; set; }
    public int ReadyQuantity { get; set; }
    public string? OrderItemInstruction { get; set; }
    public string? ItemName { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int? OrderItemModifierId { get; set; }
    public string? ModifierItemName { get; set; }
}

