using System.Security.Claims;

namespace Bookstore.Web.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSub(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst("sub")?.Value;
        }
    }
}