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
            if (frontWheelRotation == 0) return;
            frontWheelRotation = -60;

            int offset = this.CalculateRotationOffsetToRearWheels(FrontRearWheelsDistance, Math.Abs(frontWheelRotation));
            offset = frontWheelRotation < 0 ? -offset : offset;

            var angularVelocity = 0.1;
            angularVelocity = frontWheelRotation < 0 ? -angularVelocity : angularVelocity;
            this.car.Rotation += angularVelocity;

            // TODO recalculate X,Y
            Point rp = new Point(54 + offset, CarFrontRearWheelsDistance);
            PointF newCarCoordinates = this.RotatePoint(
                new PointF((float)this.car.XD, (float)this.car.YD),
                rp,
                (float)angularVelocity);
            this.car.XD = newCarCoordinates.X;
            this.car.YD = newCarCoordinates.Y;
            
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
            if (wheelRotation == 0)
            {
                return 0;
            }

            var wheelRotationRad = wheelRotation * (float)(Math.PI / 180);
            var resultRad = carLength / Math.Tan(wheelRotationRad);

            return (int)resultRad;
        }

        private PointF CalculateNewCoordinates(PointF carPosition, int offset, float rotation)
        {
            var offsetVector = new PointF(offset, 0);

            return new PointF(carPosition.X - offsetVector.X, carPosition.Y - offsetVector.Y);
        }

        private PointF RotatePoint(PointF A, PointF O, float angleDegrees)
        {
            float angleRadians = angleDegrees * (float)(Math.PI / 180); // Convert degrees to radians
            float cosTheta = (float)Math.Cos(angleRadians);
            float sinTheta = (float)Math.Sin(angleRadians);

            float Bx = O.X + (A.X - O.X) * cosTheta - (A.Y - O.Y) * sinTheta;
            float By = O.Y + (A.X - O.X) * sinTheta + (A.Y - O.Y) * cosTheta;

            return new PointF(Bx, By);
        }
    }
}
