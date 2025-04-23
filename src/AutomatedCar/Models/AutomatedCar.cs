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

        public double Velocity { get; set; } = 0;      //in pixel/tick     //non negative real number
        public double Acceleration { get; set; }       //in pixel/tick      //real number
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