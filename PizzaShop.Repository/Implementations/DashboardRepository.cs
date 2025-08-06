using Microsoft.EntityFrameworkCore;
using PizzaShop.Entity.Data;
using PizzaShop.Entity.Models;
using PizzaShop.Repository.Interfaces;

namespace PizzaShop.Repository.Implementations;

public class DashboardRepository(PizzaShopDbContext _context) :IDashboardRepository
{

    public User GetUserById(int userId)
    {
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return null;
            }
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    public List<Order> OrderDetailsForDashboard(DateTime fromdate)
    {
        try
        {
            List<Order> orders = _context.Orders.Where(o=>o.CreatedAt >= fromdate)
                                    .Include(o=>o.OrderItems).ThenInclude(oi=>oi.CategoryItem).ToList();

            if(orders == null)
            {
                return null;
            }

            return orders;
        }
        catch(Exception ex)
        {
            return new List<Order>();
        }
    }

     public List<WaitingList> GetWaitingtokensListByDate(DateTime fromdate)
    {
        try
        {
            List<WaitingList> waitingtokens = _context.WaitingLists.Where(wt=>wt.CreatedAt >= fromdate).ToList();
 
            if(waitingtokens == null)
            {
                return null;
            }
            return waitingtokens;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<WaitingList>();
        }
    }

    public List<Customer?> GetCustomersWithCompletedOrderByDate(DateTime fromdate)
    {
        try
        {
            List<Customer?> customers = _context.Orders.Where(o=>o.CreatedAt >= fromdate && o.Status == "Completed")
                                        .Select(o=>o.Customer).Distinct().ToList()!;
 
            if(customers == null)
            {
                return null;
            }
            return customers;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<Customer>()!;
        }
    }
}
