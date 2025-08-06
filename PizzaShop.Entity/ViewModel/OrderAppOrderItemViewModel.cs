namespace PizzaShop.Entity.ViewModel;

public class OrderAppOrderItemViewModel
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public List<OrderAppModifierViewModel> Modifiers { get; set; } = new List<OrderAppModifierViewModel>();
    public int Quantity { get; set; } = 1;
    public float TotalPrice => Price * Quantity;
}

public class OrderAppModifierViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public bool IsSelected { get; set; } = false;

}

// ItemId: currentItemId,
//             name: itemName,
//             price: itemPrice,
//             modifiers: currentModifiers,
//             quantity: 1

// id: itemId,
//             name: $(element).find('span:first').text(),
//             price:
