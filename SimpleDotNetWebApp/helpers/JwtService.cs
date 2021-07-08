using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;



namespace SimpleDotNetWebApp.helpers
{
    public class JwtService
    {
        /* create a secure key for the hashing algorithm */
        private string _secureKey = "this is a very very Secure KEY";

        /***
         * used to generate a secure cookie
         * pass it userId
         * rreturn a cookie string
         * */
        public string Generate(int userId)
        {
            var symmetricSecureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secureKey));
            var credentials = new SigningCredentials(symmetricSecureKey, SecurityAlgorithms.HmacSha256Signature);

            var header = new JwtHeader(credentials);

            //  the token will be valid for one day only
            var payLoad = new JwtPayload(userId.ToString(), null, null, null, DateTime.Today.AddDays(1));

            // sec
            var securityToken = new JwtSecurityToken(header, payLoad);

            // return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(securityToken)    ;
        }

        /***
         * Decode request cookie to verify an authenticated user
         * 
         * */

        public JwtSecurityToken Verify(string jwtCookie)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secureKey);

            tokenHandler.ValidateToken(jwtCookie, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
            },
            out SecurityToken validatedToken

            );


            return (JwtSecurityToken) validatedToken;
        }

    }
}
