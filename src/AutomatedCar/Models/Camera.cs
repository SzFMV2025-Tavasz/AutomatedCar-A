namespace AutomatedCar.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Avalonia;
    using Avalonia.Media;
    using global::AutomatedCar.Helpers;
    using global::AutomatedCar.SystemComponents;
    using global::AutomatedCar.SystemComponents.Packets;

    /// <summary>
    /// Camera sensor to determine lanes and closer objects in front of the car.
    /// The sensor is placed at the back of the windshield.
    /// </summary>
    public class Camera : SystemComponent
    {
        /// <summary>
        /// The length of the camera's vision.
        /// </summary>
        private const int VisionLength = 80;

        // TODO: Implement this

        /// <summary>
        /// The offset of the camera from the car's center.
        /// </summary>
        private const int CameraOffset = 0;

        private readonly VirtualFunctionBus functionBus;
        private readonly Car controlledCar;
        private readonly TriangleDetector triangleDetector;
        private readonly IEnumerable<WorldObject> worldObjects;
        private IEnumerable<WorldObject> previusTickWorldObjects;
        private double rotation;

        /// <summary>
        /// The camera's position.
        /// </summary>
        private (int x, int y) camera;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="controlledCar">The controlled <see cref="Car"/> object.</param>
        /// <param name="worldObjects">The <see cref="WorldObject"/> collection, to reach all other sprites.</param>
        /// <param name="triangleDetector">The <see cref="TriangleDetector"/> object to detect triangles.</param>
        public Camera(AutomatedCar controlledCar, IEnumerable<WorldObject> worldObjects)
            : base(controlledCar.VirtualFunctionBus)
        {
            this.camera = (controlledCar.X, controlledCar.Y);
            this.controlledCar = controlledCar;
            this.worldObjects = worldObjects;
            this.triangleDetector = new TriangleDetector(VisionLength);
            this.previusTickWorldObjects = worldObjects.ToList();
            this.rotation = controlledCar.Rotation - 90;
            this.functionBus = controlledCar.VirtualFunctionBus;

            this.controlledCar.PropertyChangedEvent += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case "X":
                        this.UpdateCoordinates();
                        break;
                    case "Y":
                        this.UpdateCoordinates();
                        break;
                    case "Rotation":
                        this.rotation = this.controlledCar.Rotation + 90;
                        break;
                    default:
                        break;
                }
            };
        }

        /// <summary>
        /// Gets or sets the x coordinate of the <see cref="Camera"/>.
        /// </summary>
        /// <remarks>
        /// If both coordinates are changing, use <see cref="CameraCoordinates"/>.
        /// </remarks>
        public int X
        {
            get => this.camera.x;
            set
            {
                this.camera.x = value;
            }
        }

        /// <summary>
        /// Gets or sets the y coordinate of the <see cref="Camera"/>.
        /// </summary>
        /// <remarks>
        /// If both coordinates are changing, use <see cref="CameraCoordinates"/>.
        /// </remarks>
        public int Y
        {
            get => this.camera.y;
            set
            {
                this.camera.y = value;
            }
        }

        /// <summary>
        /// Gets or sets both coordinates of the <see cref="Camera"/>.
        /// If both coordinates are changing, use this setter.
        /// </summary>
        public (int x, int y) CameraCoordinates
        {
            get => this.camera;
            set
            {
                this.camera = value;
            }
        }

        /// <summary>
        /// Gets or sets the rotation of the <see cref="Camera"/>.
        /// </summary>
        public double Rotation { get => this.rotation; set => this.rotation = value; }

        /// <summary>
        /// Gets the rotation of the <see cref="Camera"/> in radians.
        /// </summary>
        public double RotationInRadian => this.rotation / 180 * Math.PI;

        /// <summary>
        /// Processes the camera's view.
        /// </summary>
        /// <remarks>
        /// Runs in every tick.
        /// </remarks>
        public override void Process()
        {
            this.functionBus.CameraPackets = new List<IReadOnlyCameraPacket>();

            IEnumerable<WorldObject> visibleObjects = this.triangleDetector.ScanVisibleObjects(
                    this.worldObjects,
                    (this.controlledCar.X, this.controlledCar.Y),
                    this.camera,
                    this.rotation);

            foreach (var obj in visibleObjects)
            {
                var relativeSpeed = this.CalculateRelativeSpeedVector(this.controlledCar.Speed, this.CalculateSpeed(obj), this.rotation, obj.Rotation, out double relativeAngle);
                this.functionBus.CameraPackets.Add(
                    new CameraPacket
                    {
                        Distance = this.CalculateClosestPoint(obj),
                        Angle = relativeAngle,
                        RelativeSpeed = relativeSpeed,
                        ObjectType = obj.WorldObjectType,
                        Collideable = obj.Collideable,
                    });
            }

            this.previusTickWorldObjects = this.worldObjects.Select(o =>
                new WorldObject(o.X, o.Y, o.Filename, o.ZIndex, o.Collideable, o.WorldObjectType)
                {
                    Rotation = o.Rotation,
                    RotationPoint = o.RotationPoint,
                    RenderTransformOrigin = o.RenderTransformOrigin,
                    Geometries = new List<PolylineGeometry>(o.Geometries.Select(g =>
                        new PolylineGeometry(g.Points.Select(p => new Point(p.X, p.Y)), g.IsFilled))),
                    RawGeometries = new List<PolylineGeometry>(o.RawGeometries.Select(g =>
                        new PolylineGeometry(g.Points.Select(p => new Point(p.X, p.Y)), g.IsFilled))),
                });
        }

        private double CalculateClosestPoint(WorldObject obj)
        {
            return obj
                .Geometries
                .FirstOrDefault()
                .Points
                .Min(p =>
                    Math.Sqrt(
                        Math.Pow(p.X - this.camera.x, 2) + Math.Pow(p.Y - this.camera.y, 2)));
        }

        private void UpdateCoordinates()
        {
            var x = this.controlledCar.X + (int)(CameraOffset * Math.Cos(this.rotation));
            var y = this.controlledCar.Y + (int)(CameraOffset * Math.Sin(this.rotation));
            this.CameraCoordinates = (x, y);
        }

        private double CalculateRelativeSpeedVector(double speed, double obj, double relativeAngle, double rotation, out double relativeAngle1)
        {
            throw new NotImplementedException();
        }

        private double CalculateSpeed(WorldObject obj)
        {
            if (obj is Car car)
            {
                return car.Speed;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
