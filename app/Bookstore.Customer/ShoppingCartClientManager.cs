using Microsoft.AspNetCore.Http;
using System;

namespace Bookstore.Customer
{
    public interface IShoppingCartClientManager
    {
        string GetShoppingCartClientId();
    }

    public class ShoppingCartClientManager : IShoppingCartClientManager
    {
        private const string CookieKey = "ShoppingCartId";

        private readonly HttpContext context;

        public ShoppingCartClientManager(IHttpContextAccessor httpContextAccessor)
        {
            context = httpContextAccessor.HttpContext;
        }

        public string GetShoppingCartClientId()
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddYears(1),
                Path = "/"
            };

            var shoppingCartClientId = context.Request.Cookies[CookieKey];

            if (string.IsNullOrWhiteSpace(shoppingCartClientId))
            {
                shoppingCartClientId = context.User.Identity.IsAuthenticated ? context.User.GetUserId() : Guid.NewGuid().ToString();
            }

            context.Response.Cookies.Append(CookieKey, shoppingCartClientId, cookieOptions);

            return shoppingCartClientId;
        }
    }
}
