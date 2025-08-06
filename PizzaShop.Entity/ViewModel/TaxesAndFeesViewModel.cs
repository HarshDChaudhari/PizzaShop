namespace PizzaShop.Entity.ViewModel;

public class TaxesAndFeesViewModel
{
    public int TaxId { get; set; }

    public string TaxName { get; set; } = null!;

    public string? TaxType { get; set; }

    public decimal? TaxValue { get; set; }

    public bool IsEnabled { get; set; }
    public bool IsDefault { get; set; }

    public bool? IsDeleted { get; set; }

    public List<TaxesAndFeesViewModel> TaxFeesList { get; set; } = new List<TaxesAndFeesViewModel>();

}
