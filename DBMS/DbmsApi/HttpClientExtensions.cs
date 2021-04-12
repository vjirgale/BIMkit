using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DbmsApi
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Create patch requests
        /// </summary>
        /// <remarks>Courtesy of https://stackoverflow.com/a/29772349 </remarks>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="iContent"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent iContent)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = iContent
            };

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await client.SendAsync(request);
            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine("ERROR: " + e.ToString());
            }

            return response;
        }
        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T content)
        {
            return await PatchAsync(client, new Uri(client.BaseAddress + requestUri), new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"));
        }
    }
}