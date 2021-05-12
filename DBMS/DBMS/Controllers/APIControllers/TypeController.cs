using DBMS.Controllers.DBControllers;
using DBMS.Filters;
using DbmsApi;
using DbmsApi.API;
using DbmsApi.Mongo;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBMS.Controllers.APIControllers
{
    public class TypeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            List<ObjectTypes> listOfTypes = GetTypesRecusrive(ObjectTypeTree.Root);

            return Request.CreateResponseDBMS(HttpStatusCode.OK, listOfTypes);
        }

        private List<ObjectTypes> GetTypesRecusrive(ObjectType type)
        {
            if (type.Children.Count == 0)
            {
                return new List<ObjectTypes>() { type.ID };
            }

            List<ObjectTypes> types = new List<ObjectTypes>();
            foreach (var child in type.Children)
            {
                types.AddRange(GetTypesRecusrive(child));
            }
            return types;
        }
    }
}