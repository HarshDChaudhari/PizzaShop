using Microsoft.EntityFrameworkCore;
using PizzaShop.Entity.Data;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;

namespace PizzaShop.Repository.Implementations;

public class CustomersRepository(PizzaShopDbContext _context) : ICustomersRepository
{
    public List<Customer> GetAllCustomerList()
    {
        return _context.Customers.OrderBy(x => x.CustomerId).ToList();
    }

    public IQueryable<Customer> GetAllCustomerExport()
    {
        try

        {
            var query = _context.Customers
                .Include(x => x.Orders).AsQueryable();
                
            return query;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"DbUpdateException: {ex.Message}\n{ex.InnerException?.Message}");
            // throw;
            return null;
        }
    }

    public List<Order> GetTotalOrderCount(string Email)
    {   
        var query = _context.Orders.Where(o => o.Customer.Email == Email).ToList();
        return query;
    }

    public Customer GetCustomerById(int id)
    {
        return _context.Customers
        .Include(x => x.Orders)
        .ThenInclude(x => x.OrderItems)
        .Include(x => x.Orders)
        .ThenInclude(x => x.Payments)
        .Include(x => x.Orders)
        .FirstOrDefault(x => x.CustomerId == id)!;
    }
        
}

//  public ICollection<Payment> Payments { get; set; } // Added Payments property