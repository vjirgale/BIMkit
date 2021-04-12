using DbmsApi;
using DbmsApi.API;
using DbmsApi.Mongo;
using MathPackage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuleAPI.Models
{
    public class RuleCheckObject
    {
        public string ID;
        public string Name;
        public ObjectTypes Type;
        public Vector3D Location;
        public Vector4D Orientation;
        public bool VirtualObject;
        public Properties Properties;

        public Vector3D LeftDirectionX;
        public Vector3D ForwardDirectionY;
        public Vector3D UpDirectionZ;
        public Vector3D Dimentions;

        public List<Vector3D> GlobalVerticies = new List<Vector3D>();
        public List<Vector3D> LocalVerticies = new List<Vector3D>();
        public List<int> Triangles = new List<int>();

        [JsonConstructor]
        private RuleCheckObject() { }

        public RuleCheckObject(ModelObject modelObject)
        {
            ID = modelObject.Id;
            Name = modelObject.Name;
            Type = modelObject.TypeId;
            Location = modelObject.Location;
            Orientation = modelObject.Orientation;
            VirtualObject = false;
            Properties = modelObject.Properties;

            LocalVerticies = new List<Vector3D>();
            Triangles = new List<int>();
            foreach (Component component in modelObject.Components)
            {
                Triangles.AddRange(component.Triangles.SelectMany(t => new List<int>() { t[0] + LocalVerticies.Count, t[1] + LocalVerticies.Count, t[2] + LocalVerticies.Count }));
                LocalVerticies.AddRange(component.Vertices.Select(v => v.Copy()));
            }
            Vector3D realCenter, realDimentions;
            Utils.GetXYZDimentions(LocalVerticies, out realCenter, out realDimentions);
            Dimentions = realDimentions;
            // Should Check that the center and location are the same:
            //
            //
            //

            Matrix4 orientationMatrix = Utils.GetTranslationMatrixFromLocationOrientation(new Vector3D(0, 0, 0), Orientation);
            LeftDirectionX = Matrix4.Multiply(orientationMatrix, new Vector4D(1, 0, 0, 1));
            ForwardDirectionY = Matrix4.Multiply(orientationMatrix, new Vector4D(0, 1, 0, 1));
            UpDirectionZ = Matrix4.Multiply(orientationMatrix, new Vector4D(0, 0, 1, 1));

            Matrix4 translationMatrix = Utils.GetTranslationMatrixFromLocationOrientation(Location, Orientation);
            GlobalVerticies = LocalVerticies.Select(v => Matrix4.Multiply(translationMatrix, v.Get4D(1.0)).Get3D()).ToList();
        }

        public RuleCheckObject(string name, ObjectTypes type, Mesh globalMesh, bool virtualObj = true)
        {
            ID = new Guid().ToString();
            Name = name;
            Type = type;
            VirtualObject = virtualObj;
            Properties = new Properties();

            GlobalVerticies = globalMesh.VertexList;
            Triangles = globalMesh.TriangleList;
            Vector3D realCenter, realDimentions;
            Utils.GetXYZDimentions(LocalVerticies, out realCenter, out realDimentions);
            Dimentions = realDimentions;
            Location = realCenter;
            Orientation = Utils.GetQuaterion(new Vector3D(1, 0, 0), 0);

            Matrix4 orientationMatrix = Utils.GetTranslationMatrixFromLocationOrientation(new Vector3D(0, 0, 0), Orientation);
            LeftDirectionX = Matrix4.Multiply(orientationMatrix, new Vector4D(1, 0, 0, 1));
            ForwardDirectionY = Matrix4.Multiply(orientationMatrix, new Vector4D(0, 1, 0, 1));
            UpDirectionZ = Matrix4.Multiply(orientationMatrix, new Vector4D(0, 0, 1, 1));

            Matrix4 translationMatrix = Utils.GetTranslationMatrixFromLocationOrientation(Location, Orientation).GetTranspose();
            LocalVerticies = GlobalVerticies.Select(v => Matrix4.Multiply(translationMatrix, v.Get4D(1.0)).Get3D()).ToList();
        }

        public RuleCheckObject(string name, ObjectTypes type, Mesh localMesh, Vector3D location, Vector4D orientation, bool virtualObj = true)
        {
            ID = new Guid().ToString();
            Name = name;
            Type = type;
            VirtualObject = virtualObj;
            Properties = new Properties();

            LocalVerticies = localMesh.VertexList;
            Triangles = localMesh.TriangleList;
            Vector3D realCenter, realDimentions;
            Utils.GetXYZDimentions(LocalVerticies, out realCenter, out realDimentions);
            Dimentions = realDimentions;
            Location = location;
            Orientation = orientation;

            Matrix4 orientationMatrix = Utils.GetTranslationMatrixFromLocationOrientation(new Vector3D(0, 0, 0), Orientation);
            LeftDirectionX = Matrix4.Multiply(orientationMatrix, new Vector4D(1, 0, 0, 1));
            ForwardDirectionY = Matrix4.Multiply(orientationMatrix, new Vector4D(0, 1, 0, 1));
            UpDirectionZ = Matrix4.Multiply(orientationMatrix, new Vector4D(0, 0, 1, 1));

            Matrix4 translationMatrix = Utils.GetTranslationMatrixFromLocationOrientation(Location, Orientation);
            GlobalVerticies = LocalVerticies.Select(v => Matrix4.Multiply(translationMatrix, v.Get4D(1.0)).Get3D()).ToList();
        }

        public Mesh GetGlobalMesh()
        {
            return new Mesh(GlobalVerticies, Triangles);
        }
    }
}