namespace AutomatedCar.SystemComponents
{
    using System;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using DynamicData.Kernel;

    internal class VelocityVectorCalculator : SystemComponent
    {
        private VelocityVectorPacket velocityVectorPacket;

        //Constants: //Velocity Limits based on the car's capabilities:
        private static readonly Speed maxVelocityForward = Speed.FromKmPerHour(130);   //max sebesség előremenetben: 130 km/h
        private static readonly Speed maxVelocityBackward = Speed.FromKmPerHour(20);   //max sebesség tolatáskor: 20 km/h

        public VelocityVectorCalculator(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.velocityVectorPacket = new VelocityVectorPacket();
            this.virtualFunctionBus.VelocityVectorPacket = this.velocityVectorPacket;
        }

        public override void Process()
        {
            var acceleration = virtualFunctionBus.AccelerationPacket.Acceleration;
            var currentSpeed = World.Instance.ControlledCar.Velocity.InPixelsPerTick();

            var newVelocity = Math.Max(0.0, currentSpeed + acceleration);
            newVelocity = Math.Min(newVelocity, maxVelocityForward.InPixelsPerTick());

            World.Instance.ControlledCar.Velocity = Speed.FromPixelsPerTick(newVelocity);
        }
    }
}