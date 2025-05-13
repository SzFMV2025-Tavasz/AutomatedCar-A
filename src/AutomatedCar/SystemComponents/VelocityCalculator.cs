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
        /// max sebesség előremenetben: max 130 km/h.
        /// </summary>
        private static readonly SpeedHelper maxVelocityForward = SpeedHelper.FromKmPerHour(130);

        /// <summary>
        /// max sebesség tolatáskor: max 20 km/h.
        /// </summary>
        private static readonly SpeedHelper maxVelocityBackward = SpeedHelper.FromKmPerHour(20);

        public VelocityCalculator(VirtualFunctionBus virtualFunctionBus, AutomatedCar car)
            : base(virtualFunctionBus)
        {
            this.car = car;
        }

        public override void Process()
        {
            var acceleration = this.virtualFunctionBus.AccelerationPacket.Acceleration;
            var currentSpeed = this.car.Velocity.InPixelsPerTick();
            var newVelocity = Math.Max(0, currentSpeed + acceleration);     //velocity cannot be negative

            if(this.car.ReverseOn)
            {
                newVelocity = Math.Min(newVelocity, maxVelocityBackward.InPixelsPerTick());
            }
            else
            {
                newVelocity = Math.Min(newVelocity, maxVelocityForward.InPixelsPerTick());
            }

            this.car.Velocity = SpeedHelper.FromPixelsPerTick(newVelocity);
        }
    }
}