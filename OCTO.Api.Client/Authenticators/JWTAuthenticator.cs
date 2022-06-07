using RestSharp;
using RestSharp.Authenticators;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using OCTO.Api.Client;

namespace OCTO.Api.Client.Authenticators
{
    public class JWTAuthenticator : IAuthenticator
    {
        private Keychain keychain;

        public JWTAuthenticator(Keychain Keychain)
        {
            this.keychain = Keychain;
        }

        private string CreateSignature(IRestClient client, IRestRequest request)
        {
            var symmetricKey = Convert.FromBase64String(keychain.Token);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                           // new Claim("TOKEN", keychain.Token),
                            new Claim(ClaimTypes.Hash, keychain.Key)
                        }),

                Expires = now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            string signature = CreateSignature(client, request);
            AddAuthorizationHeader(request, signature);
        }

        private void AddAuthorizationHeader(IRestRequest request, string signature)
        {
            request.Parameters.RemoveAll(p => p.Type == ParameterType.HttpHeader && p.Name == "Authorization");

            request.AddParameter("Authorization", string.Format("Bearer {0}", signature), ParameterType.HttpHeader);
        }
    }
}
