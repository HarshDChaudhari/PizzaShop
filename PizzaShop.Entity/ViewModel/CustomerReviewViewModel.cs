namespace PizzaShop.Entity.ViewModel;

public class CustomerReviewViewModel
{
    public int ReviewId { get; set; }
    public int CustomerId { get; set; }
    public int OrderId { get; set; }
    public int? FoodRating { get; set; }

    public int? ServiceRating { get; set; }

    public int? AmbienceRating { get; set; }
    public string? CustomerName { get; set; }
    public string? ReviewText { get; set; }
    public int Rating { get; set; }
    public DateTime ReviewDate { get; set; }
    public string? OrderStatus { get; set; }
    public string? OrderComment { get; set; }
    public string? PaymentMethod { get; set; }
    public float TotalAmount { get; set; }
}
