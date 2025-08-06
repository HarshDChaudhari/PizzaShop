using Microsoft.AspNetCore.Http;
using PizzaShop.Entity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace AuthenticationDemo.Utils
{
    public static class CookieUtils
    {
        public static void SaveJWTToken(HttpResponse response, string token)
        {
            response.Cookies.Append("SuperSecretAuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(3)
            });
        }

        public static string? GetJWTToken(HttpRequest request)
        {
            _ = request.Cookies.TryGetValue("SuperSecretAuthToken", out string? token);
            return token;
        }

        public static void SaveUserData(HttpResponse response, User user)
        {
            string userData = JsonSerializer.Serialize(user);

            // Store user details in a cookie for 3 days
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(3),
                HttpOnly = true,
                Secure = true,
                IsEssential = true
            };
            response.Cookies.Append("UserData", userData, cookieOptions);
        }

        // public static User? DecodeToken(string token)
        // {
        //     var handler = new JwtSecurityTokenHandler();
        //     var jwtToken = handler.ReadJwtToken(token);

        //     // Extract claims from token
        //     var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

        //     // Example assuming token contains user ID, name, role
        //     return new User
        //     {
        //         UserId = int.Parse(claims["id"]),
        //         UserRole = int.Parse(claims["role"])
        //     };
        // }

        public static User? DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId");
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");

            if (userIdClaim == null || roleClaim == null)
            {
                return null; // Or handle missing claims appropriately
            }

            return new User
            {
                UserId = int.Parse(userIdClaim.Value),
                UserRole = int.Parse(roleClaim.Value)
            };
        }



        public static void ClearCookies(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("SuperSecretAuthToken");
            httpContext.Response.Cookies.Delete("UserData");
        }
    }
}