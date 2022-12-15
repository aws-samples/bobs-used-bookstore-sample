using Microsoft.AspNetCore.Http;
using System;

namespace Bookstore.Customer
{
    public interface IShoppingCartClientManager
    {
        string GetShoppingCartId();
    }

    public class ShoppingCartClientManager : IShoppingCartClientManager
    {
        private const string CookieKey = "ShoppingCartId";

        private readonly HttpContext context;

        public ShoppingCartClientManager(IHttpContextAccessor httpContextAccessor)
        {
            context = httpContextAccessor.HttpContext;
        }

        public string GetShoppingCartId()
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddYears(1),
                Path = "/"
            };

            var shoppingCartClientId = context.Request.Cookies[CookieKey];

            if (string.IsNullOrWhiteSpace(shoppingCartClientId))
            {
                shoppingCartClientId = context.User.Identity.IsAuthenticated ? context.User.GetSub() : Guid.NewGuid().ToString();
            }

            context.Response.Cookies.Append(CookieKey, shoppingCartClientId, cookieOptions);

            return shoppingCartClientId;
        }
    }
}
