using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OCTO.Api.Adapter.Exceptions;
using OCTO.Api.Client.Authenticators;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OCTO.Api.Client
{
    public class ApiRepository<T> where T : class, new()
    {
        private Keychain keychain { get; set; }
        private IRestClient client;

        public IDictionary<string, object> Parameters { get; private set; } = new Dictionary<string, object>();
        public IDictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();

        private RestRequest APIRequest(Method method = Method.GET, string resourceName = "")
        {
            var req = new RestRequest(resourceName, method);

            if (Headers.Count != 0)
            {
                foreach (var item in Headers)
                {
                    req.AddHeader(item.Key, item.Value);
                }
            }

            return req;
        }

        public ApiRepository(Keychain keychain, string uri, AuthType authType)
        {
            this.keychain = keychain;

            client = new RestClient(uri)
            {
                Authenticator = GetAuthenticator(authType),
            };
        }

        private IAuthenticator GetAuthenticator(AuthType authType)
        {
            if (authType == AuthType.Basic)
                return new BasicAuthenticator(keychain);
            else if (authType == AuthType.HMAC)
                return new HmacAuthenticator(keychain);
            else
                return new JWTAuthenticator(keychain);
        }

        /// <summary>
        /// Executa uma operação de GET que retorna uma coleção de objetos
        /// </summary>
        /// <param name="resourceName">Nome da action a ser requisitada [uri/resource]</param>
        /// <returns>Coleção de objetos do tipo T</returns>
        public async Task<IEnumerable<T>> FindAsync(string resourceName = "")
        {
            return await FindAnonymousAsync<T>(resourceName);
        }

        /// <summary>
        /// Executa uma operação de GET que retorna uma coleção de objetos
        /// </summary>
        /// <param name="resourceName">Nome da action a ser requisitada [uri/resource]</param>
        /// <returns>Coleção de objetos do tipo T</returns>
        public async Task<IEnumerable<TRetorno>> FindAnonymousAsync<TRetorno>(string resourceName = "") where TRetorno : class
        {
            var request = APIRequest(Method.GET, resourceName);

            SetParameters(ref request);

            IRestResponse<List<TRetorno>> response = await client.ExecuteAsync<List<TRetorno>>(request);
            ResponseHandler(response);
            return response.Data;
        }

        /// <summary>
        /// Executa uma operação de GET que retorna o objeto pelo ID
        /// </summary> 
        /// <param name="id">ID do registro a ser procurado [uri/{id}]</param>
        /// <returns>Objeto do tipo T com o ID requisitado</returns>
        public async Task<T> GetByIdAsync<Tid>(Tid id)
        {
            return await GetByIdAnonymousAsync<T, Tid>(id);
        }

        /// <summary>
        /// Executa uma operação de GET que retorna o objeto pelo ID
        /// </summary> 
        /// <param name="id">ID do registro a ser procurado [uri/{id}]</param>
        /// <returns>Objeto do tipo T com o ID requisitado</returns>
        public async Task<TRetorno> GetByIdAnonymousAsync<TRetorno, Tid>(Tid id) where TRetorno : class
        {
            var request = APIRequest(Method.GET, "{id}");
            request.AddUrlSegment("id", id);

            SetParameters(ref request);

            IRestResponse<TRetorno> response = await client.ExecuteAsync<TRetorno>(request);
            ResponseHandler(response);

            return response.Data;
        }

        /// <summary>
        /// Executa uma operação de GET que retorna o objeto pelo ID
        /// </summary> 
        /// <param name="resourceName">Nome da action a ser requisitada [uri/resourceName]</param>
        /// <returns>Objeto do tipo T com o ID requisitado</returns>
        public async Task<T> Get(string resourceName = "")
        {
            return await GetAnonymous<T>(resourceName);
        }
        
        /// <summary>
        /// Executa uma operação de GET que retorna o objeto pelo ID
        /// </summary> 
        /// <param name="resourceName">Nome da action a ser requisitada [uri/resourceName]</param>
        /// <returns>Objeto do tipo T com o ID requisitado</returns>
        public async Task<TRetorno> GetAnonymous<TRetorno>(string resourceName = "") where TRetorno : class
        {
            var request = APIRequest(Method.GET, resourceName);

            SetParameters(ref request);

            IRestResponse<TRetorno> response = await client.ExecuteAsync<TRetorno>(request);
            ResponseHandler(response);

            return response.Data;
        }

        /// <summary>
        /// Executa uma operação de POST
        /// </summary>
        /// <param name="obj">Objeto a ser enviado na requisição</param>
        /// <param name="resourceName">Nome da action a ser requisitada [uri/resourceName]</param>
        /// <returns></returns>
        public async Task<T> Post(T obj, string resourceName = "")
        {
            return await PostAnonymous<T, T>(obj, resourceName);
        }

        /// <summary>
        /// Executa uma operação de POST
        /// </summary>
        /// <param name="obj">Objeto a ser enviado na requisição</param>
        /// <param name="resourceName">Nome da action a ser requisitada [uri/resourceName]</param>
        /// <returns></returns>
        public async Task<TRetorno> PostAnonymous<TEnvio, TRetorno>(TEnvio obj, string resourceName = "") 
            where TEnvio : class 
            where TRetorno : class
        {
            var request = APIRequest(Method.POST, resourceName);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Accept", "application/json");

            string body = JsonConvert.SerializeObject(obj,
                        new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.ffff" });
            request.AddJsonBody(body);
            SetParameters(ref request);

            IRestResponse<TRetorno> response = await client.ExecuteAsync<TRetorno>(request);

            ResponseHandler(response);

            return response.Data;
        } 

        /// <summary>
        /// Executa uma operação de PUT
        /// </summary>
        /// <param name="id">ID do registro a ser atualizado</param>
        /// <param name="obj">Objeto a ser atualizado</param>
        /// <returns></returns>
        public async Task<T> Put(string id, T obj)
        {
            return await PutAnonymous<T, T>(id, obj);
        }

        /// <summary>
        /// Executa uma operação de PUT sem um corpo na requisição
        /// </summary>
        /// <param name="resourceName">Nome da action a ser requisitada [uri/resourceName]</param>
        public async Task PutAnonymous(string resourceName = "")
        {
            var request = APIRequest(Method.PUT, resourceName);
            request.AddHeader("Content-type", "application/json");

            SetParameters(ref request);

            IRestResponse response = await client.ExecuteAsync(request);

            ResponseHandler(response);
        }

        /// <summary>
        /// Executa uma operação de PUT
        /// </summary>
        /// <param name="id">ID do registro a ser atualizado</param>
        /// <param name="obj">Objeto a ser atualizado</param>
        /// <returns></returns>
        public async Task<TRetorno> PutAnonymous<TEnvio, TRetorno>(string id, TEnvio obj)
            where TEnvio : class
            where TRetorno : class
        {
            var request = APIRequest(Method.PUT, "{id}");

            request.AddHeader("Content-type", "application/json");
            request.AddUrlSegment("id", id);

            string body = JsonConvert.SerializeObject(obj,
                        new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.ffff" });
            request.AddJsonBody(body);

            SetParameters(ref request);

            IRestResponse<TRetorno> response = await client.ExecuteAsync<TRetorno>(request);

            ResponseHandler(response);

            return response.Data;
        }

        /// <summary>
        /// Executa uma operação de PUT
        /// </summary>
        /// <param name="resourceName">Nome da action requisitada [uri/resourceName]</param>
        /// <returns></returns>
        public async Task<T> Put(string resourceName = "")
        {
            return await PutAnonymous<T, T>(resourceName);
        }

        /// <summary>
        /// Executa uma operação de PUT
        /// </summary>
        /// <param name="resourceName">Nome da action requisitada [uri/resourceName]</param>
        /// <returns></returns>
        public async Task<TRetorno> PutAnonymous<TEnvio, TRetorno>(string resourceName = "") 
            where TEnvio : class
            where TRetorno : class
        {
            var request = APIRequest(Method.PUT, resourceName);

            SetParameters(ref request);

            IRestResponse<TRetorno> response = await client.ExecuteAsync<TRetorno>(request);

            ResponseHandler(response);

            return response.Data;
        }

        private void SetParameters(ref RestRequest request)
        {
            if (Parameters.Count != 0)
            {
                foreach (var item in Parameters)
                {
                    request.AddParameter(item.Key, item.Value, ParameterType.QueryStringWithoutEncode);
                }
            }
        }

        private void ResponseHandler(IRestResponse response)
        {
            ResponseMessage responseMessage = null;
            string messageException = "";

            if (!response.IsSuccessful)
            {
                if (!string.IsNullOrEmpty(response.Content))
                    responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(response.Content);

                messageException = responseMessage?.Message ?? response?.ErrorException?.Message ?? "";
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                case HttpStatusCode.Accepted:
                case HttpStatusCode.Found:
                case HttpStatusCode.NoContent:
                    return;
                //break;
                case HttpStatusCode.BadRequest:
                    throw new BadRequestException(messageException);
                //break;
                case HttpStatusCode.BadGateway:
                    throw new BadGatewayException(messageException);
                //break;
                case HttpStatusCode.NotFound:
                    throw new NotFoundException(messageException);
                //break;
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException(messageException);
                //break;
                default:
                    throw new UnhandledException(response.StatusCode.ToString(), response.ErrorException);
            }

        }
    }
}
