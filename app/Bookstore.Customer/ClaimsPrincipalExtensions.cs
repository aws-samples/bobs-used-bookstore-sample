using System.Security.Claims;

namespace Bookstore.Customer
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst("sub")?.Value;
        }
    }
}