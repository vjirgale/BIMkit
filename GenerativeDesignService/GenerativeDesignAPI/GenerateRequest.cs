using DbmsApi.API;
using MathPackage;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerativeDesignAPI
{
    public class GenerativeRequest
    {
        public string ModelID;
        public string CatalogID;
        public List<string> RuleIDs;
        public TokenData DBMSToken;
        public string RMSUsername;
        public LevelOfDetail LOD;
        public Vector3D StartLocation;

        public GenSettings GenSettings;

        public GenerativeRequest(TokenData dbmsToken, string rmsUsername, string modelId, string catalogID, List<string> ruleIds, LevelOfDetail lod, Vector3D startLocation, GenSettings genSettings)
        {
            ModelID = modelId;
            CatalogID = catalogID;
            RuleIDs = ruleIds;
            DBMSToken = dbmsToken;
            RMSUsername = rmsUsername;
            LOD = lod;
            StartLocation = startLocation;
            GenSettings = genSettings;
        }
    }

    public class GenSettings
    {
        public int itterations;
        public double movement;
        public double rate;
        public int moves;
        public bool showRoute;

        public GenSettings(int itterations, double movement, double rate, int moves, bool showRoute)
        {
            this.itterations = itterations;
            this.movement = movement;
            this.rate = rate;
            this.moves = moves;
            this.showRoute = showRoute;
        }
    }
}