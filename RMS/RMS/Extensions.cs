using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace RMS
{
    public static class Extensions
    {
        public static HttpResponseMessage CreateReasonResponse(this HttpRequestMessage request, HttpStatusCode code, string reasonPhrase)
        {
            HttpResponseMessage response = request.CreateResponse();
            response.StatusCode = code;
            response.ReasonPhrase = reasonPhrase;
            return response;
        }
    }
}