using Microsoft.EntityFrameworkCore;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Service.Implementations;

public class CustomersService(ICustomersRepository _customersRepository) : ICustomersService
{
    public async Task<PaginationViewModel<CustomerViewModel>> GetCustomerDetail(int page, int pageSize, string search = "", string customerTime = "", string sortColumn = "", string sortOrder = "asc")
    {

        List<Customer> customers = _customersRepository.GetAllCustomerList();
        List<CustomerViewModel> customerListViews = new List<CustomerViewModel>();
        foreach (Customer item in customers)
        {
            var totalOrder = item.Email != null 
            ? _customersRepository.GetTotalOrderCount(item.Email).Count 
            : 0;

            customerListViews.Add(new CustomerViewModel
            {
            CustomerId = item.CustomerId,
            CustomerName = item.CustomerName,
            Email = item.Email,
            Phone = item.Phone,
            Date = item.CreatedAt,
            TotalOrder = totalOrder
            });
        }
        if (!string.IsNullOrEmpty(search))
        {
            customerListViews = customerListViews.Where(u => u.CustomerName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }



        if (!string.IsNullOrEmpty(customerTime) && customerTime != "all")
        {
            DateTime now = DateTime.Now;

            if (customerTime == "7")
            {
                var last7Days = now.AddDays(-7);
                customerListViews = customerListViews.Where(o => o.Date >= last7Days).ToList();
            }
            else if (customerTime == "30")
            {
                var last30Days = now.AddDays(-30);
                customerListViews = customerListViews.Where(o => o.Date >= last30Days).ToList();
            }
            else if (customerTime == "month")
            {
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                customerListViews = customerListViews.Where(o => o.Date >= startOfMonth).ToList();
            }
        }

        if (!string.IsNullOrEmpty(sortColumn))
        {
            if (sortColumn.Equals("CustomerName", StringComparison.OrdinalIgnoreCase))
            {
                if (sortOrder == "asc")
                    customerListViews = customerListViews.OrderBy(u => u.CustomerName).ToList();
                else
                    customerListViews = customerListViews.OrderByDescending(u => u.CustomerName).ToList();
            }
            else if (sortColumn.Equals("Date", StringComparison.OrdinalIgnoreCase))
            {
                if (sortOrder == "asc")
                    customerListViews = customerListViews.OrderBy(u => u.Date).ToList();
                else
                    customerListViews = customerListViews.OrderByDescending(u => u.Date).ToList();
            }
            
        }


        var tableCount = customerListViews.Count;
        customerListViews = customerListViews.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PaginationViewModel<CustomerViewModel>
        {
            Items = customerListViews,
            TotalItems = tableCount,
            CurrentPage = page,
            PageSize = pageSize,
            sortColumn = sortColumn,
            sortOrder = sortOrder,
        };
    }


    public ExportOrderResultViewModel GetExportCustomer(string search, string time)
    {
        var query = _customersRepository.GetAllCustomerExport();

        

        if (!string.IsNullOrEmpty(time) && time != "all")
        {
            DateTime now = DateTime.Now;

            if (time == "7")
            {
                var last7Days = now.AddDays(-7);
                query = query.Where(o => o.CreatedAt >= last7Days);
            }
            else if (time == "30")
            {
                var last30Days = now.AddDays(-30);
                query = query.Where(o => o.CreatedAt >= last30Days);
            }
            else if (time == "month")
            {
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                query = query.Where(o => o.CreatedAt >= startOfMonth);
            }
        }

        // Search filter
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(o =>
                o.CustomerName.Contains(search));

        }

        var customerData = query.Select(o => new CustomerViewModel
        {
            CustomerId = o.CustomerId,
            Date = o.CreatedAt,
            CustomerName = o.CustomerName,
            Email = o.Email,
            Phone = o.Phone,
            TotalOrder = o.Orders != null ? o.Orders.Count : 0
        }).ToList();

        return new ExportOrderResultViewModel
        {

            search = search,
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            record = customerData.Count.ToString(),
            CustomerData = customerData,
            
        };
    }

    public CustomerHistoryViewModel GetCustomerHistory(int id)
    {
        var customer = _customersRepository.GetCustomerById(id);

        var customerHistory = new CustomerHistoryViewModel
        {  
            Name = customer.CustomerName,
            PhoneNo = customer.Phone,
            CreatedOn = customer.CreatedAt,
            Visits = customer.Orders.Count(),
            MaxOrderBill = customer.Orders.Max(o => o.TotalAmount) ?? 0f,
            Avgbill = customer.Orders.Average(o => o.TotalAmount) ?? 0f,
            Email = customer.Email,

            OrderHistory = customer.Orders.Select(o => new OrderHistory
            {
                OrderDate = o.CreatedAt ?? DateTime.MinValue,
                OrderType = o.OrderType,
                PaymentMethod = o.Payments.FirstOrDefault()?.PaymentMethod,
                Items = o.OrderItems.Count,
                TotalAmount = o.TotalAmount ?? 0f
            }).ToList()

        };


        return customerHistory; 
    }
}
