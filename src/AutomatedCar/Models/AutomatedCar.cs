namespace AutomatedCar.Models
{
    using SystemComponents.Sensors;
    using Avalonia.Media;
    using SystemComponents;
    using global::AutomatedCar.Helpers;
    using System.Numerics;
    using Avalonia;
    using System;

    public class AutomatedCar : Car
    {
        private VirtualFunctionBus virtualFunctionBus;
        private Speed velocity;
        private Vector2 direction;

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.virtualFunctionBus.RegisterComponent(new SteeringWheel(virtualFunctionBus));
            this.ZIndex = 10;
        }

        public VirtualFunctionBus VirtualFunctionBus { get => this.virtualFunctionBus; }

        /// <summary>
        /// The revolution of the engine in the car.
        /// </summary>
        public int Revolution { get; set; }

        /// <summary>
        /// The speed of the car.
        /// </summary>
        public Speed Velocity
        {
            get
            {
                return this.velocity;
            }

            set
            {
                this.velocity = value;
                this.Speed = (int)value.InPixelsPerSecond();
            }
        }

        /// <summary>
        /// The direction in which the car is facing. Always has the length of 1.
        /// </summary>
        public Vector2 Direction
        {
            get
            {
                return this.direction;
            }

            set
            {
                this.direction = Vector2.Normalize(value);

                float angleRadians = (float)Math.Atan2(value.Y, value.X);
                this.Rotation = angleRadians * (180 / Math.PI);
            }
        }

        public PolylineGeometry Geometry { get; set; }

        /// <summary>Starts the automated cor by starting the ticker in the Virtual Function Bus, that cyclically calls the system components.</summary>
        public void Start()
        {
            this.virtualFunctionBus.Start();
        }

        /// <summary>Stops the automated cor by stopping the ticker in the Virtual Function Bus, that cyclically calls the system components.</summary>
        public void Stop()
        {
            this.virtualFunctionBus.Stop();
        }
    }
}