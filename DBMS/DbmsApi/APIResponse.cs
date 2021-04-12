using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DbmsApi
{
    /// <summary>
    /// Simple wrapper for the important bits of http responses
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResponse<T>
    {
        public HttpStatusCode Code;
        public string ReasonPhrase;
        public T Data = default;

        public APIResponse() { }
        public APIResponse(HttpResponseMessage message, T Data)
        {
            this.Code = message.StatusCode;
            this.ReasonPhrase = message.ReasonPhrase;
            this.Data = Data;
        }
    }

    /// <summary>
    /// Defines an object-typed <see cref="APIResponse{T}"/> with null data,
    /// disguised as an un-typed APIResponse.
    /// </summary>
    public class APIResponse : APIResponse<object>
    {
        public APIResponse() : base() { }
        public APIResponse(HttpResponseMessage message, object Data = null) : base(message, Data) { }
    }
}