using System;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;

namespace PizzaShop.Repository.Interfaces;

public interface IAuthRepository
{
    Task<User> AuthenticateUser(string email, string password);

   User Useremail(string email);

    Task<bool> Savepassword(User user);


    Task<Role> GetRole(string roleName);

}
