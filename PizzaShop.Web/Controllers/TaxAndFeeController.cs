using Microsoft.AspNetCore.Mvc;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Web.Controllers;

[ServiceFilter(typeof(PermissionFilter))]
public class TaxAndFeeController(ITaxesAndFeesService _taxService) : Controller
{
[HttpGet]
public IActionResult TaxesAndFees(string search)
{
    try
    {
        TempData["Active"] = "TaxesAndFees";
        var taxFeesList = _taxService.GetTaxesAndFees(search);
        var viewModel = new TaxesAndFeesViewModel
        {
            TaxFeesList = taxFeesList.Select(tax => new TaxesAndFeesViewModel
            {
                TaxId = tax.TaxId,
                TaxName = tax.TaxName,
                TaxType = tax.TaxType,
                TaxValue = tax.TaxValue,
                IsDefault = tax.IsDefault,
                IsEnabled = tax.IsEnabled,
            }).ToList()
        };
        return View(viewModel);
    }
    catch (Exception ex)
    {
        TempData["Error"] = ex.Message;
        return Redirect(Request.Headers["Referer"].ToString());
    }
}

    [HttpPost]
    public IActionResult AddTax(TaxesAndFeesViewModel model){
        try
        {
            var existingTax = _taxService.GetTaxesAndFees(model.TaxName);
            if (existingTax != null)
            {
                TempData["Error"] = "Tax already exists.";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            _taxService.AddTax(model);
            TempData["Success"] = "Tax Added Successfully";
            return RedirectToAction("TaxesAndFees");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult DeleteTaxFee(int taxId)
    {
        try
        {
            _taxService.DeleteTaxFee(taxId);
            TempData["Success"] = "Tax or fee deleted successfully.";
            return RedirectToAction("TaxesAndFees");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpGet]
    public IActionResult GetAllTaxByTaxId(int TaxId)
    {
        try
        {
            var item = _taxService.GetAllTaxByTaxId(TaxId);
            if (item == null)
            {
                return NotFound();
            }
            return Json(item);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult EditTax(TaxesAndFeesViewModel model){
        try
        {
            _taxService.UpdateTax(model);
            TempData["Success"] = "Tax Updated Successfully";
            return RedirectToAction("TaxesAndFees");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
    
}
