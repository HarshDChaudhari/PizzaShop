namespace PizzaShop.Web.MiddleWare;

public class JwtMiddleware(RequestDelegate _next)
{
    public async Task Invoke(HttpContext context){
        
        var accessToken = context.Request.Cookies["SuperSecretAuthToken"];

        if(!string.IsNullOrEmpty(accessToken)){

            context.Request.Headers.Append("Authorization", $"Bearer {accessToken}");

        }
        await _next(context);
    }

}

