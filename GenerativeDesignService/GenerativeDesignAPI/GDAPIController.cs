using DbmsApi;
using RuleAPI.Models;
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

namespace GenerativeDesignAPI
{
    public class GDAPIController
    {
        // Basic web client
        private static HttpClientHandler handler;
        private static HttpClient client;
        private static List<MediaTypeFormatter> mediaTypeFormatters = new List<MediaTypeFormatter>() 
        {
            new JsonMediaTypeFormatter() { SerializerSettings = RuleJsonSettings.JsonSerializerSettings } 
        };

        public double TIMEOUT = 120.0;

        /// <summary>
        /// Create new API controller
        /// </summary>
        /// <param name="address">Base server address</param>
        public GDAPIController(string address)
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

        // Only Operation:
        
        public async Task<APIResponse<string>> PerformGenDesign(GenerativeRequest request)
        {
            HttpResponseMessage response = await TryCatchFunctionAsync(client.PostAsJsonAsync("generate", request));

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse<string>(response, await response.Content.ReadAsAsync<string>(mediaTypeFormatters));
            }
            else
            {
                response.ReasonPhrase = await response.Content.ReadAsAsync<string>();
                return new APIResponse<string>(response, default);
            }
        }
    }
}