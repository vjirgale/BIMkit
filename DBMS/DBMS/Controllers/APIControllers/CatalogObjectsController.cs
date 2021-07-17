using DBMS.Controllers.DBControllers;
using DBMS.Filters;
using DbmsApi.API;
using DbmsApi.Mongo;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;
using MathPackage;

namespace DBMS.Controllers.APIControllers
{
    public class CatalogObjectsController : ApiController
    {
        protected MongoDbController db;
        public CatalogObjectsController() { db = MongoDbController.Instance; }
        public CatalogObjectsController(MongoDbController db) { this.db = db; }

        public HttpResponseMessage Get()
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, db.RetrieveAvailableCatalogObjects());
        }

        public HttpResponseMessage Get(string id)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            MongoCatalogObject co = db.GetCatalogObject(id);
            if (co == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "No Catalog Object with that ID exists");
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, co);
        }

        public HttpResponseMessage Get(string id, string lod)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            LevelOfDetail levelOfDetail = LevelOfDetail.LOD100;
            try
            {
                levelOfDetail = (LevelOfDetail)Enum.Parse(typeof(LevelOfDetail), lod);
            }
            catch
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Missing Level of Detail");
            }

            MongoCatalogObject co = db.GetCatalogObject(id);
            if (co == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "No Catalog Object with that ID exists");
            }

            CatalogObject fullCatalogObj = new CatalogObject()
            {
                CatalogID = co.Id,
                Name = co.Name,
                Properties = co.Properties,
                TypeId = co.TypeId,
                Components = GetNextLOD(co.MeshReps, levelOfDetail)
            };
            return Request.CreateResponseDBMS(HttpStatusCode.OK, fullCatalogObj);
        }

        private List<Component> GetNextLOD(List<MeshRep> mreps, LevelOfDetail lod)
        {
            List<LevelOfDetail> lodList = Enum.GetValues(typeof(LevelOfDetail)).Cast<LevelOfDetail>().ToList();
            int startIndex = lodList.IndexOf(lod);
            for (int i = 0; i < lodList.Count; i++)
            {
                int index = (startIndex - i + lodList.Count) % lodList.Count;
                if (mreps.Any(m => m.LevelOfDetail == lod))
                {
                    return mreps.First(m => m.LevelOfDetail == lod).Components;
                }
            }

            return mreps.First().Components;
        }

        public HttpResponseMessage Post([FromBody] MongoCatalogObject catalogObject)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (!user.IsAdmin)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be an Admin");
            }

            if (catalogObject == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Missing CatalogObject");
            }

            catalogObject.Id = null;

            catalogObject.MeshReps = catalogObject.MeshReps.OrderBy(o => o.LevelOfDetail).ToList();
            catalogObject.MeshReps.Add(CreateBoundingBox(catalogObject.MeshReps.Last()));
            catalogObject.MeshReps = catalogObject.MeshReps.OrderBy(o => o.LevelOfDetail).ToList();

            string coId = db.CreateCatalogObject(catalogObject);
            return Request.CreateResponseDBMS(HttpStatusCode.Created, coId);
        }

        private MeshRep CreateBoundingBox(MeshRep mesh)
        {
            var componentVList = mesh.Components.SelectMany(m => m.Vertices);
            double minX = componentVList.Min(v => v.x);
            double maxX = componentVList.Max(v => v.x);
            double minY = componentVList.Min(v => v.y);
            double maxY = componentVList.Max(v => v.y);
            double minZ = componentVList.Min(v => v.z);
            double maxZ = componentVList.Max(v => v.z);

            double widthM = (maxX - minX) / 2.0;
            double depthM = (maxY - minY) / 2.0;
            double heightM = (maxZ - minZ) / 2.0;

            Component component = new Component()
            {
                Vertices = new List<Vector3D>
                {
                    new Vector3D(-widthM, -depthM, -heightM),
                    new Vector3D(widthM, -depthM, -heightM),
                    new Vector3D(widthM, depthM, -heightM),
                    new Vector3D(-widthM, depthM, -heightM),
                    new Vector3D(-widthM, -depthM, heightM),
                    new Vector3D(widthM, -depthM, heightM),
                    new Vector3D(widthM, depthM, heightM),
                    new Vector3D(-widthM, depthM, heightM)
                },
                Triangles = new List<int[]>
                {
                    new int[]{ 0,2,1 },
                    new int[]{ 0,3,2 },
                    new int[]{ 0,1,5 },
                    new int[]{ 0,5,4 },
                    new int[]{ 4,6,7 },
                    new int[]{ 4,5,6 },
                    new int[]{ 0,7,3 },
                    new int[]{ 0,4,7 },
                    new int[]{ 1,6,5 },
                    new int[]{ 1,2,6 },
                    new int[]{ 2,7,6 },
                    new int[]{ 2,3,7 }
                }
            };

            MeshRep newMesh = new MeshRep()
            {
                Components = new List<Component>() { component },
                Joints = new List<Joint>(),
                LevelOfDetail = LevelOfDetail.LOD100
            };

            return newMesh;
        }

        public HttpResponseMessage Put([FromBody] MongoCatalogObject catalogObject)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (!user.IsAdmin)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be an Admin");
            }

            if (catalogObject == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Missing CatalogObject");
            }

            catalogObject.MeshReps = catalogObject.MeshReps.OrderBy(o => o.LevelOfDetail).ToList();
            db.UpdateCatalogObject(catalogObject);
            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Update Successful");
        }

        public HttpResponseMessage Patch([FromBody] CatalogObjectMetadata catalogObjectData)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (!user.IsAdmin)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be an Admin");
            }

            if (catalogObjectData == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Missing CatalogObject Data");
            }

            db.UpdateCatalogObjectMetaData(catalogObjectData);
            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Update Successful");
        }

        public HttpResponseMessage Delete(string id)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (!user.IsAdmin)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be an Admin");
            }

            db.DeleteCatalogObject(id);
            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Delete Successful");
        }
    }
}