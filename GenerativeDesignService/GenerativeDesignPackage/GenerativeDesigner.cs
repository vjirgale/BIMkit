using DbmsApi;
using DbmsApi.API;
using MathPackage;
using ModelCheckPackage;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerativeDesignPackage
{
    public class GenerativeDesigner
    {
        public ModelChecker ModelCheck { get; internal set; }
        public CatalogObject CatalogObject { get; internal set; }
        public Vector3D Location;
        public int Itterations;
        public double MoveAmount = 0.2;

        public GenerativeDesigner(Model model, List<Rule> rules, CatalogObject catalogObject, Vector3D initialLoc, int itterations = 100)
        {
            ModelCheck = new ModelChecker(model, rules);
            CatalogObject = catalogObject;
            Location = initialLoc;
            Itterations = itterations;
        }

        public Model ExecuteGenDesign()
        {
            // Get all the possible orientations:
            List<Vector4D> orientations = new List<Vector4D>()
            {
                 new Vector4D(0, 0, 1, Math.PI * 0.0/180.0),
                 new Vector4D(0, 0, 1, Math.PI * 90.0/180.0),
                 new Vector4D(0, 0, 1, Math.PI * 180.0/180.0),
                 new Vector4D(0, 0, 1, Math.PI * 270.0/180.0),
            };

            double bestEval = 0;
            Configuration bestConfig = new Configuration()
            {
                Location = Location,
                CatalogObject = CatalogObject,
                Orientation = orientations.First()
            };
            while (Itterations > 0)
            {
                Itterations--;

                List<Vector3D> locations = new List<Vector3D>()
                {
                    new Vector3D(bestConfig.Location.x+MoveAmount, bestConfig.Location.y, bestConfig.Location.z),
                    new Vector3D(bestConfig.Location.x-MoveAmount, bestConfig.Location.y, bestConfig.Location.z),
                    new Vector3D(bestConfig.Location.x, bestConfig.Location.y+MoveAmount, bestConfig.Location.z),
                    new Vector3D(bestConfig.Location.x, bestConfig.Location.y-MoveAmount, bestConfig.Location.z),
                };

                foreach (Vector3D location in locations)
                {
                    foreach (Vector4D orienation in orientations)
                    {
                        string newObjId = ModelCheck.Model.AddObject(CatalogObject, location, orienation);
                        ModelCheck.RecreateVirtualObjects();
                        List<RuleResult> results = ModelCheck.CheckModel(0);
                        double evalVal = results.Sum(r => r.PassVal);

                        // Keep the best one
                        if (evalVal > bestEval)
                        {
                            bestEval = evalVal;
                            bestConfig.Location = location;
                            bestConfig.Orientation = orienation;
                        }

                        ModelCheck.Model.RemoveObject(newObjId);
                    }
                }
            }

            // Put the best back in:
            ModelCheck.Model.AddObject(bestConfig.CatalogObject, bestConfig.Location, bestConfig.Orientation);

            return ModelCheck.Model.FullModel();
        }
    }
}