using PizzaShop.Entity.Models;

namespace PizzaShop.Repository.Interfaces;

public interface IDashboardRepository
{
    User GetUserById(int userId);
    List<Order> OrderDetailsForDashboard(DateTime fromdate);
    List<Customer?> GetCustomersWithCompletedOrderByDate(DateTime fromdate);
    List<WaitingList> GetWaitingtokensListByDate(DateTime fromdate);
}
