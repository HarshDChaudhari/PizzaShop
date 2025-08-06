using System.Linq;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Entity.Data;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;

namespace PizzaShop.Repository.Implementations;

public class RoleRepository(PizzaShopDbContext _content) : IRoleRepository
{
    public List<Role> GetRoles()
    {
        return _content.Roles.ToList();
    }

    public Role GetRoleByUserId(string userId)
    {
        var userIdInt = int.Parse(userId);
        var user = _content.Users.FirstOrDefault(u => u.UserId == userIdInt);
        if (user == null)
        {
            throw new Exception($"User with id {userId} not found.");
        }
        var role = _content.Roles.FirstOrDefault(r => r.RoleId == user.UserRole);
        if (role == null)
        {
            throw new Exception($"Role with id {user.UserRole} not found.");
        }
        return role;
    }

    public Role GetRoleById(int roleId)
    {
        var role = _content.Roles.FirstOrDefault(r => r.RoleId == roleId);
        if (role == null)
        {
            throw new Exception($"Role with id {roleId} not found.");
        }
        return role;
    }

    public List<RolePermission> GetPermissionByroleId(int roleId)
    {
        return _content.RolePermissions.Where(rp => rp.RoleId == roleId).Include(rp => rp.Permission).ToList();
    }
    public List<Permission> GetPermissionListByRoleId(int roleId)
    {
        var permissionList = _content.RolePermissions
                                    .Where(rp => rp.RoleId == roleId)
                                    .OrderBy(rp => rp.Permission.PermissionId)
                                    .Select(rp => rp.Permission)
                                    .ToList();
        
        return permissionList;
    }

    public RoleViewModel UpdatePermission(RoleViewModel model){
        if(model != null){
            var query = _content.RolePermissions.Where(x => x.RoleId == model.RoleId).ToList();
            foreach(var item in model.PermissionList){
                var value = query.FirstOrDefault(y => y.PermissionId == item.PermissionId );

                value.CanView = item.CanView;
                value.CanAddEdit = item.CanAddEdit;
                value.CanDelete = item.CanDelete;

                _content.Update(value);
            }
            _content.SaveChanges();
            return model;
        }
        return null;
    }


}
