using DbmsApi;
using DbmsApi.API;
using GenerativeDesignAPI;
using GenerativeDesignPackage;
using RuleAPI;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GenerativeDesignService.Controllers
{
    public class GenerateController : ApiController
    {
        private static DBMSAPIController DBMSAPIController = new DBMSAPIController("https://localhost:44322//api/");
        private static RuleAPIController RuleAPIController = new RuleAPIController("https://localhost:44370/api/");

        public async Task<HttpResponseMessage> Post([FromBody] GenerativeRequest request)
        {
            try
            {
                DBMSAPIController.SetSessionToken(request.DBMSToken);

                // Get the model, object, and rules
                APIResponse<Model> response = await DBMSAPIController.GetModel(new ItemRequest(request.ModelID, request.LOD));
                if (response.Code != HttpStatusCode.OK)
                {
                    return Request.CreateResponse(response.Code, response.ReasonPhrase);
                }

                APIResponse<CatalogObject> response2 = await DBMSAPIController.GetCatalogObject(new ItemRequest(request.CatalogID, request.LOD));
                if (response2.Code != System.Net.HttpStatusCode.OK)
                {
                    return Request.CreateResponse(response2.Code, response2.ReasonPhrase);
                }

                // Get the rules
                RuleAPIController.SetSessionUser(request.RMSUsername);
                List<Rule> rules = new List<Rule>();
                foreach (string ruleId in request.RuleIDs)
                {
                    APIResponse<Rule> response1 = await RuleAPIController.GetRuleAsync(ruleId);
                    if (response1.Code == HttpStatusCode.OK)
                    {
                        rules.Add(response1.Data);
                    }
                }

                Model model = response.Data;
                CatalogObject catalogObject = response2.Data;

                GenerativeDesigner generativeDesigner = new GenerativeDesigner(model, rules, catalogObject, request.StartLocation);

                int itterations = request.GenSettings.itterations;
                double movement = request.GenSettings.movement;
                double rate = request.GenSettings.rate;
                int moves = request.GenSettings.moves;
                bool showRoute = request.GenSettings.showRoute;
                Model newModel = generativeDesigner.ExecuteGenDesign(itterations, movement, rate, moves, showRoute);

                // Save the models:
                newModel.Name = "Generated Model";
                APIResponse<string> response3 = await DBMSAPIController.CreateModel(newModel);
                if (response3.Code != System.Net.HttpStatusCode.Created)
                {
                    return Request.CreateResponse(response3.Code, response3.ReasonPhrase);
                }


                return Request.CreateResponse(HttpStatusCode.OK, response3.Data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Error");
            }
        }
    }
}
