using DbmsApi;
using DbmsApi.Mongo;
using MathPackage;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleAPI.Methods
{
    public class VirtualObjects
    {
        // COMPLETE
        public static List<RuleCheckObject> KitchenWorkingTriangle(RuleCheckModel model)
        {
            // Here need to find all model object that are sinks, fridges, and stoves/ranges
            List<RuleCheckObject> sinks = model.Objects.Where(o => o.Type == DbmsApi.ObjectTypes.Sink).ToList();
            List<RuleCheckObject> refrigerators = model.Objects.Where(o => o.Type == DbmsApi.ObjectTypes.Refrigerator).ToList();
            List<RuleCheckObject> cookTops = model.Objects.Where(o => o.Type == DbmsApi.ObjectTypes.CookTop || o.Type == DbmsApi.ObjectTypes.Stove || o.Type == DbmsApi.ObjectTypes.Range).ToList();

            // Create a triangle that connects the three
            // (Might need to make sure that they are in the same kitchen somehow...)
            List<RuleCheckObject> newVirtualObjs = new List<RuleCheckObject>();
            foreach (RuleCheckObject sink in sinks)
            {
                Vector3D sinkFront = Vector3D.Add(sink.Location, Vector3D.Multiply(sink.ForwardDirectionY, sink.Dimentions.y / 2.0));
                double sinkBottom = Vector3D.Add(sink.Location, Vector3D.Multiply(sink.UpDirectionZ, sink.Dimentions.z / 2.0).Neg()).z;
                double sinkTop = Vector3D.Add(sink.Location, Vector3D.Multiply(sink.UpDirectionZ, sink.Dimentions.z / 2.0)).z;
                foreach (RuleCheckObject fridge in refrigerators)
                {
                    Vector3D fridgeFront = Vector3D.Add(fridge.Location, Vector3D.Multiply(fridge.ForwardDirectionY, fridge.Dimentions.y / 2.0));
                    double fridgeBottom = Vector3D.Add(fridge.Location, Vector3D.Multiply(fridge.UpDirectionZ, fridge.Dimentions.z / 2.0).Neg()).z;
                    double fridgeTop = Vector3D.Add(fridge.Location, Vector3D.Multiply(fridge.UpDirectionZ, fridge.Dimentions.z / 2.0)).z;
                    foreach (RuleCheckObject cooktop in cookTops)
                    {
                        Vector3D cooktopFront = Vector3D.Add(cooktop.Location, Vector3D.Multiply(cooktop.ForwardDirectionY, cooktop.Dimentions.y / 2.0));
                        double cooktopBottom = Vector3D.Add(cooktop.Location, Vector3D.Multiply(cooktop.UpDirectionZ, cooktop.Dimentions.z / 2.0).Neg()).z;
                        double cooktopTop = Vector3D.Add(cooktop.Location, Vector3D.Multiply(cooktop.UpDirectionZ, cooktop.Dimentions.z / 2.0)).z;

                        double topHeight = Math.Max(Math.Max(sinkTop, fridgeTop), cooktopTop);
                        double bottomHeight = Math.Min(Math.Min(sinkBottom, fridgeBottom), cooktopBottom);

                        Mesh newTriangleMesh = Utils.CreateTriangleMesh(sinkFront, fridgeFront, cooktopFront, bottomHeight, bottomHeight);

                        RuleCheckObject newObj = new RuleCheckObject("KitchenWorkingTriangle" + newVirtualObjs.Count.ToString(), ObjectTypes.KitchenWorkingTriangle, newTriangleMesh);
                        newVirtualObjs.Add(newObj);
                    }
                }
            }

            return newVirtualObjs;
        }
        public static List<RuleCheckObject> Corner(RuleCheckModel model)
        {
            List<RuleCheckObject> newVirtualObjs = new List<RuleCheckObject>();

            List<RuleCheckObject> wallObjs = model.Objects.Where(o => o.Type == DbmsApi.ObjectTypes.Wall).ToList();

            // go over all walls with other walls:
            for (int i = 0; i < wallObjs.Count; i++)
            {
                RuleCheckObject walli = wallObjs[i];
                Mesh wallMeshi = new Mesh(walli.GlobalVerticies, walli.Triangles);

                double topA = wallMeshi.VertexList.Max(v => v.z);
                double bottomA = wallMeshi.VertexList.Min(v => v.z);

                // Get the lines of walli:
                Vector3D end0A1 = Vector3D.Add(walli.Location, (Vector3D.Multiply(walli.LeftDirectionX, walli.Dimentions.x / 2.0)));
                end0A1 = Vector3D.Add(end0A1, (Vector3D.Multiply(walli.ForwardDirectionY, walli.Dimentions.y / 2.0)));
                Vector3D end1A1 = Vector3D.Add(walli.Location, (Vector3D.Multiply(walli.LeftDirectionX, walli.Dimentions.x / 2.0)).Neg());
                end1A1 = Vector3D.Add(end1A1, (Vector3D.Multiply(walli.ForwardDirectionY, walli.Dimentions.y / 2.0)));
                Line lineA1 = new Line(end0A1.Get2D(1), end1A1.Get2D(1));

                Vector3D end0A2 = Vector3D.Add(walli.Location, (Vector3D.Multiply(walli.LeftDirectionX, walli.Dimentions.x / 2.0)));
                end0A2 = Vector3D.Add(end0A2, (Vector3D.Multiply(walli.ForwardDirectionY, walli.Dimentions.y / 2.0)).Neg());
                Vector3D end1A2 = Vector3D.Add(walli.Location, (Vector3D.Multiply(walli.LeftDirectionX, walli.Dimentions.x / 2.0)).Neg());
                end1A2 = Vector3D.Add(end1A2, (Vector3D.Multiply(walli.ForwardDirectionY, walli.Dimentions.y / 2.0)).Neg());
                Line lineA2 = new Line(end0A2.Get2D(1), end1A2.Get2D(1));

                for (int j = i + 1; j < wallObjs.Count; j++)
                {
                    RuleCheckObject wallj = wallObjs[j];
                    Mesh wallMeshj = new Mesh(wallj.GlobalVerticies, wallj.Triangles);

                    double topB = wallMeshj.VertexList.Max(v => v.z);
                    double bottomB = wallMeshj.VertexList.Min(v => v.z);

                    // Get the lines of wallj:
                    Vector3D end0B1 = Vector3D.Add(wallj.Location, (Vector3D.Multiply(wallj.LeftDirectionX, wallj.Dimentions.x / 2.0)));
                    end0B1 = Vector3D.Add(end0B1, (Vector3D.Multiply(wallj.ForwardDirectionY, wallj.Dimentions.y / 2.0)));
                    Vector3D end1B1 = Vector3D.Add(wallj.Location, (Vector3D.Multiply(wallj.LeftDirectionX, wallj.Dimentions.x / 2.0)).Neg());
                    end1B1 = Vector3D.Add(end1B1, (Vector3D.Multiply(wallj.ForwardDirectionY, wallj.Dimentions.y / 2.0)));
                    Line lineB1 = new Line(end0B1.Get2D(1), end1B1.Get2D(1));

                    Vector3D end0B2 = Vector3D.Add(wallj.Location, (Vector3D.Multiply(wallj.LeftDirectionX, wallj.Dimentions.x / 2.0)));
                    end0B2 = Vector3D.Add(end0B2, (Vector3D.Multiply(wallj.ForwardDirectionY, wallj.Dimentions.y / 2.0)).Neg());
                    Vector3D end1B2 = Vector3D.Add(wallj.Location, (Vector3D.Multiply(wallj.LeftDirectionX, wallj.Dimentions.x / 2.0)).Neg());
                    end1B2 = Vector3D.Add(end1B2, (Vector3D.Multiply(wallj.ForwardDirectionY, wallj.Dimentions.y / 2.0)).Neg());
                    Line lineB2 = new Line(end0B2.Get2D(1), end1B2.Get2D(1));

                    if (Utils.MeshOverlap(wallMeshi, wallMeshj, 1.1))
                    {
                        Vector3D A1B1 = Vector3D.Cross(lineA1.GetVect(), lineB1.GetVect());
                        A1B1 = Vector3D.Divide(A1B1, A1B1.z);
                        Vector3D A1B2 = Vector3D.Cross(lineA1.GetVect(), lineB2.GetVect());
                        A1B2 = Vector3D.Divide(A1B2, A1B2.z);
                        Vector3D A2B1 = Vector3D.Cross(lineA2.GetVect(), lineB1.GetVect());
                        A2B1 = Vector3D.Divide(A2B1, A2B1.z);
                        Vector3D A2B2 = Vector3D.Cross(lineA2.GetVect(), lineB2.GetVect());
                        A2B2 = Vector3D.Divide(A2B2, A2B2.z);

                        double top = Math.Min(topA, topB);
                        double bottom = Math.Max(bottomA, bottomB);

                        Mesh newCornerMesh = Utils.CreateExtrudedMesh(new List<Vector3D>() { A1B1, A2B1, A2B2, A1B2 }, top, bottom);
                        RuleCheckObject newObj = new RuleCheckObject("Corner" + newVirtualObjs.Count.ToString(), ObjectTypes.Corner, newCornerMesh);
                        newVirtualObjs.Add(newObj);
                    }
                }
            }

            return newVirtualObjs;
        }
        public static List<RuleCheckObject> KitchenLandingArea(RuleCheckModel model)
        {
            double dist = 0.1524;

            List<RuleCheckObject> newVirtualObjs = new List<RuleCheckObject>();

            List<RuleCheckObject> cornerCabinets = model.Objects.Where(o => o.Type == DbmsApi.ObjectTypes.CornerCabinet).ToList();
            List<RuleCheckObject> cabinets = model.Objects.Where(o => o.Type == DbmsApi.ObjectTypes.Cabinet).ToList();


            List<Tuple<Vector3D, List<RuleCheckObject>>> cabinetGroups = new List<Tuple<Vector3D, List<RuleCheckObject>>>();
            for (int i = 0; i < cabinets.Count; i++)
            {
                RuleCheckObject newCabinet = cabinets[i];

                List<Tuple<Vector3D, List<RuleCheckObject>>> cabinetGroupsNextToObject = new List<Tuple<Vector3D, List<RuleCheckObject>>>();
                foreach (Tuple<Vector3D, List<RuleCheckObject>> cabinetGroup in cabinetGroups)
                {
                    // Check if the cabinet is facing the same direction
                    if (Vector3D.AreEqual(cabinetGroup.Item1, newCabinet.LeftDirectionX))
                    {
                        foreach (RuleCheckObject cabinetGroupCab in cabinetGroup.Item2)
                        {
                            // Check if very close to the cabinet in the group
                            if (Utils.MeshDistance(newCabinet.GetGlobalMesh(), cabinetGroupCab.GetGlobalMesh()) <= dist)
                            {
                                cabinetGroupsNextToObject.Add(cabinetGroup);
                                break;
                            }
                        }
                    }
                }
                Tuple<Vector3D, List<RuleCheckObject>> newCabinetGroup = new Tuple<Vector3D, List<RuleCheckObject>>(newCabinet.LeftDirectionX, new List<RuleCheckObject> { newCabinet });
                foreach (Tuple<Vector3D, List<RuleCheckObject>> cabinetGroup in cabinetGroupsNextToObject)
                {
                    foreach (RuleCheckObject cab in cabinetGroup.Item2)
                    {
                        newCabinetGroup.Item2.Add(cab);
                    }
                    cabinetGroups.Remove(cabinetGroup);
                }

                cabinetGroups.Add(newCabinetGroup);
            }

            for (int i = 0; i < cornerCabinets.Count; i++)
            {
                RuleCheckObject newCornerCabinet = cornerCabinets[i];

                foreach (Tuple<Vector3D, List<RuleCheckObject>> cabinetGroup in cabinetGroups)
                {
                    foreach (RuleCheckObject cabinetGroupCab in cabinetGroup.Item2)
                    {
                        // Check if the cabinet is facing the same direction and very close to the cabinet in the group
                        if (Utils.MeshDistance(newCornerCabinet.GetGlobalMesh(), cabinetGroupCab.GetGlobalMesh()) <= dist)
                        {
                            cabinetGroup.Item2.Add(newCornerCabinet);
                            break;
                        }
                    }
                }
            }

            foreach (Tuple<Vector3D, List<RuleCheckObject>> cabinetGroup in cabinetGroups)
            {
                List<Vector3D> projectedCounterTopPoints = new List<Vector3D>();
                double top = double.MinValue;
                foreach (RuleCheckObject cabinet in cabinetGroup.Item2)
                {
                    foreach (Vector3D v in cabinet.GetGlobalMesh().VertexList)
                    {
                        top = Math.Max(top, v.z);
                        Vector3D newV = v.Get2D(0);
                        if (!projectedCounterTopPoints.Any(p => Vector3D.AreEqual(newV, p)))
                        {
                            projectedCounterTopPoints.Add(newV);
                        }
                    }
                }
                // Do a convex hull of the points:
                //projectedCounterTopPoints = Utils.GetConvexHull(projectedCounterTopPoints);
                double minX = projectedCounterTopPoints.Min(v => v.x);
                double maxX = projectedCounterTopPoints.Max(v => v.x);
                double minY = projectedCounterTopPoints.Min(v => v.y);
                double maxY = projectedCounterTopPoints.Max(v => v.y);
                double minZ = projectedCounterTopPoints.Min(v => v.z);
                double maxZ = projectedCounterTopPoints.Max(v => v.z);
                projectedCounterTopPoints = new List<Vector3D>
                {
                    new Vector3D(minX, minY,minZ),
                    new Vector3D(minX, maxY, minZ),
                    new Vector3D(maxX, maxY, minZ),
                    new Vector3D(maxX, minY, minZ)
                };

                Mesh newMesh = Utils.CreateExtrudedMesh(projectedCounterTopPoints, top + 0.0254, top);

                // Get the mesh center
                Vector3D center = Vector3D.Average(newMesh.VertexList.ToArray());
                double angleDeg = Vector3D.AngleRad(cabinetGroup.Item1, new Vector3D(1, 0, 0)) * (180.0 / Math.PI);
                angleDeg = cabinetGroup.Item1.y >= 0 ? angleDeg : 360.0 - angleDeg;
                angleDeg = (angleDeg + 180.0) % 360.0;
                List<Vector3D> newMeshVects = new List<Vector3D>();
                foreach (Vector3D v in newMesh.VertexList)
                {
                    Vector3D newVect = Vector3D.Subract(v, center);
                    newVect = Utils.RotatePointXY(newVect, angleDeg);
                    newMeshVects.Add(newVect);
                }
                newMesh.VertexList = newMeshVects;

                RuleCheckObject newObj = new RuleCheckObject("KitchenLandingArea" + newVirtualObjs.Count.ToString(), ObjectTypes.KitchenLandingArea, newMesh);
                newVirtualObjs.Add(newObj);
            }

            return newVirtualObjs;
        }
        public static List<RuleCheckObject> FurnishingCoM(RuleCheckModel model)
        {
            // Here need to find all model object that are type furnishing
            List<RuleCheckObject> furnishingObj = model.Objects.Where(o => o.Type == ObjectTypes.FurnishingElement).ToList();
            List<RuleCheckObject> newVirtualObjs = new List<RuleCheckObject>();
            Vector3D ApSum = new Vector3D();
            double ASum = 0;
            foreach (RuleCheckObject o in furnishingObj)
            {
                double mass = o.Dimentions.x * o.Dimentions.y * o.Dimentions.z;
                Vector3D Ap = Vector3D.Multiply(o.Location, mass);
                ApSum = Vector3D.Add(ApSum, Ap);
                ASum += mass;
            }

            Vector3D centroid = Vector3D.Divide(ApSum, ASum);
            Mesh newTriangleMesh = Utils.CreateTriangleMesh(Vector3D.Add(centroid, new Vector3D(0, 0.1, 0)), Vector3D.Add(centroid, new Vector3D(0.1, -0.1, 0)), Vector3D.Add(centroid, new Vector3D(-0.1, -0.1, 0)), 0.0, 0.0);

            RuleCheckObject newObj = new RuleCheckObject("FurnishingCoM" + newVirtualObjs.Count.ToString(), ObjectTypes.FurnishingCoM, newTriangleMesh);
            newVirtualObjs.Add(newObj);

            return newVirtualObjs;
        }
        public static List<RuleCheckObject> RoomCentroid(RuleCheckModel model)
        {
            // Here need to find all model object that are type furnishing
            List<RuleCheckObject> floorObj = model.Objects.Where(o => o.Type == ObjectTypes.Floor).ToList();

            List<RuleCheckObject> newVirtualObjs = new List<RuleCheckObject>();
            foreach (RuleCheckObject o in floorObj)
            {
                Mesh newTriangleMesh = Utils.CreateTriangleMesh(Vector3D.Add(o.Location, new Vector3D(0, 0.1, 0)), Vector3D.Add(o.Location, new Vector3D(0.1, -0.1, 0)), Vector3D.Add(o.Location, new Vector3D(-0.1, -0.1, 0)), 0.0, 0.0);
                RuleCheckObject newObj = new RuleCheckObject("RoomCentroid" + newVirtualObjs.Count.ToString(), ObjectTypes.RoomCentroid, newTriangleMesh);
                newVirtualObjs.Add(newObj);
            }

            return newVirtualObjs;
        }

        //// INCOMPLETE
        //public static List<RuleCheckObject> CreateIslands(RuleCheckModel model)
        //{
        //    List<RuleCheckObject> newVirtualObjs = new List<RuleCheckObject>();

        //    // Go over all Furnishing and flowterminal objects

        //    // Need to find a way to check if the object is connected directly or indirectly to a wall and thus is not an Island

        //    // Note there can be multiple Islands and therefore make sure the objects are grouped correctly

        //    throw new NotImplementedException();
        //    return newVirtualObjs;
        //}
        //public static List<RuleCheckObject> CreatePennisulas(RuleCheckModel model)
        //{
        //    List<RuleCheckObject> newVirtualObjs = new List<RuleCheckObject>();

        //    // Get all furnishing and Flowterminal Objects and check if it is touching (ie. within X inches of) a wall
        //    // Make a dictionary for this so that the final pennisulas can be created after it is determined which items are next to eachother.

        //    throw new NotImplementedException();
        //    return newVirtualObjs;
        //}
        //public static List<RuleCheckObject> CreateOpenSpaces(RuleCheckModel model)
        //{
        //    List<RuleCheckObject> newVirtualObjs = new List<RuleCheckObject>();

        //    // This is basically just take a convex hull of the building and subtract all furnishin items.
        //    // Need to think about this more and how each space will work

        //    throw new NotImplementedException();
        //    return newVirtualObjs;
        //}
        //public static List<RuleCheckObject> CreateWalkPaths(RuleCheckModel model)
        //{
        //    List<RuleCheckObject> newVirtualObjs = new List<RuleCheckObject>();

        //    // Get the projection of all items on the floor and subtract them from the slab

        //    // Should be one for every floor slab

        //    // These will be 2D objects for now (or for ever I guess... depends if they should have some height for diplaying purposes...)

        //    throw new NotImplementedException();
        //    return newVirtualObjs;
        //}
    }
}