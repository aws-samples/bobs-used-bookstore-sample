using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using BOBS_Backend.Models;
namespace BOBS_Backend.Controllers.LoginActions
{
    public class VerifyLogin
    {
        static string appPoolId = "us-west-2_T7YhegXAO";


        static string clientId = "5f2rnsqn8prl48cre728iqb5ut";
        static Amazon.RegionEndpoint Region = Amazon.RegionEndpoint.USWest2;

        public bool authenticate(string username, string password)
        {
            bool taskResult = signIn(username, password).Result;
            return taskResult;
        }
        private static async Task<bool> signIn(string username, string password)
        {

            AmazonCognitoIdentityProviderClient client =
                new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Region);
            CognitoUserPool pool = new CognitoUserPool(appPoolId, clientId, client);
            CognitoUser user = new CognitoUser(username, clientId, pool, client);
            InitiateSrpAuthRequest authReq = new InitiateSrpAuthRequest()
            {
                Password = password
            };
            AuthFlowResponse authF = null;
            try
            {
                authF = await user.StartWithSrpAuthAsync(authReq).ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {

                return false;
            }


        }

    }
}
