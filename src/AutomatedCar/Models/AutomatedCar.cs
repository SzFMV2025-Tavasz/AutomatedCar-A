namespace AutomatedCar.Models
{
    using SystemComponents.Sensors;
    using Avalonia.Media;
    using SystemComponents;
    using global::AutomatedCar.Helpers;
    using System.Numerics;
    using Avalonia;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using System.Reflection.PortableExecutable;

    public class AutomatedCar : Car
    {
        private VirtualFunctionBus virtualFunctionBus;

        private Speed velocity;

        private double xD;
        private double yD;

        [Obsolete("VirtualFunctionBus.InputHandlerPacket.Throttling be used instead.")]
        public bool ThrottleOn
        {
            get => this.virtualFunctionBus.InputHandlerPacket.Throttling;
        }

        [Obsolete("VirtualFunctionBus.InputHandlerPacket.Braking be used instead.")]
        public bool BrakeOn
        {
            get => this.virtualFunctionBus.InputHandlerPacket.Braking;
        }

        [Obsolete("Should check the transmission instead.")]
        public bool ReverseOn { get; } = false;

        [Obsolete("VirtualFunctionBus.InputHandlerPacket.SteeringLeft be used instead.")]
        public bool SteeringLeft
        {
            get => this.virtualFunctionBus.InputHandlerPacket.SteeringLeft;
        }

        [Obsolete("VirtualFunctionBus.InputHandlerPacket.SteeringRight be used instead.")]
        public bool SteeringRight
        {
            get => this.virtualFunctionBus.InputHandlerPacket.SteeringRight;
        }

        /// <summary>
        /// Gets or sets the input requests requested by the different system components.
        /// </summary>
        public Dictionary<InputRequesterComponent, Dictionary<InputRequestType, bool>> InputRequests { get; }

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.InputRequests = [];

            this.virtualFunctionBus = new VirtualFunctionBus();
            this.virtualFunctionBus.RegisterComponent(new InputHandler(this.virtualFunctionBus, this));
            this.virtualFunctionBus.RegisterComponent(new SteeringWheel(this.virtualFunctionBus, this));
            this.virtualFunctionBus.RegisterComponent(new Powertrain(this.virtualFunctionBus, this));

            //sorrend fontos. Amilyen sorrendben vannak hozzáadva a VFB-hez, olyan sorrendben hívódnak meg az egyes Process() függvények.
            //ctor-okat meg kell oldani h működjenek:
            this.virtualFunctionBus.RegisterComponent(new AccelerationCalculator(virtualFunctionBus, this));
            this.virtualFunctionBus.RegisterComponent(new VelocityCalculator(virtualFunctionBus, this));
            this.virtualFunctionBus.RegisterComponent(new PedalsCalculator(virtualFunctionBus, this));

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

        /// <summary>
        /// Sets the requested input for the given component.
        /// <para>
        /// The setting does not reset automatically.
        /// This means that it is possible to request steering left and steering right at the same time.
        /// The behaviour of contradictory requests from the same component are inconsistent and not recommended.
        /// </para>
        /// </summary>
        /// <param name="component">The component that is requesting the input.</param>
        /// <param name="type">The type of the input.</param>
        /// <param name="isOn">Whether the input is requested or not.</param>
        public void RequestInput(InputRequesterComponent component, InputRequestType type, bool isOn)
        {
            if (!this.InputRequests.TryGetValue(component, out var typeDict))
            {
                typeDict = [];
                this.InputRequests[component] = typeDict;
            }

            typeDict[type] = isOn;
        }
    }
}