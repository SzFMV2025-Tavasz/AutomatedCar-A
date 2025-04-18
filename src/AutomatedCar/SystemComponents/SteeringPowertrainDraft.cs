namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;

    /// <summary>
    /// Code for correct way of steering. Should NOT be in the final product. Should be merged into Powertrain.
    /// </summary>
    internal class SteeringPowertrainDraft : SystemComponent
    {
        /// <summary>
        /// The distance between the front and rear wheels in pixels.
        /// </summary>
        private const int FrontRearWheelsDistance = 135;

        /// <summary>
        /// The distance between the front of the car and the rear wheels.
        /// </summary>
        private const int CarFrontRearWheelsDistance = 180;

        private AutomatedCar car;

        public SteeringPowertrainDraft(VirtualFunctionBus virtualFunctionBus, AutomatedCar car)
            : base(virtualFunctionBus)
        {
            this.car = car;
        }

        public override void Process()
        {
            int frontWheelRotation = this.virtualFunctionBus.SteeringWheelPacket.FrontWheelState;
            int offset = this.CalculateRotationOffsetToRearWheels(FrontRearWheelsDistance, Math.Abs(frontWheelRotation));

            Point rp = new Point(54 + offset, CarFrontRearWheelsDistance);
            this.car.RotationPoint = rp;

            this.car.Rotation += 1;

            // TODO recalculate X,Y
            // TODO calculate direction
        }

        /// <summary>
        /// Calculates the offset of the rotation point from the rear wheels.
        /// </summary>
        /// <param name="carLength">The distance between the front and rear wheels in pixels.</param>
        /// <param name="wheelRotation">The rotation of the front wheels in degrees. Must be a positive number.</param>
        /// <returns>The offset of the rotation point from the rear wheels.</returns>
        private int CalculateRotationOffsetToRearWheels(double carLength, double wheelRotation)
        {
            return (int)(carLength * Math.Tan(wheelRotation));
        }
    }
}
