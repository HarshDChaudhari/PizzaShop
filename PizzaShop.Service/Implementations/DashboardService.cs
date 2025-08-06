using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Service.Implementations;

public class DashboardService(IDashboardRepository _dashboardRepository) : IDashboardService
{

    public User GetUserProfile(int userId)
    {
        try
        {
            var user = _dashboardRepository.GetUserById(userId);
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public DashboardVM GetDashboard(int dateRange)
    {
        try
        {
            DateTime fromDate = dateRange switch
            {
                0 => DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-7), DateTimeKind.Unspecified),
                1 => DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-30), DateTimeKind.Unspecified),
                2 => DateTime.SpecifyKind(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1), DateTimeKind.Unspecified),
                3 => DateTime.SpecifyKind(new DateTime(DateTime.UtcNow.Year, 1, 1), DateTimeKind.Unspecified),
                _ => DateTime.MinValue
            };
            List<Order> orders = _dashboardRepository.OrderDetailsForDashboard(fromDate);
            List<WaitingList> waitingtokens = _dashboardRepository.GetWaitingtokensListByDate(fromDate);

            List<Customer?> customers = _dashboardRepository.GetCustomersWithCompletedOrderByDate(fromDate);

            var avgTime = waitingtokens.Where(w => w.IsDeleted == true).Select(w => (w.ModifiedAt!.Value - w.CreatedAt!.Value).Ticks).DefaultIfEmpty(0).Average();
            var AverageWaitingTime = new TimeSpan(Convert.ToInt64(avgTime));




            var orderItem = orders.SelectMany(o => o.OrderItems).Where(o => o.CategoryItem != null).ToList();

            var allItems = orderItem.GroupBy(oi => oi.CategoryItemId).Select(i => new ItemData
            {
                Id = i.FirstOrDefault()!.CategoryItem!.CategoryItemId,
                Name = i.FirstOrDefault()!.CategoryItem?.ItemName,
                Image = i.FirstOrDefault()!.CategoryItem?.ImageUrl,
                TotalQuantitySold = (int)i.Sum(q => q.Quantity)
            }).OrderByDescending(i => i.TotalQuantitySold).ToList();

            var topSelling = allItems.Take(5).ToList();

            var leastSelling = allItems.OrderBy(i => i.TotalQuantitySold).Where(i => !topSelling.Any(ts => ts.Id == i.Id)).Take(5).ToList();


            return new DashboardVM
            {
                TotalSales = (decimal)orders.Sum(o => o.TotalAmount)!.Value,
                TotalOrders = orders.Count,
                AverageOrderValue = (decimal)(orders.Any() ? orders.Average(o => o.TotalAmount)!.Value : 0),
                AverageWaitingTime = AverageWaitingTime.TotalMinutes,
                NewCustomerCount = customers.Count,
                WaitingListCount = waitingtokens.Count(w => !w.IsDeleted),
                MostSellingItems = topSelling,
                LeastSellingItems = leastSelling
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new DashboardVM();
        }
    }

    public GraphDataVM GetDashboardGraph(int dateRange)
    {
        try
        {
            DateTime startDate, endDate;
            List<string> labels = new();
            List<decimal> revenueData = new();
            List<int> customerGrowthData = new();

            static DateTime AsUnspecified(DateTime dt) => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

            switch (dateRange)
            {
                case 0:
                    startDate = AsUnspecified(DateTime.UtcNow.AddDays(-7));
                    endDate = AsUnspecified(DateTime.UtcNow);
                    labels = Enumerable.Range(0, 7)
                        .Select(i => DateTime.UtcNow.AddDays(-6 + i).ToString("dd"))
                        .ToList();
                    break;

                case 1:
                    startDate = AsUnspecified(DateTime.UtcNow.AddDays(-30));
                    endDate = AsUnspecified(DateTime.UtcNow);
                    labels = Enumerable.Range(0, 30)
                        .Select(i => DateTime.UtcNow.AddDays(-29 + i).ToString("dd"))
                        .ToList();
                    break;

                case 2:
                    startDate = AsUnspecified(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1));
                    endDate = AsUnspecified(startDate.AddMonths(1).AddDays(-1));
                    labels = Enumerable.Range(1, DateTime.DaysInMonth(startDate.Year, startDate.Month))
                        .Select(d => d.ToString("00"))
                        .ToList();
                    break;

                case 3:
                    startDate = AsUnspecified(new DateTime(DateTime.UtcNow.Year, 1, 1));
                    endDate = AsUnspecified(new DateTime(DateTime.UtcNow.Year, 12, 31));
                    labels = Enumerable.Range(1, 12)
                        .Select(m => new DateTime(startDate.Year, m, 1).ToString("MMM"))
                        .ToList();
                    break;

                case 4:
                default:
                    startDate = AsUnspecified(new DateTime(2024, 1, 1));
                    endDate = AsUnspecified(DateTime.UtcNow);
                    int startYear = 2024;
                    int currentYear = DateTime.UtcNow.Year;
                    labels = Enumerable.Range(startYear, currentYear - startYear + 1)
                        .Select(y => y.ToString())
                        .ToList();
                    break;
            }

            List<Order> completedOrders = _dashboardRepository.OrderDetailsForDashboard(startDate);
            List<Customer?> customersWithCompletedOrder = _dashboardRepository.GetCustomersWithCompletedOrderByDate(startDate);

            if (dateRange == 3)
            {
                customerGrowthData = labels.Select(label =>
                     customersWithCompletedOrder.Count(c => c.CreatedAt?.ToString("MMM") == label && c.CreatedAt?.Year == DateTime.Now.Year)).ToList();

                revenueData = labels.Select(label =>
                    completedOrders.Where(o => o.CreatedAt?.ToString("MMM") == label && o.CreatedAt?.Year == DateTime.Now.Year).Sum(o => (decimal?)o.TotalAmount ?? 0)).ToList();
            }
            else if (dateRange == 4)
            {


                customerGrowthData = labels.Select(label =>
               customersWithCompletedOrder.Count(c => c.CreatedAt?.Year.ToString() == label)).ToList();

                revenueData = labels.Select(label =>
                    completedOrders.Where(o => o.CreatedAt?.Year.ToString() == label).Sum(o => (decimal?)o.TotalAmount ?? 0)).ToList();
            }
            else
            {
                customerGrowthData = labels.Select(label =>
                    customersWithCompletedOrder.Count(c => c.CreatedAt?.ToString("dd") == label)).ToList();

                revenueData = labels.Select(label =>
                        completedOrders.Where(o => o.CreatedAt?.ToString("dd") == label).Sum(o => (decimal?)o.TotalAmount ?? 0)).ToList();

            }

            return new GraphDataVM
            {
                Labels = labels,
                RevenueData = revenueData,
                CustomerGrowthData = customerGrowthData,
                MaxRevenue = revenueData.Any() ? Math.Ceiling(revenueData.Max() / 500) * 500 : 0,
                MaxCustomerGrowth = customerGrowthData.Any() ? ((customerGrowthData.Max() + 9) / 10) * 10 : 0
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new GraphDataVM();
        }
    }
}

