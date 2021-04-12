using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace DBMS.Controllers.APIControllers
{
    public class DownloadsController : ApiController
    {
        public string filename = "BIMPlatform.zip";
        public HttpResponseMessage Get(string id)
        {
            if (id.ToLower() == "bimplatform")
            {
                // Make sure bimplatform file exists
                string dir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                string path = Path.Combine(dir, filename);
                if (File.Exists(path))
                {
                    var bytes = File.ReadAllBytes(path);
                    var dataStream = new MemoryStream(bytes);
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StreamContent(dataStream);

                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = filename;
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("applicaiton/octet-stream");
                    return response;
                }
                else
                {
                    return Request.CreateReasonResponse(HttpStatusCode.NotFound, "Unable to find " + path);
                }
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}