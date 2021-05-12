using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace DBMS
{
    public static class Extensions
    {
        public static HttpResponseMessage CreateResponseDBMS(this HttpRequestMessage request, HttpStatusCode code, object value)
        {
            HttpResponseMessage response = request.CreateResponse(code, value);
            response.ReasonPhrase = code != HttpStatusCode.OK ? (string)value : "Success";
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }
    }
}