﻿namespace AutomatedCar.Models
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;
    using Helpers;
    using Visualization;
    using Avalonia.Media;
    using global::AutomatedCar.SystemComponents.Sensors;

    public class World
    {
        public string WorldName { get; private set; }
        private int controlledCarPointer = 0;
        public List<AutomatedCar> controlledCars = new();

        private Camera camera;
        private Radar radar;

        public static World Instance { get; } = new World();

        public List<WorldObject> WorldObjects { get; set; } = new List<WorldObject>();

        public AutomatedCar ControlledCar
        {
            get => this.controlledCars[this.controlledCarPointer];
        }

        public int ControlledCarPointer
        {
            get => this.controlledCarPointer;
            set
            {
                this.controlledCarPointer = value;
            }
        }

        public void AddControlledCar(AutomatedCar controlledCar)
        {
            this.controlledCars.Add(controlledCar);
            this.AddObject(controlledCar);
            AddRadar();
            AddCamera();
        }

        /// <summary>
        /// Adds a camera to the specified automated car, allowing it to detect and process world objects.
        /// </summary>
        /// <param name="automatedCar">The automated car to which the camera will be added.</param>
        /// <param name="worldObjects">The collection of world objects that the camera will process.</param>
        public void AddCamera()
        {
            this.camera = new Camera(ControlledCar, WorldObjects);
        }

        public void AddRadar()
        {
            this.radar = new Radar(ControlledCar.VirtualFunctionBus, ControlledCar, 100);
        }

        public void NextControlledCar()
        {
            if (this.controlledCarPointer < this.controlledCars.Count - 1)
            {
                this.ControlledCarPointer += 1;
            }
            else
            {
                this.ControlledCarPointer = 0;
            }
            AddRadar();
            AddCamera();
        }

        public void PrevControlledCar()
        {
            if (this.controlledCarPointer > 0)
            {
                this.ControlledCarPointer -= 1;
            }
            else
            {
                this.ControlledCarPointer = this.controlledCars.Count - 1;
            }
            AddRadar();
            AddCamera();
        }

        public int Width { get; set; }

        public int Height { get; set; }

        private DebugStatus debugStatus = new DebugStatus();

        public void AddObject(WorldObject worldObject)
        {
            this.WorldObjects.Add(worldObject);
        }

        public void PopulateFromJSON(string filename)
        {
            var rotationPoints = this.ReadRotationsPoints();
            var renderTransformOrigins = this.CalculateRenderTransformOrigins();
            var worldObjectPolygons = this.ReadPolygonJSON();
            this.WorldName = filename;

            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(filename));

            RawWorld rawWorld = JsonConvert.DeserializeObject<RawWorld>(reader.ReadToEnd());
            this.Height = rawWorld.Height;
            this.Width = rawWorld.Width;
            foreach (RawWorldObject rwo in rawWorld.Objects)
            {
                var wo = new WorldObject(rwo.X, rwo.Y, rwo.Type + ".png", this.DetermineZIndex(rwo.Type), this.DetermineCollidablity(rwo.Type), this.DetermineType(rwo.Type));
                (int x, int y) rp = (0, 0);

                if (rotationPoints.ContainsKey(rwo.Type))
                {
                    rp = rotationPoints[rwo.Type];
                }

                wo.RotationPoint = new System.Drawing.Point(rp.x, rp.y);

                string rto = "0,0";

                if (renderTransformOrigins.ContainsKey(rwo.Type))
                {
                    rto = renderTransformOrigins[rwo.Type];
                }

                wo.RenderTransformOrigin = rto;

                wo.Rotation = this.RotationMatrixToDegree(rwo.M11, rwo.M12);

                if (worldObjectPolygons.ContainsKey(rwo.Type))
                {
                    // deep copy
                    foreach (var g in worldObjectPolygons[rwo.Type])
                    {
                        wo.Geometries.Add(new PolylineGeometry(g.Points, false));
                        wo.RawGeometries.Add(new PolylineGeometry(g.Points, false));
                    }

                    // apply rotation
                    foreach (var geometry in wo.Geometries)
                    {
                        var rotate = new RotateTransform(wo.Rotation);
                        var translate = new TranslateTransform(-wo.RotationPoint.X, -wo.RotationPoint.Y);
                        var transformGroup = new TransformGroup();
                        transformGroup.Children.Add(rotate);
                        transformGroup.Children.Add(translate);

                        var mx2 = new System.Drawing.Drawing2D.Matrix(rwo.M11, rwo.M12, rwo.M21, rwo.M22, wo.RotationPoint.X, wo.RotationPoint.Y);
                        var mx = new System.Drawing.Drawing2D.Matrix();
                        mx.RotateAt(Convert.ToSingle(wo.Rotation), new PointF(wo.RotationPoint.X, wo.RotationPoint.Y));
                        mx.Translate(wo.RotationPoint.X, wo.RotationPoint.Y);
                        PointF[] gpa = new PointF[geometry.Points.Count];

                        var gpa2 = this.ToDotNetPoints(geometry.Points).ToArray();
                        this.ToDotNetPoints(geometry.Points).CopyTo(gpa);
                        mx2.TransformPoints(gpa2);
                        geometry.Points = this.ToAvaloniaPoints(gpa2);
                    }
                }

                this.AddObject(wo);
            }
        }

        private List<System.Drawing.PointF> ToDotNetPoints(IList<Avalonia.Point> points)
        {
            var result = new List<System.Drawing.PointF>();
            foreach (var p in points)
            {
                result.Add(new PointF(Convert.ToSingle(p.X), Convert.ToSingle(p.Y)));
            }

            return result;
        }

        private List<System.Drawing.PointF> ToDotNetPoints(IList<Avalonia.Point> points, int x, int y)
        {
            var result = new List<System.Drawing.PointF>();
            foreach (var p in points)
            {
                result.Add(new PointF(Convert.ToSingle(p.X) + x, Convert.ToSingle(p.Y) + y));
            }

            return result;
        }

        private Avalonia.Points ToAvaloniaPoints(IEnumerable<PointF> points)
        {
            var result = new Avalonia.Points();
            foreach (var p in points)
            {
                result.Add(new Avalonia.Point(p.X, p.Y));
            }

            return result;
        }

        private Dictionary<string, (int x, int y)> ReadRotationsPoints(string filename = "reference_points.json")
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"AutomatedCar.Assets.{filename}"));

            var rotationPoints = JsonConvert.DeserializeObject<List<RotationPoint>>(reader.ReadToEnd());
            Dictionary<string, (int x, int y)> result = new();
            foreach (RotationPoint rp in rotationPoints)
            {
                result.Add(rp.Type, (rp.X, rp.Y));
            }

            return result;
        }

        private Dictionary<string, List<PolylineGeometry>> ReadPolygonJSON(string filename = "worldobject_polygons.json")
        {
            // TODO: Avalonia specific
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"AutomatedCar.Assets.{filename}"));

            var objects = JsonConvert.DeserializeObject<Dictionary<string, List<RawWorldObjectPolygon>>>(reader.ReadToEnd())["objects"];
            var result = new Dictionary<string, List<PolylineGeometry>>();
            foreach (RawWorldObjectPolygon rwop in objects)
            {
                var polygonList = new List<PolylineGeometry>();
                foreach (RawPolygon rp in rwop.Polys)
                {
                    var points = new Avalonia.Points();

                    foreach (var p in rp.Points)
                    {
                        points.Add(new Avalonia.Point(p[0], p[1]));
                    }

                    polygonList.Add(new PolylineGeometry(points, false));
                }

                result.Add(rwop.Type, polygonList);
            }

            return result;
        }

        // It accepts different string values than WPF. For .5,.5 you actually need 50%,50%. .5,.5 is treated as "half of the logical pixel" in both directions instead of "half of the control"
        private Dictionary<string, string> CalculateRenderTransformOrigins(string filename = "reference_points.json")
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"AutomatedCar.Assets.{filename}"));

            var rotationPoints = JsonConvert.DeserializeObject<List<RotationPoint>>(reader.ReadToEnd());
            Dictionary<string, string> result = new();
            foreach (RotationPoint rp in rotationPoints)
            {
                var img = new System.Drawing.Bitmap(Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream($"AutomatedCar.Assets.WorldObjects.{rp.Type}.png"));
                var x = rp.Y / (double)img.Size.Width * 100.0;
                var y = rp.X / (double)img.Size.Height * 100.0;
                result.Add(rp.Type, x.ToString("0.00", nfi) + "%," + y.ToString("0.00", nfi) + "%");
            }

            return result;
        }

        // https://math.stackexchange.com/questions/3349681/angle-from-2x2-rotation-matrix
        // https://en.wikipedia.org/wiki/Rotation_matrix#In_two_dimensions
        private double RotationMatrixToDegree(float m11, float m12)
        {
            // return Math.Atan2(m11, m12) * (180.0 / Math.PI);
            var result = Math.Acos(m11) * (180.0 / Math.PI);
            if (m12 < 0)
            {
                result = 360 - result;
            }

            return result;
        }

        private int DetermineZIndex(string type)
        {
            int result = 1;
            if (type == "crosswalk")
            {
                result = 5;
            }
            if (type == "tree")
            {
                result = 20;
            }

            return result;
        }

        private bool DetermineCollidablity(string type)
        {
            List<string> collideables = new List<string> { "boundary", "garage", "parking_bollard",
                "roadsign_parking_right", "roadsign_priority_stop", "roadsign_speed_40", "roadsign_speed_50", "roadsign_speed_60", "tree" };
            bool result = false;
            if (collideables.Contains(type))
            {
                result = true;
            }

            return result;
        }

        private WorldObjectType DetermineType(string type)
        {
            WorldObjectType result = WorldObjectType.Other;
            switch (type)
            {
                case "boundary":
                    result = WorldObjectType.Boundary;
                    break;
                case "garage":
                    result = WorldObjectType.Building;
                    break;
                case string s when s.StartsWith("car_"):
                    result = WorldObjectType.Car;
                    break;
                case "crosswalk":
                    result = WorldObjectType.Crosswalk;
                    break;
                case string s when s.StartsWith("parking_space_"):
                    result = WorldObjectType.ParkingSpace;
                    break;
                case string s when s.StartsWith("road_"):
                    result = WorldObjectType.Road;
                    break;
                case string s when s.StartsWith("roadsign_"):
                    result = WorldObjectType.RoadSign;
                    break;
                case "tree":
                    result = WorldObjectType.Tree;
                    break;
                default:
                    result = WorldObjectType.Other;
                    break;
            }

            return result;
        }

        public GraphicsPath AddGeometry()
        {
            GraphicsPath geom = new();
            List<Point> points = new();
            points.Add(new Point(50, 50));
            points.Add(new Point(50, 100));
            points.Add(new Point(100, 50));
            points.Add(new Point(50, 50));
            geom.AddPolygon(points.ToArray());
            geom.CloseFigure();

            // geom.PathPoints

            return geom;
        }
    }
}
