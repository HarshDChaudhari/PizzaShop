using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Filter;
using PizzaShop.Service.Interfaces;
using static PizzaShop.Filter.CustomAuthorize;


namespace PizzaShop.Web.Controllers;
// [CustomAuthorize(UserRoles.Admin, UserRoles.Manager)]
public class OrderAppController(IOrderAppService _orderAppService) : Controller
{
    #region Kot
    // [CustomAuthorize(UserRoles.Chef)]
    [CustomAuthorize(UserRoles.Admin, UserRoles.Manager, UserRoles.Chef)]
    public IActionResult Kot()
    {
        var model = _orderAppService.GetKotOrderDetails("All", "InProgress", 1);
        return View(model);
    }

    public IActionResult LoadKOTPartial(string category, string status, int page)
    {
        var model = _orderAppService.GetKotOrderDetails(category, status, page);
        return PartialView("./PartialView/_KOTCardsPartial", model.Orders);
    }

    public IActionResult GetOrderItemsModal(int orderId, string category, string status)
    {
        var model = _orderAppService.GetOrderForModal(category, orderId, status);
        ViewBag.CurrentStatus = status;
        return PartialView("./PartialView/_KOTModalPartial", model);
    }

    [HttpPost]
    public IActionResult UpdateOrderItems(List<int> selectedItemIds, int orderId, string newStatus, Dictionary<int, int> updatedQuantities)
    {
        var result = _orderAppService.UpdateOrderItem(selectedItemIds, orderId, newStatus, updatedQuantities);
        if (!result.Error)
        {
           
            return Json(new { success = true, message = result.Message });
        }
        else
        {
            
            return Json(new { success = false, message = result.Message });
        }
    }

    #endregion

    #region Menu
    public IActionResult MenuOrderApp(int id)
    {
        var category = _orderAppService.AllCategories();
        ViewBag.Id = id;
        return View(category);
    }

    [HttpGet]
    public IActionResult loadItemsByCategory(int categoryId, string searchQuery)
    {
        var items = _orderAppService.GetMenuDetails(categoryId, searchQuery);
        return PartialView("./PartialView/_ItemCardPartial", items);
    }

    [HttpPost]
    public IActionResult ToggleFavorite(int itemId)
    {
        var toggle = _orderAppService.ToogleFavourite(itemId);
        return Json(new { Success = true , toggle = toggle.IsFavourite });
    }

    [HttpGet]
    public IActionResult GetModifierDetails(int itemId, int orderId = 0)
    {
        @ViewBag.OrderId = orderId;
        var modifier = _orderAppService.GetModifierDetails(itemId);
        return PartialView("./PartialView/_OrderAppMenuModifierPartial", modifier);
    }

    [HttpGet]
    public IActionResult OrderPartial(int orderId)
    {
        var order = new OrderAppOrderDetailsViewModel();
        if (orderId != 0)
        {
            order = _orderAppService.GetOrderDetails(orderId);
        }
        return PartialView("./PartialView/_OrderPartial", order);
    }

    [HttpPost]
    public IActionResult SaveOrder(int orderId, string orderItemsString, float TotalAmount)
    {
        var orderItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderAppOrderItemViewModel>>(orderItemsString);

        var saveOrder = _orderAppService.SaveOrder(orderId, orderItems, TotalAmount);


        return Json(new { Success = true });
    }

    [HttpPost]
    public IActionResult CheckCompleteOrder(int orderId, int customerId)
    {
        var result = _orderAppService.CheckCompleteOrder(orderId);
        if (result == -1)
        {
            return Json(new { Success = false });

        }
        var order = new OrderViewModel();
        order.CustomerId = customerId;
        order.OrderId = orderId;
        return PartialView("./PartialView/_CompleteOrderModal", order);
    }
    [HttpPost]
    public IActionResult CancelOrderModal(int orderId, int customerId)
    {
        var result = _orderAppService.CheckOrder(orderId);
        if (result == -1)
        {
            return Json(new { Success = false });

        }
        var order = new OrderViewModel();
        order.CustomerId = customerId;
        order.OrderId = orderId;
        return PartialView("./PartialView/_CancelOrderPartial", order);
    }

    [HttpPost]
    public IActionResult CompleteOrder(int orderId, int customerId)
    {
        var result = _orderAppService.CompleteOrder(orderId);
        if (result == true)
        {
            var customer = new CustomerReviewViewModel();
            customer.CustomerId = customerId;
            customer.OrderId = orderId;
            return PartialView("./PartialView/_CustomerReviewModal", customer);
        }
        else
        {
            return Json(new { Success = false });
        }
    }

    [HttpPost]
    public IActionResult CancelOrder(int orderId)
    {
        var result = _orderAppService.CancelOrder(orderId);
        return Json(new { Success = result });
    }

    [HttpPost]
    public IActionResult CustomerReview(CustomerReviewViewModel model)
    {
        var result = _orderAppService.CustomerReview(model);
        if (result == true)
        {
            TempData["Success"] = "Review Added Successfully";
            return RedirectToAction("Tables");
        }
        else
        {
            TempData["Error"] = "Failed to Add Review";
            return Json(new { Success = false });
        }
    }

    [HttpPost]
    public IActionResult UpdatePaymentMethod(int orderId, string paymentMethod)
    {
        var result = _orderAppService.UpdatePaymentMethod(orderId, paymentMethod);
        if (result)
        {
            // TempData["Success"] = "Payment Method Updated Successfully";
            return Json(new { Success = true });
        }
        else
        {
            // TempData["Error"] = "Failed to Update Payment Method";
            return Json(new { Success = false });
        }
    }

