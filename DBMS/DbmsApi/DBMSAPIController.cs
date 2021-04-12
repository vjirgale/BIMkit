using DbmsApi.API;
using DbmsApi.Mongo;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace DbmsApi
{
    public class DBMSAPIController
    {
        #region Properties

        // Basic web client
        private static HttpClientHandler handler;
        private static HttpClient client;
        private static List<MediaTypeFormatter> mediaTypeFormatters = new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() { SerializerSettings = DBMSJsonSettings.JsonSerializerSettings } };

        public TokenData Token { get; private set; }

        #endregion

        /// <summary>
        /// Create new API controller
        /// </summary>
        /// <param name="address">Base server address</param>
        public DBMSAPIController(string address)
        {
            handler = new HttpClientHandler()
            {
                CookieContainer = new CookieContainer(),
                UseCookies = true,
            };
            client = new HttpClient(handler);

            // Set up http client
            SetBaseAddress(address);

            // Set request headers to json
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", " ");


            // Configure certificate verification
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                {
                    return true;
                }

                // TODO: at some point we should standardize on one certificate and put its hash here (Certificate -> details -> thumbprint in chrome)
                /*
                else if (cert.GetCertHashString() == "somehash")
                {
                    return true;
                }

                return false;
                */
                return true;
            };
        }

        /// <summary>
        /// Change the dbms base address
        /// </summary>
        /// <param name="address"></param>
        public void SetBaseAddress(string address)
        {
            client.BaseAddress = new Uri(address);
        }

        /// <summary>
        /// Sets the Session Token in the requests
        /// </summary>
        /// <param name="token"></param>
        public void SetSessionToken(TokenData token)
        {
            Token = token;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token == null ? " " : token.Id);
        }

        /// <summary>
        /// Call the asynchronous function and return the result. If the task times out or results in a <see cref="SocketException"/>, the execption is
        /// caught and the appropriate response is returned. 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> TryCatchFunctionAsync(Task<HttpResponseMessage> task)
        {
            try
            {
                return await TimeoutAfter(task);
            }
            catch (TimeoutException)
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.RequestTimeout, ReasonPhrase = "Server timed out." };
            }
            catch (HttpRequestException e)
            {
                if (e.GetBaseException().GetType() == typeof(SocketException))
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.ServiceUnavailable, ReasonPhrase = "Unable to establish connection with server." };
                else
                    throw e;
            }
        }

        /// <summary>
        /// Cancels the task if it hasn't completed in the given timeout
        /// </summary>
        /// <remarks>Modified from https://stackoverflow.com/a/22078975 </remarks>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<TResult> TimeoutAfter<TResult>(Task<TResult> task)
        {
            TimeSpan timeout = TimeSpan.FromSeconds(30);

            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;
                }
                else
                {
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }

        #region Login

        /// <summary>
        /// Send login request to server
        /// </summary>
        /// <param name="authModel"></param>
        /// <returns></returns>
        public async Task<APIResponse<TokenData>> LoginAsync(string username, string password)
        {
            return await LoginAsync(new AuthModel { Username = username, Password = password });
        }

        /// <summary>
        /// Send login request to server with credentials from <paramref name="authModel"/>
        /// </summary>
        /// <param name="authModel"></param>
        /// <returns></returns>
        public async Task<APIResponse<TokenData>> LoginAsync(AuthModel authModel)
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PostAsJsonAsync("login", authModel));

            if (response.IsSuccessStatusCode)
            {
                SetSessionToken(await response.Content.ReadAsAsync<TokenData>());
                return new APIResponse<TokenData>(response, await response.Content.ReadAsAsync<TokenData>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<TokenData>(response, default);
            }
        }

        /// <summary>
        /// Reset credentials
        /// </summary>
        public async Task<APIResponse<string>> Logout()
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.DeleteAsync("login"));

            SetSessionToken(null);
            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        #endregion

        #region models

        /// <summary>
        /// List the models avaiable to the user
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse<List<ModelMetadata>>> GetAvailableModels()
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("models"));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<List<ModelMetadata>>(response, await response.Content.ReadAsAsync<List<ModelMetadata>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<List<ModelMetadata>>(response, default);
            }
        }

        /// <summary>
        /// Fetch a model belonging to user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retreived <see cref="Model"/> will be null if not found</returns>
        public async Task<APIResponse<Model>> GetModel(ItemRequest request)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("models/" + request.ItemId + "/" + request.LOD.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<Model>(response, await response.Content.ReadAsAsync<Model>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<Model>(response, default);
            }
        }

        /// <summary>
        /// Create a new model on the users account. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns><see cref="APIResponse"/> containing the new models Id</returns>
        public async Task<APIResponse<string>> CreateModel(Model model)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PostAsJsonAsync("models", model));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        /// <summary>
        /// Update a model belonging to the user by id. Success of update will be indicated by response code.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<APIResponse<string>> UpdateModel(Model model)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PutAsJsonAsync("models", model));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        /// <summary>
        /// Delete a model belonging to the user by id. Success of update will be indicated by response code.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponse<string>> DeleteModel(string id)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.DeleteAsync("models/" + id));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        #endregion

        #region Catalog

        /// <summary>
        /// List the catalog objects (doesn't require any authentication)
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse<List<CatalogObjectMetadata>>> GetAvailableCatalogObjects()
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("catalogobjects"));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<List<CatalogObjectMetadata>>(response, await response.Content.ReadAsAsync<List<CatalogObjectMetadata>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<List<CatalogObjectMetadata>>(response, default);
            }
        }

        /// <summary>
        /// Fetch a catalog object by id (doesn't require any authentication
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retreived <see cref="CatalogObject"/> will be null if not found</returns>
        public async Task<APIResponse<MongoCatalogObject>> GetCatalogObject(string id)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("catalogobjects/" + id));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<MongoCatalogObject>(response, await response.Content.ReadAsAsync<MongoCatalogObject>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<MongoCatalogObject>(response, default);
            }
        }

        /// <summary>
        /// Fetch a catalog object by id (doesn't require any authentication
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retreived <see cref="CatalogObject"/> will be null if not found</returns>
        public async Task<APIResponse<CatalogObject>> GetCatalogObject(ItemRequest request)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("catalogobjects/" + request.ItemId + "/" + request.LOD.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<CatalogObject>(response, await response.Content.ReadAsAsync<CatalogObject>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<CatalogObject>(response, default);
            }
        }

        /// <summary>
        /// Create a new catalog object
        /// </summary>
        /// <param name="catalogObject"></param>
        /// <returns><see cref="APIResponse"/> containing the new models Id</returns>
        public async Task<APIResponse<string>> CreateCatalogObject(MongoCatalogObject catalogObject)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PostAsJsonAsync("catalogobjects", catalogObject));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        /// <summary>
        /// Update a catalog object. Success of update will be indicated by response code.
        /// </summary>
        /// <param name="catalogObject"></param>
        /// <returns></returns>
        public async Task<APIResponse<string>> UpdateCatalogObject(MongoCatalogObject catalogObject)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PutAsJsonAsync("catalogobjects/", catalogObject));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        /// <summary>
        /// Update the metadata (Name & properties) of a catalog object
        /// </summary>
        /// <param name="availableCatalogObject"></param>
        /// <returns></returns>
        public async Task<APIResponse<string>> UpdateCatalogObjectMetaData(CatalogObjectMetadata availableCatalogObject)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PatchAsJsonAsync("catalogobjects/", availableCatalogObject));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }

        }

        /// <summary>
        /// Delete a catalog object by id. Success of update will be indicated by response code.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponse<string>> DeleteCatalogObject(string id)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.DeleteAsync("catalogobjects/" + id));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        #endregion

        #region Material

        /// <summary>
        /// Retrieve a material by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponse<Material>> GetMaterial(string id)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("materials/" + id));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<Material>(response, await response.Content.ReadAsAsync<Material>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<Material>(response, default);
            }
        }

        /// <summary>
        /// Retrieve all materials
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponse<List<Material>>> GetMaterials()
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("materials"));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<List<Material>>(response, await response.Content.ReadAsAsync<List<Material>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<List<Material>>(response, default);
            }
        }

        /// <summary>
        /// Create a new material 
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public async Task<APIResponse<string>> CreateMaterial(Material material)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PostAsJsonAsync("materials", material));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        /// <summary>
        /// Update an existing material
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public async Task<APIResponse<string>> UpdateMaterial(Material material)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PutAsJsonAsync("materials", material));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        /// <summary>
        /// Delete an existing material
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponse<string>> DeleteMaterial(string id)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.DeleteAsync("materials/" + id));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        #endregion

        #region Users

        /// <summary>
        /// Get Information about the logged in user (based on session token)
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse<List<string>>> GetUserNames()
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("users"));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<List<string>>(response, await response.Content.ReadAsAsync<List<string>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<List<string>>(response, default);
            }
        }

        /// <summary>
        /// Get Information about the logged in user (based on session token)
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse<UserData>> GetUserData(string username)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("users/" + username));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<UserData>(response, await response.Content.ReadAsAsync<UserData>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<UserData>(response, default);
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<APIResponse<UserData>> CreateUser(NewUser user)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PostAsJsonAsync("users", user));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<UserData>(response, await response.Content.ReadAsAsync<UserData>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<UserData>(response, default);
            }
        }

        /// <summary>
        /// Update the user with username==<paramref name="id"/> with the supplied <see cref="UserData"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<APIResponse<string>> UpdateUserData(UserData user)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PutAsJsonAsync("users", user));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<APIResponse<string>> DeleteUser(string username)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.DeleteAsync("users/" + username));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        #endregion

        #region ModelPermissions

        /// <summary>
        /// Get all models (admin only)
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse<List<ModelMetadata>>> GetAllModels()
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("modelpermissions"));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<List<ModelMetadata>>(response, await response.Content.ReadAsAsync<List<ModelMetadata>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<List<ModelMetadata>>(response, default);
            }
        }

        /// <summary>
        /// Get the model permissions for a model
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public async Task<APIResponse<ModelPermission>> GetModelPermissions(string modelId)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("modelpermissions/" + modelId));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<ModelPermission>(response, await response.Content.ReadAsAsync<ModelPermission>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<ModelPermission>(response, default);
            }
        }

        /// <summary>
        /// Update the access permissions for a model. User must either own
        /// the model or be an admin.
        /// </summary>
        /// <param name="ownershipModel"></param>
        /// <returns></returns>
        public async Task<APIResponse<string>> ConfigureModelPermissions(ModelPermission ownershipModel)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PutAsJsonAsync("modelpermissions", ownershipModel));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        #endregion

        #region Admin

        /// <summary>
        /// Get a list of <see cref="UserData"/> for all users
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse<List<UserData>>> GetUserList()
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("admin"));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<List<UserData>>(response, await response.Content.ReadAsAsync<List<UserData>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<List<UserData>>(response, default);
            }
        }

        /// <summary>
        /// Get the models available to a particular user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<APIResponse<List<ModelMetadata>>> GetAvailableModels(string username)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("admin/" + username));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<List<ModelMetadata>>(response, await response.Content.ReadAsAsync<List<ModelMetadata>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<List<ModelMetadata>>(response, default);
            }
        }

        /// <summary>
        /// Update user password based on the contents of the supplied <see cref="AuthModel"/>
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        public async Task<APIResponse<string>> UpdateUserPassword(AuthModel auth)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PutAsJsonAsync("admin", auth));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }

        #endregion

        #region

        /// <summary>
        /// Gets a list of the available types as strings
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse<List<ObjectTypes>>> GetTypesList()
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("type"));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<List<ObjectTypes>>(response, await response.Content.ReadAsAsync<List<ObjectTypes>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<List<ObjectTypes>>(response, default);
            }
        }

        #endregion
    }
}