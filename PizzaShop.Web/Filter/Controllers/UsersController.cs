using System;
using System.Diagnostics;
using AuthenticationDemo.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Service.Implementations;
using PizzaShop.Service.Interfaces;
using PizzaShop.Service.Utils;


namespace PizzaShop.Web.Controllers;

[ServiceFilter(typeof(PermissionFilter))]
public class UsersController(IUserService _userService, IJwtService _jwtService, ISendmailService _sendmail) : Controller
{

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        try
        {

            // var principal = _jwtService.ValidateToken(Request.Cookies["SuperSecretAuthToken"]);
            // if (principal == null)
            // {
            //     return RedirectToAction("Login", "Validation");
            // }
            // int UserId = int.Parse(principal.FindFirst("UserId")?.Value ?? "0");
            var User = SessionUtils.GetUser(HttpContext);
            var UserId = User.UserId;
            if (UserId == 0)
            {
                return BadRequest("User ID is missing.");
            }
            var user = _userService.GetUser(UserId.ToString());
            return View(user);

        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }

    [HttpPost]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        try
        {
            if (model.FormFile != null && model.FormFile.Length > 0)
            {
                model.Imgurl = await ProfileImageUploadUtils.SaveProfileImageUploadAsync(model.FormFile);
            }


            // var principal = _jwtService.ValidateToken(Request.Cookies["SuperSecretAuthToken"]);
            // if (principal == null)
            // {
            //     return RedirectToAction("Login", "Validation");
            // }
            // int UserId = int.Parse(principal.FindFirst("UserId")?.Value ?? "0");

            var User = SessionUtils.GetUser(HttpContext);
            var UserId = User.UserId;



            if (UserId == 0)
            {
                return BadRequest("User ID is missing.");
            }
            model.Id = UserId;
            var user = _userService.GetUser(UserId.ToString());
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.Address = model.Address;
            user.ZipCode = model.ZipCode;
            user.Country = model.Country;
            user.State = model.State;
            user.City = model.City;
            user.Status = model.Status;
            if (model.Imgurl != null)
            {
                user.Imgurl = model.Imgurl;
            }
            _userService.UpdateUser(user);
            TempData["Success"] = "Profile Updated Successfully";
            return RedirectToAction("Profile");


            // user.ProfileImg = model.ProfileImg;
            // user.MyImage = model.MyImage;
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    public ActionResult GetCountries()
    {
        try
        {
            var countries = _userService.GetCountries();
            return Json(countries);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    public IActionResult GetRoles()
    {
        try
        {
            var roles = _userService.GetRoles();
            var abc = Json(roles);
            return abc;
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    public IActionResult GetStates(int CountryId)
    {
        try
        {
            var states = _userService.GetStates(CountryId);
            return Json(states);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    public IActionResult GetCities(int StateId)
    {
        try
        {
            var cities = _userService.GetCities(StateId);
            return Json(cities);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }


    [HttpGet]
    public IActionResult Users()
    {
        try
        {
            TempData["Active"] = "User";
            return View();

        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
    [HttpGet]
    public async Task<IActionResult> UserList(int page, int pageSize, string search = "", string sortColumn = "", string sortOrder = "asc")
    {

        try
        {
            PaginationViewModel<UserlistViewModel> table = await _userService.GetUserList(page, pageSize, search, sortColumn, sortOrder);

            return PartialView("./PartialView/_UserList", table);

        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }



    [HttpGet]
    public IActionResult AddUser()
    {
        TempData["Active"] = "User";
        return View();
    }

    [HttpPost]
    public IActionResult AddUser(AdduserViewModel model)
    {
        try
        {
            var check = _userService.GetAllUser().FirstOrDefault(x => x.Email == model.Email);
            if (check != null)
            {
                TempData["Error"] = "User Already Exists";
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            _userService.AddUser(model);

            Task task = _sendmail.SendMailUser(model.Email, model.Password);

            TempData["Success"] = "User Added Successfully";
            return RedirectToAction("Users");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString()); ;
        }
    }

    [HttpGet]
    public IActionResult EditUser(string Email)
    {
        try
        {
            TempData["Active"] = "User";
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (Email == null)
            {
                TempData["Error"] = "Email not Found";
                return View();
            }

            var user = _userService.GetUserByEmail(Email);

            return View(user);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(EdituserViewModel model)
    {
        try
        {
            if (model.FormFile != null && model.FormFile.Length > 0)
            {
                model.Imgurl = await ProfileImageUploadUtils.SaveProfileImageUploadAsync(model.FormFile);
            }

            var user = _userService.GetUserByEmail(model.Email);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Users");
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Username = model.Username;
            user.Status = model.Status;
            user.Country = model.Country;
            user.State = model.State;
            user.City = model.City;
            user.Phone = model.Phone;
            user.Address = model.Address;
            user.ZipCode = model.ZipCode;
            user.Imgurl = model.Imgurl;

            _userService.UpdateUserinEdit(user);

            TempData["Success"] = "User Updated Successfully";
            return RedirectToAction("Users");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            var referer = Request.Headers["Referer"].ToString();
            return Redirect(string.IsNullOrEmpty(referer) ? Url.Action("Users") : referer);
        }
    }


    [HttpPost]
    public IActionResult DeleteUser(string Email)
    {
        try
        {
            _userService.DeleteUser(Email);
            TempData["Success"] = "User Deleted Successfully";
            return RedirectToAction("Users");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }

}
