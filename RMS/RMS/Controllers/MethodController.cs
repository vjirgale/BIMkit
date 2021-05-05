using RMS.Services;
using RuleAPI.Models;
using RuleAPI.Methods;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;
using System;

namespace RMS.Controllers
{
    public class MethodController : ApiController
    {
        /// <summary>
        /// Gets the property or relation methods
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string id)
        {
            Dictionary<string, Type> methods = new Dictionary<string, Type>();
            if (id == "property")
            {
                foreach (var kvp in MethodFinder.GetAllPropertyMethods())
                {
                    methods.Add(kvp.Key, kvp.Value);
                }
                return Request.CreateResponse(HttpStatusCode.OK, methods);
            }
            if (id == "relation")
            {
                foreach (var kvp in MethodFinder.GetAllRelationMethods())
                {
                    methods.Add(kvp.Key, kvp.Value);
                }
            }
            if (id == "vo")
            {
                return Request.CreateResponse(HttpStatusCode.OK, MethodFinder.GetAllVOMethods());
            }
            if (id == "type")
            {
                return Request.CreateResponse(HttpStatusCode.OK, MethodFinder.GetAllTypes());
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request");
        }
    }
}