using Microsoft.AspNetCore.Mvc;

namespace PizzaShop.Web.Controllers;

public class EventManagementController : Controller
{
    public IActionResult Index()
    {
        TempData["Active"] = "EventManagement";
        return View();
    }
    public IActionResult AddEvent()
    {
        TempData["Active"] = "EventManagement";
        return View();
    }
}
