using PizzaShop.Entity.Models;
namespace PizzaShop.Repository.Interfaces;

public interface IUserRepository
{

    List<User> GetAllUser();
    

    string GetRoleName(int id);
     User GetAll(int id);

     User GetAllByEmail(string email);

    List<Country> GetCountry();

    List<State> GetState(int country_id);

    Role GetRole(int id);

    bool Delete(User user);

    List<Role> GetRoleList();

   List<City> GetCity(int state_id);


    bool Update(User user);

    bool Add(User user);

}