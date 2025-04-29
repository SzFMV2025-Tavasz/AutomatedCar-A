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
        private Speed velocity = new Speed(0); //0 kezdőértéket szeretnék beállítani
        public bool ThrottleOn { get; set; } = false;
        public bool BrakeOn { get; set; } = false;
        public bool ReverseOn { get; set; } = false;
        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.virtualFunctionBus.RegisterComponent(new SteeringWheel(virtualFunctionBus));

            //sorrend fontos. Amilyen sorrendben vannak hozzáadva a VFB-hez, olyan sorrendben hívódnak meg az egyes Process() függvények:
            this.virtualFunctionBus.RegisterComponent(new AccelerationCalculator(virtualFunctionBus));
            this.virtualFunctionBus.RegisterComponent(new VelocityVectorCalculator(virtualFunctionBus));
            this.virtualFunctionBus.RegisterComponent(new PedalsCalculator(virtualFunctionBus));

            this.ZIndex = 10;
        }

        public VirtualFunctionBus VirtualFunctionBus { get => this.virtualFunctionBus; }

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