using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;

namespace BOBS_Backend.Controllers.LoginActions
{

    public class RegisterAdmin
    {
        static string appPoolId = "us-west-2_YiUZkX4bW";
        static string appPoolId2 = "us-west-2_T7YhegXAO";
        static string clientId = "4sdk4dai0hem8qlpjv229sfv1a";

        static string clientId2 = "5f2rnsqn8prl48cre728iqb5ut";
        static Amazon.RegionEndpoint Region = Amazon.RegionEndpoint.USWest2;

        public bool Register(string username, string password, string email)
        {
            bool result = signUp(username, password, email).Result;
            return result;
        }
        public async Task<bool> signUp(string username, string password, string email)
        {
            AmazonCognitoIdentityProviderClient client = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Region);
            SignUpRequest signupRequest = new SignUpRequest()
            {
                ClientId = clientId2,
                Username = username,
                Password = password
            };
            List<AttributeType> attributes = new List<AttributeType>()
            {
                new AttributeType(){Name="email",Value=email}

            };
            signupRequest.UserAttributes = attributes;
            try
            {
                SignUpResponse result = await client.SignUpAsync(signupRequest);
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }
    }


}
