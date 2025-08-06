using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Web.Controllers;
[ServiceFilter(typeof(PermissionFilter))]
public class OrderController(IOrderService _orderService) : Controller
{
    [HttpGet]
    public IActionResult Orders()
    {
        TempData["Active"] = "Orders";
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetOrderDetails( int page, int pageSize, string search, string orderStatus, string orderTime,DateTime? fromDate, DateTime? toDate, string sortOrderColumn = "", string sortOrderOrder = "asc")
    {
        try
        {

            PaginationViewModel<OrderViewModel> table = await _orderService.GetOrderDetail(page, pageSize, search, orderStatus, orderTime, fromDate, toDate,sortOrderColumn,sortOrderOrder);
             return PartialView("./PartialView/_OrderPartial", table);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpGet]
     public IActionResult ExportToExcel(string search, string time, DateTime fromDate, DateTime toDate, string status = "")
    {
        var ordersdata = _orderService.GetExportOrders(search: search, status: status, time: time, fromDate: fromDate, toDate: toDate);

        Guid name = Guid.NewGuid();
        var path = Path.Combine("D:\\" + name + ".xlsx");



        using var wb = new XLWorkbook();

        var ws = wb.AddWorksheet();
        ws.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        ws.Range("A2", "B3").Merge().Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Cell("A2").Value = "Status";
        ws.Cell("A2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        .Border.SetTopBorder(XLBorderStyleValues.Thin)
        .Border.SetRightBorder(XLBorderStyleValues.Thin)
        .Border.SetBottomBorder(XLBorderStyleValues.Thin)
        .Border.SetLeftBorder(XLBorderStyleValues.Thin);
        ws.Range("C2", "F3").Merge();
        ws.Cell("C2").Value = ordersdata.status;
        ws.Cell("C2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        .Border.SetTopBorder(XLBorderStyleValues.Thin)
        .Border.SetRightBorder(XLBorderStyleValues.Thin)
        .Border.SetBottomBorder(XLBorderStyleValues.Thin)
        .Border.SetLeftBorder(XLBorderStyleValues.Thin);

        ws.Range("H2", "I3").Merge().Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Cell("H2").Value = "Search Text";
        ws.Cell("H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        .Border.SetTopBorder(XLBorderStyleValues.Thin)
        .Border.SetRightBorder(XLBorderStyleValues.Thin)
        .Border.SetBottomBorder(XLBorderStyleValues.Thin)
        .Border.SetLeftBorder(XLBorderStyleValues.Thin);
        ws.Range("J2", "M3").Merge();
        ws.Cell("J2").Value = ordersdata.search;
        ws.Cell("J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        .Border.SetTopBorder(XLBorderStyleValues.Thin)
        .Border.SetRightBorder(XLBorderStyleValues.Thin)
        .Border.SetBottomBorder(XLBorderStyleValues.Thin)
        .Border.SetLeftBorder(XLBorderStyleValues.Thin);

        ws.Range("A5", "B6").Merge().Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Cell("A5").Value = "DATE";
        ws.Cell("A5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        .Border.SetTopBorder(XLBorderStyleValues.Thin)
        .Border.SetRightBorder(XLBorderStyleValues.Thin)
        .Border.SetBottomBorder(XLBorderStyleValues.Thin)
        .Border.SetLeftBorder(XLBorderStyleValues.Thin);
        ws.Range("C5", "F6").Merge();
        ws.Cell("C5").Value = ordersdata.Date;
        ws.Cell("C5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        .Border.SetTopBorder(XLBorderStyleValues.Thin)
        .Border.SetRightBorder(XLBorderStyleValues.Thin)
        .Border.SetBottomBorder(XLBorderStyleValues.Thin)
        .Border.SetLeftBorder(XLBorderStyleValues.Thin);

        ws.Range("H5", "I6").Merge().Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Cell("H5").Value = "No of Record";
        ws.Cell("H5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        .Border.SetTopBorder(XLBorderStyleValues.Thin)
        .Border.SetRightBorder(XLBorderStyleValues.Thin)
        .Border.SetBottomBorder(XLBorderStyleValues.Thin)
        .Border.SetLeftBorder(XLBorderStyleValues.Thin);
        ws.Range("J5", "M6").Merge();
        ws.Cell("J5").Value = ordersdata.record;
        ws.Cell("J5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        .Border.SetTopBorder(XLBorderStyleValues.Thin)
        .Border.SetRightBorder(XLBorderStyleValues.Thin)
        .Border.SetBottomBorder(XLBorderStyleValues.Thin)
        .Border.SetLeftBorder(XLBorderStyleValues.Thin);

        var img = "D:\\DotNet N Layered\\PizzaShop\\PizzaShop.Web\\wwwroot\\images\\logos\\pizzashop_logo.png";
        ws.Range("O2", "P6").Merge();
        ws.AddPicture(img).MoveTo(ws.Cell("O2")).Scale(.3);


        ws.Cell("A9").Value = "Id";
        ws.Cell("A9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Range("B9", "D9").Merge();
        ws.Cell("B9").Value = "Date";
        ws.Cell("B9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Range("E9", "G9").Merge();
        ws.Cell("E9").Value = "Customer";
        ws.Cell("E9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Range("H9", "J9").Merge();
        ws.Cell("H9").Value = "Status";
        ws.Cell("H9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Range("K9", "L9").Merge();
        ws.Cell("K9").Value = "Payment Mode";
        ws.Cell("K9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Range("M9", "N9").Merge();
        ws.Cell("M9").Value = "Ratings";
        ws.Cell("M9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Range("O9", "P9").Merge();
        ws.Cell("O9").Value = "Total Amount";
        ws.Cell("O9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));

        for (var j = 0; j < ordersdata.orderData.Count(); j++)
        {
            var i = j + 10;
            // dataTable.Rows.Add(row.OrderID,row.OrderDate,row.Customer,row.Status,row.PaymentMod,row.Rating,row.Total);
            ws.Cell("A" + i).Value = ordersdata.orderData[j].OrderId;
            ws.Range("B" + i, "D" + i).Merge();
            ws.Cell("B" + i).Value = ordersdata.orderData[j].OrderDate + "";
            ws.Range("E" + i, "G" + i).Merge();
            ws.Cell("E" + i).Value = ordersdata.orderData[j].CustomerName;
            ws.Range("H" + i, "J" + i).Merge();
            ws.Cell("H" + i).Value = ordersdata.orderData[j].OrderStatus;
            ws.Range("K" + i, "L" + i).Merge();
            ws.Cell("K" + i).Value = ordersdata.orderData[j].PaymentMethod;
            ws.Range("M" + i, "N" + i).Merge();
            ws.Cell("M" + i).Value = ordersdata.orderData[j].Rating;
            ws.Range("O" + i, "P" + i).Merge();
            ws.Cell("O" + i).Value = ordersdata.orderData[j].TotalAmount;
        }


        using var stream = new MemoryStream();
        wb.SaveAs(stream);
        stream.Position = 0;

        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders.xlsx");


    }



    [HttpGet]
    public async Task<IActionResult> OrderDetails(int id)
    {
        var model = _orderService.GetOrderDetails(id);
        return View(model);
    }

    public async Task<IActionResult> DownloadInvoice(int id)
    {
        // Setup Rotativa
        var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        Rotativa.AspNetCore.RotativaConfiguration.Setup(wwwrootPath, "Rotativa");

        // Await the async call to get the model
        var model = _orderService.GetOrderDetails(id);

        if (model == null)
        {
            TempData["Error"] = "Order notfound";
            return RedirectToAction("Order", "Order");
            // _logger.LogWarning("No order details found for Order ID {OrderId}.", id);
        }

        // Return the model to the view
        // return View(model);

        // Uncomment this to directly generate the PDF if desired
        return new Rotativa.AspNetCore.ViewAsPdf("DownloadInvoice", model) { FileName = "Invoice.pdf" };
        // return View("DownloadInvoice", model);
    }
}
