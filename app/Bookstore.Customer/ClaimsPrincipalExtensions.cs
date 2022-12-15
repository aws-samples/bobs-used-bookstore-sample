using System.Security.Claims;

namespace Bookstore.Customer
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSub(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst("sub")?.Value;
        }
    }
}