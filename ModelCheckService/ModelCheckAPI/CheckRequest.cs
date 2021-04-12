using DbmsApi.API;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelCheckAPI
{
    public class CheckRequest
    {
        public string ModelID;
        public List<string> RuleIDs;
        public TokenData DBMSToken;
        public string RMSUsername;
        public LevelOfDetail LOD;
        public double DefaultRuleResult = 0.0;

        public CheckRequest(TokenData dbmsToken, string rmsUsername, string modelId, List<string> ruleIds, LevelOfDetail lod)
        {
            ModelID = modelId;
            RuleIDs = ruleIds;
            DBMSToken = dbmsToken;
            RMSUsername = rmsUsername;
            LOD = lod;
        }
    }
}