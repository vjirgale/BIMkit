using DbmsApi;
using DbmsApi.API;
using ModelCheckAPI;
using ModelCheckPackage;
using RuleAPI;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ModelCheckService.Controllers
{
    public class CheckController : ApiController
    {
        private static DBMSAPIController DBMSAPIController = new DBMSAPIController("https://localhost:44322//api/");
        private static RuleAPIController RuleAPIController = new RuleAPIController("https://localhost:44370/api/");

        public async Task<HttpResponseMessage> Post([FromBody] CheckRequest request)
        {
            try
            {
                // Use token to access the model
                DBMSAPIController.SetSessionToken(request.DBMSToken);
                APIResponse<Model> response = await DBMSAPIController.GetModel(new ItemRequest(request.ModelID, request.LOD));
                if (response.Code != HttpStatusCode.OK)
                {
                    return Request.CreateResponse(response.Code, response.ReasonPhrase);
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

                // do the check:
                ModelChecker modelCheck = new ModelChecker(response.Data, rules);
                List<RuleResult> result = modelCheck.CheckModel(request.DefaultRuleResult);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Error");
            }
        }
    }
}
