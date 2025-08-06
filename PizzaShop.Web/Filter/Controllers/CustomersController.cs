using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Web.Controllers;
[ServiceFilter(typeof(PermissionFilter))]
public class CustomersController(ICustomersService _customersService) : Controller
{
    public IActionResult Customers()
    {
        TempData["Active"] = "Customers";
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> GetCustomerDetails( int page, int pageSize, string search, string customerTime , string sortCustomerColumn = "", string sortCustomerOrder = "asc")
    {
        try
        {
            PaginationViewModel<CustomerViewModel> table = await _customersService.GetCustomerDetail(page, pageSize, search, customerTime,sortCustomerColumn,sortCustomerOrder);
             return PartialView("./PartialView/_CustomerPartial", table);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }


     public IActionResult ExportAllCustomer(string time,string search)
    {
        // int timeint = int.Parse(time);
        var customerdata = _customersService.GetExportCustomer(search: search, time: time);

        using var wb = new XLWorkbook();

        var ws = wb.AddWorksheet();
        ws.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        ws.Range("A2", "B3").Merge().Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Cell("A2").Value = "Account";
        ws.Cell("A2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        .Border.SetTopBorder(XLBorderStyleValues.Thin)
        .Border.SetRightBorder(XLBorderStyleValues.Thin)
        .Border.SetBottomBorder(XLBorderStyleValues.Thin)
        .Border.SetLeftBorder(XLBorderStyleValues.Thin);
        ws.Range("C2", "F3").Merge();
      //  ws.Cell("C2").Value = ordersdata.status;
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
        ws.Cell("J2").Value = customerdata.search;
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
        ws.Cell("C5").Value = customerdata.Date;
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
        ws.Cell("J5").Value = customerdata.record;
        ws.Cell("J5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        .Border.SetTopBorder(XLBorderStyleValues.Thin)
        .Border.SetRightBorder(XLBorderStyleValues.Thin)
        .Border.SetBottomBorder(XLBorderStyleValues.Thin)
        .Border.SetLeftBorder(XLBorderStyleValues.Thin);

        // var img = "D:\\DotNet Project\\Pizzashop\\PizzaShop\\Pizzashop.web\\wwwroot\\images\\logos\\pizzashop_logo.png";
        var img = "D:\\DotNet N Layered\\PizzaShop\\PizzaShop.Web\\wwwroot\\images\\logos\\pizzashop_logo.png";
        ws.Range("O2", "P6").Merge();
        ws.AddPicture(img).MoveTo(ws.Cell("O2")).Scale(.3);


        ws.Cell("A9").Value = "Id";
        ws.Cell("A9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Range("B9", "D9").Merge();
        ws.Cell("B9").Value = "Name";
        ws.Cell("B9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Range("E9", "G9").Merge();
        ws.Cell("E9").Value = "Email";
        ws.Cell("E9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Range("H9", "J9").Merge();
        ws.Cell("H9").Value = "Date";
        ws.Cell("H9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Range("K9", "L9").Merge();
        ws.Cell("K9").Value = "Phone";
        ws.Cell("K9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Range("M9", "N9").Merge();
        ws.Cell("M9").Value = "TotalOrder";
        ws.Cell("M9").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0066A8"));
        ws.Range("O9", "P9").Merge();
        

        for (var j = 0; j < customerdata.CustomerData.Count(); j++)
        {
            var i = j + 10;
            // dataTable.Rows.Add(row.OrderID,row.OrderDate,row.Customer,row.Status,row.PaymentMod,row.Rating,row.Total);
            ws.Cell("A" + i).Value = customerdata.CustomerData[j].CustomerId;
            ws.Range("B" + i, "D" + i).Merge();
            ws.Cell("B" + i).Value = customerdata.CustomerData[j].CustomerName;
            ws.Range("E" + i, "G" + i).Merge();
            ws.Cell("E" + i).Value = customerdata.CustomerData[j].Email;
            ws.Range("H" + i, "J" + i).Merge();
            ws.Cell("H" + i).Value = customerdata.CustomerData[j].Date + "";
            ws.Range("K" + i, "L" + i).Merge();
            ws.Cell("K" + i).Value = customerdata.CustomerData[j].Phone;
            ws.Range("M" + i, "N" + i).Merge();
            ws.Cell("M" + i).Value = customerdata.CustomerData[j].TotalOrder;
           
        }


        using var stream = new MemoryStream();
        wb.SaveAs(stream);
        stream.Position = 0;

        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customer.xlsx");
    }

    [HttpGet]
    public IActionResult GetCustomerHistory(int customerId)
    {
        var customerHistory = _customersService.GetCustomerHistory(customerId);
        return PartialView("./PartialView/_CustomerHistory", customerHistory);
    }
}
