using PizzaShop.Entity.Models;

namespace PizzaShop.Repository.Interfaces;

public interface ICustomersRepository
{
    List<Customer> GetAllCustomerList();
     IQueryable<Customer> GetAllCustomerExport();
     List<Order> GetTotalOrderCount(string Email);

      Customer GetCustomerById(int id);
}
