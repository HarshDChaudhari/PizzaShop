using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;

namespace PizzaShop.Repository.Interfaces;

public interface IRoleRepository
{
    List<Role> GetRoles();
    Role GetRoleByUserId(string userId);
    List<RolePermission> GetPermissionByroleId(int roleId);
    List<Permission> GetPermissionListByRoleId(int roleId);
    Role GetRoleById(int roleId);

    RoleViewModel UpdatePermission(RoleViewModel model);
    }