using Microsoft.AspNetCore.Mvc;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Web.Controllers;
[ServiceFilter(typeof(PermissionFilter))]
public class RoleAndPermissionController(IRoleService _roleService, IUserService _userService) : Controller
{
    [HttpGet]
    public IActionResult Roles()
    {
        TempData["Active"] = "RolesAndPermissions";

        ViewBag.Roles = _roleService.GetRoles();

        return View();
    }

    [HttpGet]
    public IActionResult Permission(int roleId)
    {
        try
        {
            TempData["Active"] = "RolesAndPermissions";
            ViewBag.Roles = roleId;
            ViewBag.RoleName = _roleService.GetRoleById(roleId).RoleName;

            var rolePermission = _roleService.GetPermissionByroleId(roleId);

            var permissionList = _roleService.GetPermissionListByRoleId(roleId);

            var model = new RoleViewModel
            {
                RoleId = roleId,
                PermissionList = rolePermission.Select(x => new PermissionViewModel
                {
                    PermissionId = x.PermissionId,
                    ModuleName = permissionList.FirstOrDefault(p => p.PermissionId == x.PermissionId).ModuleName,
                    CanView = x.CanView,
                    CanAddEdit = x.CanAddEdit,
                    CanDelete = x.CanDelete,
                }).ToList()
            };

            return View(model);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public IActionResult Permission(RoleViewModel model)
    {
        try
        {
            var updated = _roleService.UpdatePermission(model);
            TempData["Success"] = "Permission updated successfully.";
            return RedirectToAction("Roles", new { RoleId = model.RoleId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
