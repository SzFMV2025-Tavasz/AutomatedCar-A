namespace AutomatedCar
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using System.Runtime.ConstrainedExecution;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.ViewModels;
    using AutomatedCar.Views;
    using Avalonia;
    using Avalonia.Controls.ApplicationLifetimes;
    using Avalonia.Markup.Xaml;
    using Avalonia.Media;
    using Newtonsoft.Json.Linq;

    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var world = this.CreateWorld();
                desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel(world) };
            }

            base.OnFrameworkInitializationCompleted();
        }

        public World CreateWorld()
        {
            var world = World.Instance;

            //this.AddDummyCircleTo(world);

            world.PopulateFromJSON($"AutomatedCar.Assets.oval.json");

            this.AddNpcsTo(world);

            this.AddNpcsTo(world);

            this.AddControlledCarsTo(world);

            this.StartNpcs(world);

            return world;
        }

        private PolylineGeometry GetControlledCarBoundaryBox()
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
    .GetManifestResourceStream($"AutomatedCar.Assets.worldobject_polygons.json"));
            string json_text = reader.ReadToEnd();
            dynamic stuff = JObject.Parse(json_text);
            var points = new List<Point>();
            foreach (var i in stuff["objects"][0]["polys"][0]["points"])
            {
                points.Add(new Point(i[0].ToObject<int>(), i[1].ToObject<int>()));
            }

            return new PolylineGeometry(points, false);
        }

        private PolylineGeometry GetNpcCarBoundaryBox()
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
    .GetManifestResourceStream($"AutomatedCar.Assets.worldobject_polygons.json"));
            string json_text = reader.ReadToEnd();
            dynamic stuff = JObject.Parse(json_text);
            var points = new List<Point>();
            foreach (var i in stuff["objects"][0]["polys"][0]["points"])
            {
                points.Add(new Point(i[0].ToObject<int>(), i[1].ToObject<int>()));
            }

            return new PolylineGeometry(points, false);
        }

        private PolylineGeometry GetNpcPedestrianBoundaryBox()
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
    .GetManifestResourceStream($"AutomatedCar.Assets.worldobject_polygons.json"));
            string json_text = reader.ReadToEnd();
            dynamic stuff = JObject.Parse(json_text);
            var polygonForPed = stuff["objects"] as object;
            var points = new List<Point>();
            foreach (var i in stuff["objects"][30]["polys"][0]["points"])
            {
                points.Add(new Point(i[0].ToObject<int>(), i[1].ToObject<int>()));
            }

            return new PolylineGeometry(points, false);
        }

        private void AddDummyCircleTo(World world)
        {
            var circle = new Circle(200, 200, "circle.png", 20);

            circle.Width = 40;
            circle.Height = 40;
            circle.ZIndex = 20;
            circle.Rotation = 45;

            world.AddObject(circle);
        }

        private AutomatedCar CreateControlledCar(int x, int y, int rotation, string filename)
        {
            var controlledCar = new Models.AutomatedCar(x, y, filename);

            controlledCar.Geometry = this.GetControlledCarBoundaryBox();
            controlledCar.RawGeometries.Add(controlledCar.Geometry);
            controlledCar.Geometries.Add(controlledCar.Geometry);
            controlledCar.RotationPoint = new System.Drawing.Point(54, 120);
            controlledCar.Rotation = rotation;

            controlledCar.Start();

            return controlledCar;
        }

        private void AddControlledCarsTo(World world)
        {
            var controlledCar = this.CreateControlledCar(480, 1425, 0, "car_1_white.png");

            world.AddControlledCar(controlledCar);
        }

        private void AddNpcsTo(World world)
        {
            var npcJsonObjects = NpcLoader.ReadNpcsJson();

            foreach (var npcJsonObject in npcJsonObjects)
            {
                NpcPath path = new NpcPath(npcJsonObject);
                if (npcJsonObject.Type == NpcType.CAR)
                {
                    NpcCar car = new NpcCar(path);
                    car.Geometry = this.GetNpcCarBoundaryBox();
                    car.RawGeometries.Add(car.Geometry);
                    car.Geometries.Add(car.Geometry);
                    car.RotationPoint = new System.Drawing.Point(54, 120);
                    if ($"AutomatedCar.Assets.{car.WorldName}.json" == world.WorldName)
                    {
                        world.AddObject(car);
                    }
                }
                else
                {
                    NpcPedestrian pedestrian = new NpcPedestrian(path);
                    pedestrian.Geometry = this.GetNpcPedestrianBoundaryBox();
                    pedestrian.RawGeometries.Add(pedestrian.Geometry);
                    pedestrian.Geometries.Add(pedestrian.Geometry);
                    //pedestrian.RotationPoint = new System.Drawing.Point(54, 120);
                    if ($"AutomatedCar.Assets.{pedestrian.WorldName}.json" == world.WorldName)
                    {
                        world.AddObject(pedestrian);
                    }
                }
            }

        }
        private void StartNpcs(World world)
        {
            foreach (var npc in world.WorldObjects.OfType<Npc>())
            {
                npc.Start();
            }
        }

    }
}