namespace AutomatedCar.SystemComponents
{
    using System;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    internal class SteeringWheel : SystemComponent
    {
        public SteeringWheelPacket SteeringWheelPacket { get; }

        public SteeringWheel(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.SteeringWheelPacket = new SteeringWheelPacket();
            this.virtualFunctionBus.SteeringWheelPacket = this.SteeringWheelPacket;
        }

        public override void Process()
        {
            if (Keyboard.IsKeyDown(Avalonia.Input.Key.Left))
            {
                this.SteeringWheelPacket.SteeringWheelState = Math.Max(-450, this.SteeringWheelPacket.SteeringWheelState - 1);
            }

            if (Keyboard.IsKeyDown(Avalonia.Input.Key.Right))
            {
                this.SteeringWheelPacket.SteeringWheelState = Math.Min(450, this.SteeringWheelPacket.SteeringWheelState + 1);
            }
        }
    }
}
