namespace AutomatedCar.Models
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

    public class World
    {
        //privát adattagok:
        private int controlledCarPointer = 0;
        private DebugStatus debugStatus = new DebugStatus();        //itt miért kell?

        //public adattagok:
        public List<WorldObject> WorldObjects { get; set; } = new List<WorldObject>();

        public List<AutomatedCar> controlledCars = new();

        public int ControlledCarPointer                         //az AutomatedCar-ok közül kiválasztjuk azt amelyiket irányítani szeretnénk
        {
            get => this.controlledCarPointer;
            set
            {
                this.controlledCarPointer = value;
            }
        }

        public AutomatedCar ControlledCar                       //visszaadjuk a pointerral kijelölt AutomatedCar-t
        {
            get => this.controlledCars[this.controlledCarPointer];
        }

        public int Width { get; set; }                          //szélesség, magasság

        public int Height { get; set; }


        //publikus függvények:-----------------------------------------------------------------------------------------------------------------------------------------------

        public static World Instance { get; } = new World();    //singleton pattern : biztosítja, hogy a World osztályból csak egyetlen példány létezzen az alkalmazás futása során.
                                                                //Használata : World world = World.Instance;

        public void AddObject(WorldObject worldObject)
        {
            this.WorldObjects.Add(worldObject);
        }
        public void AddControlledCar(AutomatedCar controlledCar)
        {
            this.controlledCars.Add(controlledCar);
            this.AddObject(controlledCar);
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
        }

        public void PopulateFromJSON(string filename)                       //test_world.json-re hívja.
        {
            //az alábbi 3 dictionary-re igaznak kell lennie: minden disctionary-ben minden típus (Assets mappa minden png-je) pontosan egyszer szerepel. 
            var rotationPoints = this.ReadRotationsPoints();                     //Dictionary<"típus.png",(int x,int y)>        //reference_points.json -re hívva
            var renderTransformOrigins = this.CalculateRenderTransformOrigins(); //Dictionary<"típus.png", "x%,y%">             //reference_points.json -re hívva
            var worldObjectPolygons = this.ReadPolygonJSON();                    //Dictionary<"típus.png",List<PolylineGeometry>>  //worldobject_polygons.json -re hívva

            //kinyitjuk a test_world.json-t és beolvasunk egy RawWorld-öt: (Emlékeztető: RawWorld tulajdonságai: width,height, List<RawWorldObject> Objects)
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(filename));

            RawWorld rawWorld = JsonConvert.DeserializeObject<RawWorld>(reader.ReadToEnd());
            this.Height = rawWorld.Height;
            this.Width = rawWorld.Width;

            //végigmegyünk a RawWorld-ben lévő RawWorldObject-eken.   (Emlékeztető: RawWorldObject tulajdonságai: string type, x, y, 2x2-es mátrix)
            foreach (RawWorldObject rwo in rawWorld.Objects)
            {
                //ctor:
                var wo = new WorldObject(rwo.X, rwo.Y, rwo.Type + ".png", this.DetermineZIndex(rwo.Type), this.DetermineCollidablity(rwo.Type), this.DetermineType(rwo.Type));

                //RotationPoint:
                (int x, int y) rp = (0, 0);

                if (rotationPoints.ContainsKey(rwo.Type))
                {
                    rp = rotationPoints[rwo.Type];
                }

                wo.RotationPoint = new System.Drawing.Point(rp.x, rp.y);

                //RenderTransformOrigin:
                string rto = "0,0";

                if (renderTransformOrigins.ContainsKey(rwo.Type))
                {
                    rto = renderTransformOrigins[rwo.Type];
                }

                wo.RenderTransformOrigin = rto;

                //Rotation szög kiszedése a forgató mátrixból:
                wo.Rotation = this.RotationMatrixToDegree(rwo.M11, rwo.M12);


                //kikeressük a WorldObjectet típus.png alapján a worldObjectPolygons dictionary-ból:
                if (worldObjectPolygons.ContainsKey(rwo.Type))
                {
                    // deep copy
                    foreach (var g in worldObjectPolygons[rwo.Type])
                    {
                        wo.Geometries.Add(new PolylineGeometry(g.Points, false));
                        wo.RawGeometries.Add(new PolylineGeometry(g.Points, false));        //RawGeometries = Geometries kezdetben
                    }

                    // apply rotation -> mindegyik geometriát elforgatjuk a RotationPoint körül Rotation szögben.
                    foreach (var geometry in wo.Geometries)
                    {
                        //az mx2 mátrix 3x3-mas és a sorai a következők: {m11 m12 0}, {m21 m22 0}, {dx dy 1},
                        //ahol dx=wo.RotationPoint.X, dy=wo.RotationPoint.Y. A mátrix által megadott transzformáció a következő: elforgat a RotationPoint pont körül, Rotation fokkal
                        var mx2 = new System.Drawing.Drawing2D.Matrix(rwo.M11, rwo.M12, rwo.M21, rwo.M22, wo.RotationPoint.X, wo.RotationPoint.Y);

                        var gpa2 = this.ToDotNetPoints(geometry.Points).ToArray(); //a geometry.Points tulajdonság Avalonia.Point-okat tartalmaz.
                                                                                   //Ezeket alakítjuk át .Net pontokká, és tesszük Array-ba.
                                                                                   //.Net pontok: List<PointF>, ahol PointF=(float,float)
                        //az mx2 mátrixot hattatjuk a gpa2 pontokra
                        mx2.TransformPoints(gpa2);

                        //visszaalakítjuk a gpa2-t Avalonia.Points-ra, és beállítjuk a geometry.Points tulajdonságot
                        geometry.Points = this.ToAvaloniaPoints(gpa2);
                    }
                }

                this.AddObject(wo);
            }
        }

        //privát függvények: mindet kizárólag csak a PopulateFromJSON() használja:---------------------------------------------------------------------------------------------------
        private List<System.Drawing.PointF> ToDotNetPoints(IList<Avalonia.Point> points)
        {
            var result = new List<System.Drawing.PointF>();
            foreach (var p in points)
            {
                result.Add(new PointF(Convert.ToSingle(p.X), Convert.ToSingle(p.Y)));
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
                result.Add(rp.Type, (rp.X, rp.Y));          //rp.Type = png fájl neve
            }

            return result;
        }

        // It accepts different string values than WPF.
        // For .5,.5 you actually need 50%,50%. .5,.5 is treated as "half of the logical pixel" in both directions instead of "half of the control"
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

        private Dictionary<string, List<PolylineGeometry>> ReadPolygonJSON(string filename = "worldobject_polygons.json")
        {
            // TODO: Avalonia specific
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"AutomatedCar.Assets.{filename}"));

            var objects = JsonConvert.DeserializeObject<Dictionary<string, List<RawWorldObjectPolygon>>>(reader.ReadToEnd())["objects"];    //Dictionary helyett lehetne List<RawWorldObjectPolygon> is
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

                result.Add(rwop.Type, polygonList);     //rwop.Type = png fájl neve
            }

            return result;
        }


        //Determine: ZIndex, Collidability, Type -> mind a png fájl neve alapján:
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
    }
}
