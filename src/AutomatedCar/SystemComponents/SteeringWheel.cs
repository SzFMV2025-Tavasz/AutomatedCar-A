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
                this.SteeringWheelPacket.State = Math.Max(-450, this.SteeringWheelPacket.State - 1);
            }

            if (Keyboard.IsKeyDown(Avalonia.Input.Key.Right))
            {
                this.SteeringWheelPacket.State = Math.Min(450, this.SteeringWheelPacket.State + 1);
            }
        }
    }
}
