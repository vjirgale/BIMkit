using DbmsApi;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace RuleAPI
{
    public class RuleAPIController
    {
        // Basic web client
        static HttpClientHandler handler = new HttpClientHandler();
        static HttpClient client = new HttpClient(handler);
        static List<MediaTypeFormatter> mediaTypeFormatters = new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() { SerializerSettings = RuleJsonSettings.JsonSerializerSettings } };
        public RuleUser CurrentUser;

        public double TIMEOUT = 30.0;

        public RuleAPIController(string address)
        {
            client.BaseAddress = new Uri(address);

            // Set request headers to json
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetSessionUser(RuleUser user)
        {
            CurrentUser = user;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user == null ? "" : user.Username);
        }

        public void SetSessionUser(string user)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user == null ? "" : user);
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
            TimeSpan timeout = TimeSpan.FromSeconds(TIMEOUT);

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

        #region User Methods:

        public async Task<APIResponse<RuleUser>> LoginAsync(string username)
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("user/" + username));

            if (response.IsSuccessStatusCode)
            {
                RuleUser user = await response.Content.ReadAsAsync<RuleUser>();
                SetSessionUser(user);
                return new APIResponse<RuleUser>(response, user);
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<RuleUser>(response, default);
            }
        }

        public async Task<APIResponse<RuleUser>> CreateUserAsync(RuleUser newUser)
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PostAsJsonAsync("user", newUser));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<RuleUser>(response, await response.Content.ReadAsAsync<RuleUser>());
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<RuleUser>(response, default);
            }
        }

        public async Task<APIResponse> UpdateUserAsync(string oldUsernamem, RuleUser newUser)
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PutAsJsonAsync("user/" + oldUsernamem, newUser));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse(response);
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse(response);
            }
        }

        public void Logout()
        {
            RuleUser temp = null;
            SetSessionUser(temp);
        }

        #endregion

        #region Rule Methods:

        public async Task<APIResponse<Rule>> GetRuleAsync(string ruleId)
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("rule/" + ruleId));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<Rule>(response, await response.Content.ReadAsAsync<Rule>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<Rule>(response, default);
            }
        }

        public async Task<APIResponse<List<Rule>>> GetAllRulesAsync()
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("rule/all"));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<List<Rule>>(response, await response.Content.ReadAsAsync<List<Rule>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<List<Rule>>(response, default);
            }
        }

        public async Task<APIResponse<string>> CreateRuleAsync(Rule newRule)
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PostAsJsonAsync("rule", newRule));

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

        public async Task<APIResponse<string>> UpdateRuleAsync(Rule rule)
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PutAsJsonAsync("rule", rule));

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

        public async Task<APIResponse<string>> DeleteRuleAsync(string ruleId)
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.DeleteAsync("rule/" + ruleId));

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

        #region RuleSet Methods:

        public async Task<APIResponse<RuleSet>> GetRuleSetAsync(string ruleSetId)
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("ruleset/" + ruleSetId));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<RuleSet>(response, await response.Content.ReadAsAsync<RuleSet>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<RuleSet>(response, default);
            }
        }

        public async Task<APIResponse<List<RuleSet>>> GetAllRuleSetsAsync()
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("ruleset/all"));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<List<RuleSet>>(response, await response.Content.ReadAsAsync<List<RuleSet>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<List<RuleSet>>(response, default);
            }
        }

        public async Task<APIResponse<string>> CreateRuleSetAsync(RuleSet newRuleSet)
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PostAsJsonAsync("ruleset", newRuleSet));

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

        public async Task<APIResponse<string>> UpdateRuleSetAsync(RuleSet ruleSet)
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PutAsJsonAsync("ruleset", ruleSet));

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

        public async Task<APIResponse<string>> DeleteRuleSetAsync(string ruleSetId)
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.DeleteAsync("ruleset/" + ruleSetId));

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

        #region VO/Relation/Properties Methods:

        public async Task<APIResponse<Dictionary<ObjectTypes, string>>> GetVOMethodsAsync()
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("method/vo"));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<Dictionary<ObjectTypes, string>>(response, await response.Content.ReadAsAsync<Dictionary<ObjectTypes, string>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<Dictionary<ObjectTypes, string>>(response, default);
            }
        }

        public async Task<APIResponse<List<ObjectTypes>>> GetTypesList()
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("method/type"));

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

        public async Task<APIResponse<Dictionary<string, Type>>> GetPropertyMethodsAsync()
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("method/property"));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<Dictionary<string, Type>>(response, await response.Content.ReadAsAsync<Dictionary<string, Type>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<Dictionary<string, Type>>(response, default);
            }
        }

        public async Task<APIResponse<Dictionary<string, Type>>> GetRelationMethodsAsync()
        {
            // Perform POST request
            HttpResponseMessage response = await TryCatchFunctionAsync(client.GetAsync("method/relation"));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<Dictionary<string, Type>>(response, await response.Content.ReadAsAsync<Dictionary<string, Type>>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<Dictionary<string, Type>>(response, default);
            }
        }

        #endregion
    }
}