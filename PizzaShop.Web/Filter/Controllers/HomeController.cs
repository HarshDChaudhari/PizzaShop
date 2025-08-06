using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using PizzaShop.Service.Interfaces;
using PizzaShop.Entity.ViewModel;

namespace PizzaShop.Web.Controllers;
// [ServiceFilter(typeof(PermissionFilter))]
public class HomeController(ILogger<HomeController> _logger , IDashboardService _dashboardService, IJwtService _jwtService) : Controller
{

    public IActionResult Index()
    {
        TempData["Active"] = "Dashboard";
         var principal = _jwtService.ValidateToken(Request.Cookies["SuperSecretAuthToken"]);
            if (principal == null)
            {
                return RedirectToAction("Login", "Validation");
            }
            int Id = int.Parse(principal.FindFirst("RoleId")?.Value ?? "0");

        if (Convert.ToInt64(Id) == 3)
        {
            return RedirectToAction("Kot", "OrderApp");
        }
        @ViewBag.RoleId = Id;

        return View();
    }

    [HttpGet]
    public IActionResult UserProfile()
    {
        var principal = _jwtService.ValidateToken(Request.Cookies["SuperSecretAuthToken"]);
            if (principal == null)
            {
                return RedirectToAction("Login", "Validation");
            }
            int UserId = int.Parse(principal.FindFirst("UserId")?.Value ?? "0");
        var model = _dashboardService.GetUserProfile(UserId);
        return Json(model);
    }

    [HttpGet]
    public IActionResult GetDashboard(int dateRange)
    {
        var model = _dashboardService.GetDashboard(dateRange);
         var principal = _jwtService.ValidateToken(Request.Cookies["SuperSecretAuthToken"]);
            if (principal == null)
            {
                return RedirectToAction("Login", "Validation");
            }
            string Id = principal.FindFirst("RoleId")?.Value ?? "0";

            ViewBag.RoleId = Id;
        return PartialView("./PartialView/_DashboardPartial", model);
    }

    [HttpGet]
    public IActionResult GetDashboardGraph(int dateRange)
    {
        var model = _dashboardService.GetDashboardGraph(dateRange);
        return Json(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string errorCode)
    {
        var viewModel = new ErrorViewModel();
 
        int.TryParse(errorCode, out int code);
 
        switch (code)
        {
            case 403:
                viewModel.HtmlTitleTag = "403";
                return View("Error", viewModel);
            case 404:
                viewModel.HtmlTitleTag = "404";
                return View("Error", viewModel);
            case 500:
                viewModel.HtmlTitleTag = "500";
                return View("Error", viewModel);
            default:
                viewModel.HtmlTitleTag = "Unknown Error";
                return View("Error", viewModel);
        }
    }
}