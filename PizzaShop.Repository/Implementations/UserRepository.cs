using Microsoft.EntityFrameworkCore;
using PizzaShop.Entity.Data;
using PizzaShop.Entity.Models;
using PizzaShop.Repository.Interfaces;

namespace PizzaShop.Repository.Implementations;

public class UserRepository (PizzaShopDbContext _context) : IUserRepository
{

    public List<User> GetAllUser(){
        
        return   _context.Users.OrderBy(x => x.UserId)
                    .ToList();
    }

     public string GetRoleName(int id)
        {
            var role = _context.Roles.Find(id);

            if (role == null)
            {
                return "Role not found";
            }
            return role.RoleName;
        }

    public  User GetAll(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            throw new Exception($"User with id {id} not found.");
        }
        return user;
    }

    public  User GetAllByEmail(string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            throw new Exception($"User with email {email} not found.");
        }
        return user;
    }

    public Role GetRole(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);

        if (user == null)
        {
            throw new Exception($"User with id {id} not found.");
        }
        var role = _context.Roles.FirstOrDefault(r => r.RoleId == user.UserRole);
        if (role == null)
        {
            throw new Exception($"Role with id {user.UserRole} not found.");
        }
        return role;
    }

    public List<Country> GetCountry()
    {
        return _context.Countries.ToList();
   }

    public List<Role> GetRoleList()
    {
        return _context.Roles.ToList();
    }

    public  List<State> GetState(int country_id)
    {
        return _context.States.Where(s => s.CountryId == country_id).ToList();
    }

    public  List<City> GetCity(int state_id)
    {

        return _context.Cities.Where(c => c.StateId == state_id).ToList();
    }



    public bool Update(User user)
    {

        _context.Users.Update(user);
        return _context.SaveChanges() > 0;
    }

    public bool Add(User user)
    {
        _context.Users.Add(user);
        return _context.SaveChanges() > 0;
    }

    public bool Delete(User user)
    {
        user.Isdeleted = true;
        // _context.Users.Remove(user);
        _context.Users.Update(user);
        return _context.SaveChanges() > 0;
    }

}