    [HttpGet]
    public IActionResult GetCustomerDetail(int customerId)
    {
        var customer = _orderAppService.GetCustomerDetail(customerId);
        return PartialView("./PartialView/_CustomerDetailPartial", customer);
    }

    [HttpPost]
    public IActionResult UpdateCustomerDetail(CustomerViewModel model)
    {
        var customer = _orderAppService.UpdateCustomerDetail(model);
        var id = model.Orderid;
        if (customer)
        {
            TempData["Success"] = "Customer Updated Successfully";
            return Json(new { Success = true, id = id });
        }
        else
        {
            return Json(new { Success = false, id = id });
        }
    }

    [HttpGet]
    public IActionResult GetOrderWiseComment(int orderId)
    {
        var order = _orderAppService.GetOrderWiseComment(orderId);
        return PartialView("./PartialView/_OrderWiseCommentPartial", order);
    }
    [HttpPost]
    public IActionResult UpdateOrderWiseComment(OrderViewModel model)
    {
        var order = _orderAppService.UpdateOrderWiseComment(model);
        if (order)
        {
            TempData["Success"] = "Comment Updated Successfully";
            return Json(new { Success = true });
        }
        else
        {
            return Json(new { Success = false });
        }
    }

    [HttpGet]
    public IActionResult GetItemWiseComment(int itemId, int orderId)
    {
        var order = _orderAppService.GetItemWiseComment(itemId, orderId);
        return PartialView("./PartialView/_ItemWiseCommentPartial", order);
    }

    [HttpPost]
    public IActionResult ReduceOrderItemQuantity(int orderDetailId)
    {
        var orderItem = _orderAppService.GetReadyQuantity(orderDetailId);
        return Json(orderItem);
    }

    [HttpPost]
    public IActionResult CheckOrderStatus(int orderId)
    {
        var status = _orderAppService.checkOrderStatus(orderId);
        if (status != null)
        {
            return Json(new { Success = true, status = status });
        }
        else
        {
            return Json(new { Success = false, status = status });
        }
    }

    #endregion
    #region Tables
    public IActionResult Tables()
    {
        var TableList = _orderAppService.GetTableList();
        return View(TableList);
    }

    public IActionResult AddWaitingUser()
    {
        return PartialView("./PartialView/_TableWaitingListModal");
    }

    [HttpGet]
    public IActionResult SectionsList()
    {
        var sectionsList = _orderAppService.GetSections();

        return Json(sectionsList);
    }

    [HttpGet]
    public IActionResult TableList(int sectionId)
    {
        var tableList = _orderAppService.GetTables(sectionId);

        return Json(tableList);
    }
    [HttpGet]
    public IActionResult TableListBySectionId(int sectionId)
    {
        var tableList = _orderAppService.GetTablesBySectionId(sectionId);

        return Json(tableList);
    }

    [HttpPost]
    public IActionResult AddWaitingList(WaitingListViewModel model)
    {
        try
        {
            
            _orderAppService.AddWaitingList(model);
            TempData["Success"] = "Added To Waiting List Successfully";
            return Redirect(Request.Headers["Referer"].ToString());
            
            
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
    [HttpPost]
    public IActionResult AddAssignSidebar(int sectionId, List<int> tableIds)
    {
        var waitingList = _orderAppService.AddAssign(sectionId, tableIds);
        return PartialView("./PartialView/_TableAssignTable", waitingList);
    }



    [HttpPost]
    public IActionResult AddAsCustomer(AddAssignTableViewModel model)
    {
        var customer = _orderAppService.AddAsCustomer(model);
        TempData["Success"] = "Table Assign Successfully";
        return Json(new { Success = true, id = customer });
    }


    #endregion
    #region WaitingList
    public IActionResult WaitingList()
    {
        var SectionList = _orderAppService.GetSectionsForWaitingList();

        return View(SectionList);
    }


    public IActionResult GetWaitingList(int sectionId)
    {
        var waitingList = _orderAppService.GetWaitingList(sectionId);
        return PartialView("./PartialView/_WaitingListPartial", waitingList);
    }

    public IActionResult AddWaitingUserWaitingList()
    {
        return PartialView("./PartialView/_WaitingListAddWaitingModal");
    }

    public IActionResult LoadWaitingTokenDetails(int waitingId)
    {

        var waitingList = _orderAppService.GetWaitingListByWaitingId(waitingId);

        return PartialView("./PartialView/_WaitingListEditWaitingModal", waitingList);
    }

    public IActionResult UpdateWaitingList(WaitingListViewModel model)
    {
        _orderAppService.UpdateWaitingList(model);

        TempData["Success"] = "Updated Successfully";
        return Json(new { Success = true });
    }

    [HttpPost]
    public IActionResult AddAssignWaitingList(int waitingId, int sectionId, List<int> tableIds)
    {
        var Id = _orderAppService.AddAsCustomerWaitingList(waitingId, sectionId, tableIds);
        TempData["Success"] = "Table Assign Successfully";
        return Json(new { Success = true, id = Id });
    }
    public IActionResult DeleteWaiting(int waitingId)
    {
        _orderAppService.DeleteWaiting(waitingId);
        TempData["Success"] = "Deleted Successfully";
        return Json(new { Success = true });
    }


    public IActionResult CheckExistingUser(string email){
        var user = _orderAppService.CheckExistingUser(email);
        return Json(user);
    }


    #endregion

}
