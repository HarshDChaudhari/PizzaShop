namespace PizzaShop.Entity.ViewModel;

public class DashboardVM
{
    public decimal TotalSales { get; set; }
    public int TotalOrders { get; set; }
    public decimal AverageOrderValue { get; set; }
    public double AverageWaitingTime { get; set; }
    public int WaitingListCount { get; set; }
    public int NewCustomerCount { get; set; }
    public List<ItemData>? MostSellingItems { get; set; }
    public List<ItemData>? LeastSellingItems { get; set; }
}

public class ItemData 
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int TotalQuantitySold { get; set; }
    public string? Image { get; set; }
}

public class GraphDataVM
{
    public decimal MaxRevenue { get; set; }
    public int MaxCustomerGrowth { get; set; }
    public List<string>? Labels { get; set; }
    public List<decimal>? RevenueData { get; set; }
    public List<int>? CustomerGrowthData { get; set; }
}

