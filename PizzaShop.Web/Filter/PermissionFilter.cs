using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PizzaShop.Service.Interfaces;
using PizzaShop.Entity.Models;
using Microsoft.AspNetCore.Http;

public class PermissionFilter : IActionFilter
{
    private readonly IRoleService _roleService;
    private readonly IJwtService _jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionFilter(IRoleService roleService, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
    {
        _roleService = roleService;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }

    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        
        var userRoleId = 0;
        bool canView;
        bool canAddEdit;
        bool canDelete;
        var principal = _jwtService.ValidateToken(_httpContextAccessor.HttpContext.Request.Cookies["SuperSecretAuthToken"]);
        if (principal == null)
        {
            context.Result = new RedirectResult("/Validation/Login");
            return;
        }
        if (principal != null)
        {
            userRoleId = int.Parse(principal.FindFirst("RoleId")?.Value ?? "0");
        }

        var rolePermissions = _roleService.GetPermissionByroleId(userRoleId);

        var actionName = context.ActionDescriptor.RouteValues["controller"];

        if(actionName == "Validation" || actionName == "Home"){
            canView = true;
            canAddEdit = true;
            canDelete = true;
        }
        else{
            canView = CheckUserPermission(actionName, rolePermissions, permissionType: "CanView");
            canAddEdit = CheckUserPermission(actionName, rolePermissions, permissionType: "CanAddEdit");
            canDelete = CheckUserPermission(actionName, rolePermissions, permissionType: "CanDelete");
        }


            // If the action doesn't meet permission criteria, return "Permission Denied"
        // Continue normal execution
        context.HttpContext.Items["CanView"] = canView;
        context.HttpContext.Items["CanAddEdit"] = canAddEdit;
        context.HttpContext.Items["CanDelete"] = canDelete;

            if (!canView)
            {
                var controller = context.Controller as Controller;
                if (controller != null)
                {
                    controller.TempData["Error"] = "Access Denied";
                }
                context.Result = new RedirectResult("/Home/Index");
                return;
            }


        if (!Convert.ToBoolean(context.HttpContext.Items["CanDelete"]) || !Convert.ToBoolean(context.HttpContext.Items["CanAddEdit"]))
        {
            var method = context.HttpContext.Request.Method;
            if (method != "GET")
            {
                if (context.HttpContext.Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    context.Result = new JsonResult(new { error = true , message = "Permission Denied" }) { StatusCode = StatusCodes.Status403Forbidden };
                }
                else
                {
                    var controller = context.Controller as Controller;
                    if (controller != null)
                    {
                        controller.TempData["Error"] = "Permission Denied";
                    }
                    context.Result = new RedirectResult(context.HttpContext.Request.Headers["Referer"].ToString());
                }
            }
            return;
        }
        
    }



    public void OnActionExecuted(ActionExecutedContext context)
    {
        
        // No action needed after the action executes
    }

    private bool CheckUserPermission(string actionName, List<RolePermission> rolePermissions, string permissionType)
    {
        return rolePermissions.Any(rp =>
            rp.Permission != null &&
            rp.Permission.ModuleName == actionName &&
            (permissionType == "CanView" && rp.CanView ||
             permissionType == "CanAddEdit" && rp.CanAddEdit ||
             permissionType == "CanDelete" && rp.CanDelete));
    }
}

