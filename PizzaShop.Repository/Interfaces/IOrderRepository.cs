using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;

namespace PizzaShop.Repository.Interfaces;

public interface IOrderRepository
{
    List<Order> GetAllOrderList();
    OrderSummaryViewModel GetOrderDetails(long id);

    IQueryable<Order> GetAllOrderExport();
}
