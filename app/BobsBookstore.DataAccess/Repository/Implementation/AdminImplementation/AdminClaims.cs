using System.Collections.Generic;
using BobsBookstore.Models.AdminUser;

namespace BobsBookstore.DataAccess.Repository.Implementation.AdminImplementation
{
    public class AdminClaims
    {
       public AdminUser GetAdminClaims(IEnumerable<System.Security.Claims.Claim> Claims)
        {
            AdminUser ad = new AdminUser();
            foreach(var claim in Claims)
            {
                string type = claim.Type;
                switch (type)
                {
                    case "cognito:groups":ad.Group = claim.Value;
                        break;
                    case "cognito:username":ad.Username = claim.Value;
                        break;
                    case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress":
                        ad.Email = claim.Value;
                        break;
                    default: break;
                }
               
            }

            return ad;
        }
    }
}
