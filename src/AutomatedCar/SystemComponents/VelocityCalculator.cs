namespace AutomatedCar.SystemComponents
{
    using System;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using DynamicData.Kernel;

    internal class VelocityCalculator : SystemComponent
    {
        private readonly AutomatedCar car;

        //Constants: //Velocity Limits based on the car's capabilities:

        /// <summary>
        /// max sebesség előremenetben: 130 km/h.
        /// </summary>
        private static readonly Speed maxVelocityForward = Speed.FromKmPerHour(130);

        /// <summary>
        /// max sebesség tolatáskor: 20 km/h.
        /// </summary>
        private static readonly Speed maxVelocityBackward = Speed.FromKmPerHour(20);

        public VelocityCalculator(VirtualFunctionBus virtualFunctionBus, AutomatedCar car)
            : base(virtualFunctionBus)
        {
            this.car = car;
        }

        public override void Process()
        {
            var acceleration = this.virtualFunctionBus.AccelerationPacket.Acceleration;
            var currentSpeed = World.Instance.ControlledCar.Velocity.InPixelsPerTick();

            var newVelocity = currentSpeed + acceleration;
            newVelocity = Math.Max(0.0, newVelocity);
            newVelocity = Math.Min(newVelocity, maxVelocityForward.InPixelsPerTick());

            this.car.Velocity = Speed.FromPixelsPerTick(newVelocity);
        }
    }
}