namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Numerics;
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
            if (frontWheelRotation == 0)
            {
                return;
            }

            var rotationOffset = this.CalculateRotationOffsetToRearWheels(FrontRearWheelsDistance, frontWheelRotation);
            var angularVelocity = this.CalculateAngularVelocity(frontWheelRotation);

            // Rotate car
            this.car.Rotation += angularVelocity;

            //Recalculate rotation point
            Point originalRp = this.car.RotationPoint;
            Point rp = new Point(54 + rotationOffset, CarFrontRearWheelsDistance);
            this.car.RotationPoint = rp;

            // Move car
            var moveVector = this.CalculateMoveVector(
                new PointF((float)this.car.XD, (float)this.car.YD),
                this.car.RotationPoint,
                (float)angularVelocity);
            this.car.XD += moveVector.X;
            this.car.YD += moveVector.Y;

            this.car.RotationPoint = originalRp;
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

            double wheelRotationAbs = Math.Abs(wheelRotation);
            var wheelRotationRad = wheelRotationAbs * (float)(Math.PI / 180);
            var offset = (int)(carLength / Math.Tan(wheelRotationRad));

            return wheelRotation < 0 ? -offset : offset;
        }

        private double CalculateAngularVelocity(int frontWheelRotation)
        {
            var angularVelocity = 0.1;
            return frontWheelRotation < 0 ? -angularVelocity : angularVelocity;
        }

        private Vector2 CalculateMoveVector(PointF carPosition, PointF rotationPoint, float angularVelocity)
        {
            float angularVelocityRad = (float)(angularVelocity * (Math.PI / 180));

            var absoluteRotationPoint = this.AbsoluteRotationPoint(carPosition, rotationPoint);
            var carOldPositionToRotationPoint = new Vector2(absoluteRotationPoint.X - carPosition.X, absoluteRotationPoint.Y - carPosition.Y);
            var rotationPointToCarNewPosition = new Vector2(carPosition.X - absoluteRotationPoint.X, carPosition.Y - absoluteRotationPoint.Y).Rotate(angularVelocityRad);

            return new Vector2(carOldPositionToRotationPoint.X + rotationPointToCarNewPosition.X, carOldPositionToRotationPoint.Y + rotationPointToCarNewPosition.Y);
        }

        private PointF AbsoluteRotationPoint(PointF carPosition, PointF rotationPoint)
        {
            return new PointF(carPosition.X + rotationPoint.X, carPosition.Y + rotationPoint.Y);
        }
    }
}
