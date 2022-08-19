using System.Collections.Generic;
using System.Security.Claims;
using DataModels.AdminUser;

namespace DataAccess.Repository.Implementation.AdminImplementation
{
    public class AdminClaims
    {
        public AdminUser GetAdminClaims(IEnumerable<Claim> Claims)
        {
            var ad = new AdminUser();
            foreach (var claim in Claims)
            {
                var type = claim.Type;
                switch (type)
                {
                    case "cognito:groups":
                        ad.Group = claim.Value;
                        break;
                    case "cognito:username":
                        ad.Username = claim.Value;
                        break;
                    case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress":
                        ad.Email = claim.Value;
                        break;
                }
            }

            return ad;
        }
    }
}