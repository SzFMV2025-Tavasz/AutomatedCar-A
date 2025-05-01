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

        private double xD;
        private double yD;

        public bool ThrottleOn { get; set; } = false;
        public bool BrakeOn { get; set; } = false;
        public bool ReverseOn { get; set; } = false;

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.virtualFunctionBus.RegisterComponent(new SteeringWheel(this.virtualFunctionBus, this));
            this.virtualFunctionBus.RegisterComponent(new Powertrain(this.virtualFunctionBus, this));

            //sorrend fontos. Amilyen sorrendben vannak hozzáadva a VFB-hez, olyan sorrendben hívódnak meg az egyes Process() függvények.
            //ctor-okat meg kell oldani h működjenek:
            this.virtualFunctionBus.RegisterComponent(new AccelerationCalculator(virtualFunctionBus,this));
            this.virtualFunctionBus.RegisterComponent(new VelocityCalculator(virtualFunctionBus, this));
            this.virtualFunctionBus.RegisterComponent(new PedalsCalculator(virtualFunctionBus,this));

            this.ZIndex = 10;
            this.XD = x;
            this.YD = y;
            this.Velocity = Helpers.Speed.FromPixelsPerTick(0);
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

        public double XD
        {
            get => this.xD;
            set
            {
                this.X = (int)value;
                this.xD = value;
            }
        }

        public double YD
        {
            get => this.yD;
            set
            {
                this.Y = (int)value;
                this.yD = value;
            }
        }

        public bool SteeringLeft { get; set; }

        public bool SteeringRight { get; set; }

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