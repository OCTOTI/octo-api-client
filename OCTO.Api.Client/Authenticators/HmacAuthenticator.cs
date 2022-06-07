using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OCTO.Api.Client.Helpers;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Security.Cryptography;
using System.Text;

namespace OCTO.Api.Client.Authenticators
{
    public class HmacAuthenticator : IAuthenticator
    {
        private const string authorizationScheme = "amx";

        private Keychain keychain;
        private string CreateSignature(IRestClient client, IRestRequest request) {

            string requestContentBase64String = string.Empty;

            string requestUri = System.Web.HttpUtility.UrlEncode(client.BuildUri(request).AbsoluteUri.ToLower());
            string requestApi = request.Method.ToString().ToUpperInvariant().ToLower();

            //Calculate UNIX time
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            //create random nonce for each request
            string nonce = Guid.NewGuid().ToString("N");

            // Get content type
            Parameter bodyParameter = request.Parameters.GetBodyParameter(client.DefaultParameters);
            if (bodyParameter != null)
            {
                byte[] content = GetBodyBytes(client, request);                
                MD5 md5 = MD5.Create();
                byte[] requestContentHash = md5.ComputeHash(content);
                requestContentBase64String = Convert.ToBase64String(requestContentHash);                
            }
                //Creating the raw signature string
            string signatureRawData = String.Format("{0}{1}{2}{3}{4}{5}", keychain.Token, requestApi, requestUri, requestTimeStamp, nonce, requestContentBase64String);

            var secretKeyByteArray = Convert.FromBase64String(keychain.Key);

            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

            string retorno = String.Empty;

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);                
                retorno = string.Format("{0}:{1}:{2}:{3}", keychain.Token, requestSignatureBase64String, nonce, requestTimeStamp);
            }

            return retorno;
        }        

        protected virtual byte[] GetBodyBytes(IRestClient client, IRestRequest request)
        {
            byte[] bodyBytes = null;

            Parameter bodyParameter = request.Parameters.GetBodyParameter(client.DefaultParameters);

            if (bodyParameter != null)
            {                
                if (bodyParameter.Value != null)
                {
                    Encoding encoding = client.Encoding ?? Encoding.UTF8;
                    string body = bodyParameter.Value as string;
                    bodyBytes = encoding.GetBytes(body);
                }
            }

            return bodyBytes;
        }

        private void AddAuthorizationHeader(IRestRequest request, string signature)
        {            
            request.Parameters.RemoveAll(p => p.Type == ParameterType.HttpHeader && p.Name == "Authorization");

            request.AddParameter("Authorization", $"{authorizationScheme} {signature}", ParameterType.HttpHeader);
        }

        public HmacAuthenticator(Keychain keychain)
        {
            this.keychain = keychain;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            string signature = CreateSignature(client, request);
            AddAuthorizationHeader(request, signature);
        }
    }
}
