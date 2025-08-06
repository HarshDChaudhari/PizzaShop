using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Service.Implementations;

public class UserService(IUserRepository _repository) : IUserService
{
    public ProfileViewModel GetUser(string id)
    {
        if (!int.TryParse(id, out int userId))
        {
            return null;
        }
        var user = _repository.GetAll(userId);
        if (user == null)
        {
            return null;
        }

        var role = user.UserRole.HasValue ? _repository.GetRole(user.UserId) : null;

        var profileViewModel = new ProfileViewModel
        {
            Id = user.UserId,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Country = user.CountryId,
            State = user.StateId,
            City = user.CityId,
            Phone = user.Phone,
            Address = user.Address,
            ZipCode = user.ZipCode,
            Role = role?.RoleName,
            Imgurl = user.ImgUrl
        };

        return profileViewModel;
    }

    public List<User> GetAllUser(){

        return _repository.GetAllUser();
    }

    public EdituserViewModel GetUserByEmail(string Email)
    {

        var user = _repository.GetAllByEmail(Email);
        if (user == null)
        {
            return null;
        }

        var role = user.UserRole.HasValue ? _repository.GetRole(user.UserId) : null;

        var edituserViewModel = new EdituserViewModel
        {
            Email = user.Email,
            Role = user.UserRole,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Status = user.Status,
            Country = user.CountryId,
            State = user.StateId,
            City = user.CityId,
            Phone = user.Phone,
            Address = user.Address,
            ZipCode = user.ZipCode,
            Imgurl = user.ImgUrl
        };

        return edituserViewModel;
    }


    public List<Country> GetCountries()
    {
        return _repository.GetCountry();
    }

    public List<Role> GetRoles()
    {
        return _repository.GetRoleList();
    }

    public List<State> GetStates(int CountryId)
    {
        return _repository.GetState(CountryId);
    }

    public List<City> GetCities(int StateId)
    {
        return _repository.GetCity(StateId);
    }

    public bool UpdateUser(ProfileViewModel model)
    {
        var user = _repository.GetAll(model.Id);
        if (user == null)
        {
            return false;
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Username = model.Username;
        user.CountryId = model.Country;
        user.StateId = model.State;
        user.CityId = model.City;
        user.Phone = model.Phone;
        user.ImgUrl = model.Imgurl;
        user.Address = model.Address;
        user.ZipCode = model.ZipCode;

        return _repository.Update(user);
    }
    public bool UpdateUserinEdit(EdituserViewModel model)
    {
        if (model != null)
        {

            var user = _repository.GetAllByEmail(model.Email);
            if (user == null)
            {
                return false;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Username = model.Username;
            user.CountryId = model.Country;
            user.StateId = model.State;
            user.CityId = model.City;
            user.Phone = model.Phone;
            user.Address = model.Address;
            user.ZipCode = model.ZipCode;
            user.Status = model.Status;
            user.UserRole = model.Role;
            user.ImgUrl = model.Imgurl;

            return _repository.Update(user);
        }
        return false;
    }

    // public bool UpdateProfileImage(ProfileViewModel model)
    // {
    //     var user = _repository.GetAll(model.Id);
    //     if (user == null)
    //     {
    //         return false;
    //     }

    //     user.ProfileImg = model.ProfileImg;

    //     return _repository.Update(user);
    // }


    public bool AddUser(AdduserViewModel model)
    {
        if (model != null)
        {


            var hashed = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var user = new User
            {
                Email = model.Email,
                UserRole = model.Role,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Password = hashed,
                CountryId = model.Country,
                StateId = model.State,
                CityId = model.City,
                Phone = model.Phone,
                Address = model.Address,
                ZipCode = model.ZipCode,
                Status = model.Status,
                ImgUrl = model.Imgurl
            };

            return _repository.Add(user);
        }
        return false;
    }

    public bool DeleteUser(string Email)
    {
        var user = _repository.GetAllByEmail(Email);
        if (user == null)
        {
            return false;
        }

        return _repository.Delete(user);
    }

    public async Task<PaginationViewModel<UserlistViewModel>> GetUserList(int page, int pageSize, string search = "", string sortColumn = "", string sortOrder = "")
    {


        List<User> userList = _repository.GetAllUser();
        List<UserlistViewModel> UserListViews = new List<UserlistViewModel>();
        int tableCount;

        foreach (User user in userList)
        {
            var role = user.UserRole.HasValue ? _repository.GetRole(user.UserId) : null;
            UserListViews.Add(new UserlistViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Imgurl = user.ImgUrl,
                Email = user.Email,
                Phone = user.Phone?.ToString(),
                Status = user.Status ?? false,
                RoleName = role?.RoleName,
            });
        }
        if (!string.IsNullOrEmpty(search))
        {
            UserListViews = UserListViews.Where(u => u.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (!string.IsNullOrEmpty(sortColumn))
        {
            if (sortColumn.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                if (sortOrder == "asc")
                    UserListViews = UserListViews.OrderBy(u => u.FirstName).ToList();
                else
                    UserListViews = UserListViews.OrderByDescending(u => u.FirstName).ToList();
            }
            else if (sortColumn.Equals("Role", StringComparison.OrdinalIgnoreCase))
            {
                if (sortOrder == "asc")
                    UserListViews = UserListViews.OrderBy(u => u.RoleName).ToList();
                else
                    UserListViews = UserListViews.OrderByDescending(u => u.RoleName).ToList();
            }
        }


        tableCount = UserListViews.Count();
        UserListViews = UserListViews.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PaginationViewModel<UserlistViewModel>
        {
            Items = UserListViews,
            TotalItems = tableCount,
            CurrentPage = page,
            PageSize = pageSize,
            sortOrder = sortOrder,
            sortColumn = sortColumn,
        };
    }

}
