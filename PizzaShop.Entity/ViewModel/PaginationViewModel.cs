namespace PizzaShop.Entity.ViewModel;

public class PaginationViewModel<T>
{
    public List<T> Items {get; set; } = new List<T>();
    public int TotalItems {get; set; }
    public int CurrentPage {get; set; }
    public int TotalPages {get; set; }
    public int PageSize {get; set; }
    public string sortOrder {get; set; }
    public string sortColumn {get; set; }

    public string? search {get; set;}

    public bool HasPreviousPage => CurrentPage > 1 ;
    public bool HasNextPage => CurrentPage < TotalPages ;

}
