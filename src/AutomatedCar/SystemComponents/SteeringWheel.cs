namespace AutomatedCar.SystemComponents
{
    using System;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    internal class SteeringWheel : SystemComponent
    {
        private const int RotationSpeed = 6;

        private AutomatedCar car;

        public SteeringWheelPacket SteeringWheelPacket { get; }

        public SteeringWheel(VirtualFunctionBus virtualFunctionBus, AutomatedCar car)
            : base(virtualFunctionBus)
        {
            this.car = car;
            this.SteeringWheelPacket = new SteeringWheelPacket();
            this.virtualFunctionBus.SteeringWheelPacket = this.SteeringWheelPacket;
        }

        public override void Process()
        {
            if (this.car.SteeringLeft)
            {
                this.SteeringWheelPacket.SteeringWheelState = Math.Max(-450, this.SteeringWheelPacket.SteeringWheelState - RotationSpeed);
            }

            if (this.car.SteeringRight)
            {
                this.SteeringWheelPacket.SteeringWheelState = Math.Min(450, this.SteeringWheelPacket.SteeringWheelState + RotationSpeed);
            }
        }
    }
}
