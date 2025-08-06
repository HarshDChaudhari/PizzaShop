using Microsoft.AspNetCore.Http;
using PizzaShop.Entity.Models;
using System.Text.Json;

namespace AuthenticationDemo.Utils
{
    public static class SessionUtils
    {
        public static void SetUser(HttpContext httpContext, User user)
        {
            if (user != null)
            {
                string userData = JsonSerializer.Serialize(user);
                httpContext.Session.SetString("UserData", userData);

                // Store simple string in Session
                httpContext.Session.SetString("UserId", user.UserId.ToString());
            }
        }

        public static User? GetUser(HttpContext httpContext)
        {
            // Check session first
            string? userData = httpContext.Session.GetString("UserData");

            if (string.IsNullOrEmpty(userData))
            {
                // If session is empty, check the cookie
                httpContext.Request.Cookies.TryGetValue("UserData", out userData);
            }

            return string.IsNullOrEmpty(userData) ? null : JsonSerializer.Deserialize<User>(userData);
        }
        public static void ClearSession(HttpContext httpContext) => httpContext.Session.Clear();

    }
}