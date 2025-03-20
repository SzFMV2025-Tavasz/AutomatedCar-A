namespace AutomatedCar.Models
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::AutomatedCar.SystemComponents;

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

        /// <summary>
        /// The epsilon value used for pre-validation.
        /// </summary>
        /// <remarks>
        /// This is set to 80, which is the camera's vision length.
        /// </remarks>
        private const int PreValidationEpsilon = 80;

        // https://hu.wikipedia.org/wiki/H%C3%A1romsz%C3%B6g#:~:text=45%2D90%20h%C3%A1romsz%C3%B6g-,30%2D60%2D90%20h%C3%A1romsz%C3%B6g,-Szab%C3%A1lyos%20h%C3%A1romsz%C3%B6g%5B
        // Long edge (hypotenuse).
        private static readonly double VectorSize = (2 * VisionLength) / Math.Cbrt(3);
        private readonly VirtualFunctionBus functionBus;
        private readonly Car controlledCar;
        private readonly IEnumerable<WorldObject> worldObjects;
        private double rotation;

        /// <summary>
        /// The camera's position.
        /// </summary>
        private (int x, int y) camera;

        /// <summary>
        /// The left point of the camera's view (triangle).
        /// </summary>
        private (int x, int y) leftEdge;

        /// <summary>
        /// The right point of the camera's view (triangle).
        /// </summary>
        private (int x, int y) rightEdge;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="controlledCar">The controlled <see cref="Car"/> object.</param>
        /// <param name="worldObjects">The <see cref="WorldObject"/> collection, to reach all other sprites.</param>
        public Camera(AutomatedCar controlledCar, IEnumerable<WorldObject> worldObjects)
            : base(controlledCar.VirtualFunctionBus)
        {
            this.camera = (controlledCar.X, controlledCar.Y);
            this.worldObjects = worldObjects;
            this.rotation = controlledCar.Rotation - 90;
            this.functionBus = controlledCar.VirtualFunctionBus;

            this.CalculateEdges();
            this.controlledCar.PropertyChangedEvent += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case "X":
                        this.camera.x = this.controlledCar.X - 90;
                        break;
                    case "Y":
                        this.camera.y = this.controlledCar.Y - 90;
                        break;
                    case "Rotation":
                        this.rotation = this.controlledCar.Rotation;
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
        /// If both coordinates are changing, use <see cref="SetCameraCoordinates"/>.
        /// </remarks>
        public int X
        {
            get => (int)this.camera.x;
            set
            {
                this.camera.x = value;
                this.CalculateEdges();
            }
        }

        /// <summary>
        /// Gets or sets the y coordinate of the <see cref="Camera"/>.
        /// </summary>
        /// <remarks>
        /// If both coordinates are changing, use <see cref="SetCameraCoordinates"/>.
        /// </remarks>
        public int Y
        {
            get => this.camera.y;
            set
            {
                this.camera.y = value;
                this.CalculateEdges();
            }
        }

        /// <summary>
        /// Gets or sets both coordinates of the <see cref="Camera"/>.
        /// If both coordinates are changing, use this setter.
        /// </summary>
        public (int x, int y) SetCameraCoordinates
        {
            get => this.camera;
            set
            {
                this.camera = value;
                this.CalculateEdges();
            }
        }

        /// <summary>
        /// Gets or sets the rotation of the <see cref="Camera"/>.
        /// </summary>
        public double Rotation { get => this.rotation; set => this.rotation = value; }

        /// <summary>
        /// Processes the camera's view.
        /// </summary>
        /// <remarks>
        /// Runs in every tick.
        /// </remarks>
        public override void Process()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Scans the visible objects in the camera's view.
        /// </summary>
        /// <returns>The collection of the visible objects.</returns>
        private IEnumerable<WorldObject> ScanVisibleObjects()
        {
            throw new NotImplementedException();
        }

        private void CalculateEdges()
        {
            this.leftEdge = this.CalculateEdge(-30);
            this.rightEdge = this.CalculateEdge(30);
        }

        /// <summary>
        /// Calculates the edge point of the camera's view based on the given angle, the stored rotation, and the camera's position.
        /// </summary>
        /// <param name="angle">The angle in degrees to calculate the edge point.</param>
        /// <returns>The calculated edge point as a <see cref="Avalonia.Point"/>.</returns>
        private (int x, int y) CalculateEdge(int angle)
        {
            var angleInRadian = (angle + this.controlledCar.Rotation) / 180 * Math.PI;
            var x = this.camera.x + (Math.Cos(angleInRadian) * VectorSize);
            var y = this.camera.y + (VectorSize * Math.Sin(angleInRadian));

            return ((int)x, (int)y);
        }

        /// <summary>
        /// Checks if the given <see cref="WorldObject"/> is inside the camera's view.
        /// </summary>
        /// <remarks>
        /// For validation, it uses the following: The bounding box is simply the min/max of the x/y values among the 3 triangle's vertices, slightly inflated by the epsilon value.
        /// </remarks>
        private bool PreValidateObjects(WorldObject worldObject)
        {
            throw new NotImplementedException();
        }
    }
}
