using System;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Entity.Data;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;

namespace PizzaShop.Repository.Implementations;

public class AuthRepository (PizzaShopDbContext _context) : IAuthRepository
{
    public async Task<User> AuthenticateUser(string email, string password)
    {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
    }

    public User Useremail(string email)
    {       
        var user = _context.Users.FirstOrDefault(u => u.Email == email);

        return user ;
    }


    public async Task<bool> Savepassword(Entity.Models.User user)
    {
        _context.Users.Update(user);
        return (await _context.SaveChangesAsync() > 0);
    }

    public async Task<Role> GetRole(string roleName)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        return role;
    }
}
