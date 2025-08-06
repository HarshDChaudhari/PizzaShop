using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Service.Implementations;

public class RoleService(IRoleRepository _roleRepository) : IRoleService
{
    public List<Role> GetRoles()
    {
        return _roleRepository.GetRoles();
    }

    public Role GetRoleById(int roleId){
        return _roleRepository.GetRoleById(roleId);
    }

    public List<RolePermission> GetPermissionByroleId(int roleId){
        return _roleRepository.GetPermissionByroleId(roleId);
    }

    public List<Permission> GetPermissionListByRoleId(int roleId){
        return _roleRepository.GetPermissionListByRoleId(roleId);
    } 

    public RoleViewModel UpdatePermission (RoleViewModel model){
        var index = _roleRepository.UpdatePermission(model);
        if(index !=null){
            return model;
        }
        return null;
    }


    public Role GetRoleByUserId(string userId)
    {
        return _roleRepository.GetRoleByUserId(userId);
    }

}
