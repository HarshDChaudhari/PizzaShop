using Microsoft.AspNetCore.Mvc;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Service.Interfaces;


namespace PizzaShop.Web.Controllers;
[ServiceFilter(typeof(PermissionFilter))]
public class TableAndSectionController(ITablesAndSectionService _tablesAndSectionService, IJwtService _jwtService) : Controller
{

    [HttpGet]
    public IActionResult Sections()
    {
        TempData["Active"] = "TableAndSection";
        var sectionsList = _tablesAndSectionService.GetSections();
        // var principal = _jwtService.ValidateToken(Request.Cookies["SuperSecretAuthToken"]);
        // if (principal == null)
        // {
        //     return RedirectToAction("Login", "Validation");
        // }
        // int RoleId = int.Parse(principal?.Claims.ElementAt(3).Value ?? "0");
        // var rolePermissions = _tablesAndSectionService.GetPermissionByroleId(RoleId);

        var viewModel = new SectionsViewModel
        {
            SectionList = sectionsList.Select(section => new SectionsViewModel
            {
                SectionId = section.SectionId,
                SectionName = section.SectionName,
                Description = section.Description,
            }).ToList(),

            // PermissionList = rolePermissions.Select(x => new PermissionViewModel
            // {
            //     ModuleName = x.Permission.ModuleName,
            //     CanView = x.CanView,
            //     CanAddEdit = x.CanAddEdit,
            //     CanDelete = x.CanDelete,
            // }).ToList()

        };
        return View(viewModel);
    }
    
    [HttpGet]
    public async Task<IActionResult> Tables(int SectionId, int page, int pageSize, string search = "")
    {
        try
        {
            ViewBag.search = search;
            PaginationViewModel<TableViewModel> table = await _tablesAndSectionService.GetTable(SectionId, page, pageSize, search);

            return PartialView("./PartialView/_Table", table);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpGet]
    public IActionResult SectionsList()
    {
        var sectionsList = _tablesAndSectionService.GetSections();

        return Json(sectionsList);
    }

    [HttpPost]
    public IActionResult AddSection(SectionsViewModel model)
    {
        try
        {
            var check = _tablesAndSectionService.GetSections().FirstOrDefault(x => x.SectionName == model.SectionName);
            if (check != null)
            {
                TempData["Error"] = "Section already exists.";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            _tablesAndSectionService.AddSection(model);
            TempData["Success"] = "Added Section Successfully";
            return RedirectToAction("Sections");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult EditSection(SectionsViewModel model)
    {
        try
        {
            
            _tablesAndSectionService.EditSection(model);
            TempData["Success"] = "Sections Updated successfully.";
            return RedirectToAction("Sections");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult DeleteSection(SectionsViewModel model)
    {
        try
        {
            _tablesAndSectionService.DeleteSection(model.SectionId);
            TempData["Success"] = "Deleted successfully.";
            return RedirectToAction("Sections");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }



    [HttpPost]
    public IActionResult AddTable(TableViewModel model)
    {
        try
        {
            var check = _tablesAndSectionService.GetAllTable().FirstOrDefault(x => x.TableName == model.TableName && x.SectionId == model.SectionId);
            if (check != null)
            {
                return Json(new { Success = false , Message = "Table already exists." });
            }
            _tablesAndSectionService.AddTable(model);
            TempData["Success"] = "Table Added Successfully";
            return Json(new { Success = true });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpGet]
    public IActionResult GetTableDetails(int tableId)
    {
        try
        {
            var item = _tablesAndSectionService.GetTableById(tableId);
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
    public IActionResult UpdateTable(TableViewModel model)
    {
        try
        {
            _tablesAndSectionService.EditTable(model);
            TempData["Success"] = "Table Updated Successfully";
            return Json(new { Success = true });
            // return PartialView("./PartialView/_Table", model);
            // return Json(model);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult DeleteTable(int tableId)
    {
        try
        {
            var result = _tablesAndSectionService.DeleteTable(tableId);

            if (result)
            {
                TempData["Success"] = "Table deleted successfully.";
                return Json(result);
            }
            else
            {
                TempData["Error"] = "Failed to delete Table.";
                return Json(result);
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult DeleteMultipleTable(List<int> dataId)
    {
        try
        {
            if (dataId.Count != 0)
            {

                var result = _tablesAndSectionService.DeleteMultipleTable(dataId);

                if (result)
                {
                    TempData["Success"] = "Category item deleted successfully.";
                    return Json(result);
                }
                else
                {
                    TempData["Error"] = "Failed to delete category item.";
                    return Json(result);
                }
            }
            else
            {
                TempData["Error"] = "Please select Category Item";
                return RedirectToAction("Menu", "Menu");
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
        

    }

}
