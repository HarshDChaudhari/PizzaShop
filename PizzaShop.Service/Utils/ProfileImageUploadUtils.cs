using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
namespace PizzaShop.Service.Utils;

public class ProfileImageUploadUtils
{
    private static readonly string _profileImagePath = Path.Combine("wwwroot", "images", "profile");
    public static async Task<string> SaveProfileImageUploadAsync(IFormFile file)
    {

        var profileImagePath = Path.Combine(Directory.GetCurrentDirectory(), _profileImagePath);
        if (!Directory.Exists(profileImagePath))
        {
            Directory.CreateDirectory(profileImagePath);
        }

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(profileImagePath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return the relative path to the file
        return Path.Combine("images", "profile", fileName);
    }

    private static readonly string _CategoryAndModifierProfileImagesPath = Path.Combine("wwwroot", "images", "CategoryAndModifierProfileImages");
    public static async Task<string> SaveCategoryAndModifierProfileImageUploadAsync(IFormFile file)
    {

        var CatAndModImagePath = Path.Combine(Directory.GetCurrentDirectory(), _CategoryAndModifierProfileImagesPath);
        if (!Directory.Exists(CatAndModImagePath))
        {
            Directory.CreateDirectory(CatAndModImagePath);
        }

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(CatAndModImagePath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return the relative path to the file
        return Path.Combine("images", "CategoryAndModifierProfileImages", fileName);
    }
}

