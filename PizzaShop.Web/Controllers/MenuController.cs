using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Service.Interfaces;
using Newtonsoft.Json;
using PizzaShop.Service.Utils;
using static PizzaShop.Filter.CustomAuthorize;
using PizzaShop.Filter;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.EMMA;

namespace PizzaShop.Web.Controllers;

[ServiceFilter(typeof(PermissionFilter))]
[CustomAuthorize(UserRoles.Admin, UserRoles.Manager)]
public class MenuController(IMenuService _menuService) : Controller
{

    [HttpGet]
    public IActionResult Menu()
    {
        try
        {
            TempData["Active"] = "Menu";
            var data = _menuService.GetCategories();
            var data2 = _menuService.GetModifierGroups();

            if (data != null && data2 != null)
            {
                var menu = new MenuViewModel
                {
                    CategoriesList = data.Select(p => new CategoryViewModel
                    {
                        CategoryId = p.CategoryId,
                        CategoryName = p.CategoryName,
                        Description = p.Description
                    }).ToList(),

                    ModifierList = data2.Select(p => new ModifierViewModel
                    {
                        ModifierGroupId = p.ModifierGroupId,
                        ModifierName = p.ModifierName,
                        Description = p.Description
                    }).ToList(),

                };
                return View(menu);
            }
            return View(new MenuViewModel());
        }

        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
    #region Category


    // for categoryList View
    [HttpGet]
    public async Task<IActionResult> CategoryItems(int categoryId, int page, int pageSize, string search = "")
    {
        try
        {
            PaginationViewModel<CategoryListViewModel> categoryItems = await _menuService.GetCategoryItems(categoryId, page, pageSize, search);
            // var categoryItems = _menuService.GetCategoryItemsByCategoryId(categoryId);

            // return Json(categoryItems);
            // return PartialView(categoryItems);
            return PartialView("./PartialView/CategoryList", categoryItems);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    // for edit categoryItems
    [HttpGet]
    public IActionResult GetItemDetails(int itemId)
    {
        try
        {
            var item = _menuService.GetItemById(itemId);
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
    [HttpGet]
    public IActionResult GetEditCategoryItemModal()
    {
        try
        {
            return PartialView("./PartialView/_EditCategoryItemsModal");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpGet]
    public IActionResult GetAddCategoryItemModal()
    {
        try
        {
            return PartialView("PartialView/_AddCategoryItemsModal");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCategoryItem(CategoryListViewModel formData)
    {
        try
        {

            var selectedModifiersJson = formData.SelectedModifiersString;


            var selectedModifiers = JsonConvert.DeserializeObject<List<CategoryModifierMappingsViewModel>>(selectedModifiersJson);


            formData.SelectedModifiers = selectedModifiers?.Where(m => m != null).ToList();


            if (formData.FormFile != null && formData.FormFile.Length > 0)
            {
                var path = await ProfileImageUploadUtils.SaveCategoryAndModifierProfileImageUploadAsync(formData.FormFile);
                formData.ImageUrl = $"{Request.Scheme}://{Request.Host}/{path}";

            }

            _menuService.EditCategoryItem(formData);
            TempData["Success"] = "CategoryItem Edited Successfully";
            // return PartialView("./PartialView/Category");
            return Json(new { Success = true });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddCategoryItem(CategoryListViewModel model)
    {
        try
        {
            var selectedModifiersJson = model.SelectedModifiersString;
            var selectedModifiers = JsonConvert.DeserializeObject<List<CategoryModifierMappingsViewModel>>(selectedModifiersJson);


            model.SelectedModifiers = selectedModifiers?.Where(m => m != null).ToList();

            if (model.FormFile != null && model.FormFile.Length > 0)
            {
                var path = await ProfileImageUploadUtils.SaveCategoryAndModifierProfileImageUploadAsync(model.FormFile);
                model.ImageUrl = $"{Request.Scheme}://{Request.Host}/{path}";
            }
            _menuService.AddCategoryItem(model);
            TempData["Success"] = "CategoryItem Added Successfully";
            return Json(new { Success = true });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult AddCategory(MenuViewModel model)
    {
        try
        {
            var check = _menuService.CheckCategory(model.CategoryName);
            if (check)
            {
                return Json(new { Success = false, Message = "Category already exists." });
            }
            _menuService.AddCategory(model);
            TempData["Success"] = "Category Added Successfully";
            return Json(new { Success = true });
            // return RedirectToAction("Menu");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult EditCategory(MenuViewModel model)
    {
        try
        {
            var category = _menuService.GetCategoryById(model.CategoryId);
            if (category.CategoryName != model.CategoryName)
            {
                var check = _menuService.CheckCategory(model.CategoryName);
                if (check)
                {
                    return Json(new { Success = false, Message = "Category already exists." });
                }
            }
            _menuService.EditCategory(model);
            TempData["Success"] = "Category Updated successfully.";
            return Json(new { Success = true });
            // return RedirectToAction("Menu");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult DeleteCategory(MenuViewModel model)
    {
        try
        {
            _menuService.RemoveCategory(model.CategoryId);
            TempData["Success"] = "Deleted successfully.";
            return RedirectToAction("Menu");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    public IActionResult GetCategory()
    {
        try
        {
            var category = _menuService.GetCategories();
            return Json(category);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult DeleteCategoryItem(int itemId)
    {
        try
        {
            var result = _menuService.DeleteCategoryItem(itemId);

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
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult DeleteMultipleCategoryItem(List<int> dataId)
    {
        try
        {

            if (dataId.Count != 0)
            {

                var result = _menuService.DeleteMultipleCategoryItem(dataId);

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

    #endregion

    #region Modifier
    [HttpGet]
    public async Task<IActionResult> ModifierItems(int ModifierGroupId, int page, int pageSize, string search = "")
    {
        try
        {
            PaginationViewModel<ModifierListViewModel> modifierItems = await _menuService.GetModifierItems(ModifierGroupId, page, pageSize, search);


            return PartialView("./PartialView/ModifierList", modifierItems);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    // [Route("/Menu/GetModifierItemDetails")]
    [HttpGet]
    public async Task<IActionResult> GetModifierItemDetails(int modifierGroupId)
    {
        try
        {
            var item = await _menuService.GetModifierItemById(modifierGroupId);
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
    // [Route("/Menu/GetAllModifier")]
    [HttpGet]
    public IActionResult GetAllModifier()
    {
        try
        {
            var item = _menuService.GetAllModifierById();
            return Json(item);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
    [HttpGet]
    public IActionResult ModifierItemsList()
    {
        try
        {
            var item = _menuService.ModifierItemsList();
            return Json(item);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }


    [HttpPost]
    public IActionResult UpdateModifierItem(ModifierListViewModel model)
    {
        try
        {
            _menuService.EditModifierItem(model);
            TempData["Success"] = "ModifierItem Edited Successfully";
            return Json(new { Success = true });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult AddModifierItem(ModifierListViewModel model)
    {
        try
        {
            var check = _menuService.GetModifierItems().FirstOrDefault(x => x.ModifierItemName == model.ModifierItemName);
            if (check != null)
            {
                TempData["Error"] = "ModifierItem already exists.";
                return Json(new { Success = false, Message = "ModifierItem already exists." });
            }
            if (ModelState.IsValid)
            {
                _menuService.AddModifierItem(model);
                TempData["Success"] = "ModifierItem added Successfully";
                return Json(new { Success = true });

            }
            return Json(new { Success = false, Message = "Please enter valid data." });

        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }




    [HttpPost]
    public IActionResult AddModifier(MenuViewModel model, string SelectedModifierIds, string selectedModifierName)
    {
        try
        {
            var check = _menuService.GetModifierGroups().FirstOrDefault(x => x.ModifierName == model.ModifierName);
            if (check != null)
            {
                TempData["Error"] = "Modifier already exists.";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            if (!string.IsNullOrEmpty(SelectedModifierIds))
            {
                model.SelectedModifierIds = SelectedModifierIds.Split(',').Select(int.Parse).ToList();
                model.SelectedModifierName = selectedModifierName.Split(',').ToList();
            }
            _menuService.AddModifier(model);
            TempData["Success"] = "Added Modifier Successfully";
            return RedirectToAction("Menu");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    //  public async Task<IActionResult> AddModifierGroup(AddModifierGroupViewModel model, string SelectedModifierIds, string selectedModifierName)
    // {
    //     if (!string.IsNullOrEmpty(SelectedModifierIds))
    //     {
    //         model.SelectedModifierIds = SelectedModifierIds.Split(',').Select(int.Parse).ToList();
    //         model.selectedModifierName = selectedModifierName.Split(',').ToList();
    //     }
    //     _categoryService.AddModifierGroupAsync(model);
    //     TempData["succses"] = "Add Modifier Group";
    //     return RedirectToAction("Menupage");
    // }

    [HttpGet]
    public IActionResult GetAddModifierItemModal()
    {
        try
        {
            return PartialView("PartialView/_AddModifierItemsModal");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]

    public IActionResult EditModifier(MenuViewModel model)
    {
        try
        {
            _menuService.EditModifier(model);
            TempData["Success"] = "Modifier Edited Successfully";
            return Json(new { Success = true });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }


    [HttpPost]
    public IActionResult DeleteModifier(MenuViewModel model)
    {
        try
        {
            _menuService.RemoveModifier(model.ModifierGroupId);
            TempData["Success"] = "Deleted successfully.";
            return RedirectToAction("Menu");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }


    public IActionResult GetModifier()
    {
        try
        {
            var modifiers = _menuService.GetModifierGroups();
            return Json(modifiers);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    public IActionResult GetUnitList()
    {
        try
        {
            var list = _menuService.UnitList();
            return Json(list);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }


    [HttpPost]
    public IActionResult DeleteModifierItem(int ModifierItemId , int ModifierGroupId)
    {
        try
        {
            var result = _menuService.DeleteModifierItem(ModifierItemId, ModifierGroupId);

            if (result)
            {
                TempData["Success"] = "Modifier item deleted successfully.";
                return Json(result);
            }
            else
            {
                TempData["Error"] = "Failed to delete modifier item.";
                return Json(result);
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    // [HttpGet]
    // public IActionResult GetALLModifierItemDetails()
    // {
    //     var modifierItemsList = _menuService.GetModifierItems();
    //     return PartialView("./PartialView/_ExistingModifiersPartial" ,modifierItemsList);
    // }
    [HttpGet]
    public async Task<IActionResult> GetALLModifierItemDetails(int page, int pageSize, string search = "")
    {
        try
        {
            PaginationViewModel<ExistingModifierViewModel> modifierItemsList = await _menuService.GetModifierItems(page, pageSize, search);
            return PartialView("PartialView/_ExistingModifiersTablePartial", modifierItemsList);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult DeleteMultipleModifierItem(List<int> dataId)
    {
        try
        {
            if (dataId.Count != 0)
            {

                var result = _menuService.DeleteMultipleModifierItem(dataId);

                if (result)
                {
                    TempData["Success"] = "Modifier item deleted successfully.";
                    return Json(result);
                }
                else
                {
                    TempData["Error"] = "Failed to delete Modifier item.";
                    return Json(result);
                }
            }
            else
            {
                TempData["Error"] = "Please select Modifier Item";
                return RedirectToAction("Menu", "Menu");
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpGet]
    public IActionResult ExistingModifierModelShow()
    {
        try
        {
            return PartialView("PartialView/_ExistingModifiersPartial");
            // return PartialView("PartialView/_ExistingModifiersTablePartial");
            // return Ok();
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpGet]
    public IActionResult allModifierItemsByModifierGroupId(int modifierGroupId)
    {
        try
        {
            var item = _menuService.GetAllModifierItemsByModifierGroupId(modifierGroupId);
            return Json(item);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }


    #endregion

    // public IActionResult CheckConstrians(string name, string value = "")
    // {
    //     if (string.IsNullOrEmpty(value))
    //     {
    //         return Ok();
    //     }
    //     var check =_menuService.CheckConstrain(name : name, value : value );
    //     if( check == false)
    //     {
    //         return Ok();
    //     }
    //     return Ok(value+ " is already present");
    // }

}
