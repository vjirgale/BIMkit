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

        private Random random = new Random();
        //private double alphaMove = 0.00001;
        //private double MoveSTD = 10.0;

        public GenerativeDesigner(Model model, List<Rule> rules, CatalogObject catalogObject, Vector3D initialLoc)
        {
            ModelCheck = new ModelChecker(model, rules);
            CatalogObject = catalogObject;
            Location = initialLoc;
        }

        public Model ExecuteGenDesign(int Itterations, double moveAmount, double reductionRate, int movesPerItteration, bool showRoute)
        {
            List<Configuration> configsList = new List<Configuration>();

            // Get all the possible orientations:
            List<Vector4D> orientations = new List<Vector4D>()
            {
                Utils.GetQuaterion(new Vector3D(0, 0, 1), 0.0 * Math.PI / 180.0),
                Utils.GetQuaterion(new Vector3D(0, 0, 1), 90.0 * Math.PI / 180.0),
                Utils.GetQuaterion(new Vector3D(0, 0, 1), 180.0 * Math.PI / 180.0),
                Utils.GetQuaterion(new Vector3D(0, 0, 1), 270.0 * Math.PI / 180.0)
            };

            double bestEval = 0;
            int interationNum = 0;
            Configuration bestConfig = new Configuration()
            {
                Location = Location,
                CatalogObject = CatalogObject,
                Orientation = orientations.First()
            };
            while (Itterations > interationNum)
            {
                interationNum++;

                moveAmount *= reductionRate;
                //List<Vector3D> locations = new List<Vector3D>()
                //{
                //    new Vector3D(bestConfig.Location.x+moveAmount, bestConfig.Location.y, bestConfig.Location.z),
                //    new Vector3D(bestConfig.Location.x-moveAmount, bestConfig.Location.y, bestConfig.Location.z),
                //    new Vector3D(bestConfig.Location.x, bestConfig.Location.y+moveAmount, bestConfig.Location.z),
                //    new Vector3D(bestConfig.Location.x, bestConfig.Location.y-moveAmount, bestConfig.Location.z),
                //};

                //double moveAmount = Math.Pow(Math.E, -alphaMove * interationNum) * MoveSTD;
                List<Vector3D> locations = new List<Vector3D>();
                for (int i = 0; i < movesPerItteration; i++)
                {
                    double deltaX = RandomGausian(0, moveAmount);
                    double deltaY = RandomGausian(0, moveAmount);
                    locations.Add(new Vector3D(bestConfig.Location.x + deltaX, bestConfig.Location.y + deltaY, bestConfig.Location.z));
                }

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
                            configsList.Add(new Configuration() { CatalogObject = bestConfig.CatalogObject, Location = bestConfig.Location, Orientation = bestConfig.Orientation });

                            bestEval = evalVal;
                            bestConfig.Location = location;
                            bestConfig.Orientation = orienation;
                        }

                        ModelCheck.Model.RemoveObject(newObjId);
                    }
                }
            }

            // Put the best back in:
            if (showRoute)
            {
                foreach (Configuration config in configsList)
                {
                    ModelCheck.Model.AddObject(config.CatalogObject, config.Location, config.Orientation);
                }
            }
            else
            {
                ModelCheck.Model.AddObject(bestConfig.CatalogObject, bestConfig.Location, bestConfig.Orientation);
            }

            return ModelCheck.Model.FullModel();
        }

        private double RandomGausian(double mean, double std)
        {
            double u1 = 1.0 - random.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal = mean + std * randStdNormal; //random normal(mean,stdDev^2)

            return randNormal;
        }
    }
}