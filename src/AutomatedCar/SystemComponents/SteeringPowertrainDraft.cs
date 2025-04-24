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

        private readonly AutomatedCar car;

        public SteeringPowertrainDraft(VirtualFunctionBus virtualFunctionBus, AutomatedCar car)
            : base(virtualFunctionBus)
        {
            this.car = car;
        }

        public override void Process()
        {
            // Calculating with edge cases
            int frontWheelRotation = this.virtualFunctionBus.SteeringWheelPacket.FrontWheelState;
            if (frontWheelRotation == 0)
            {
                return;
            }

            var rotationOffset = this.CalculateRotationOffsetToRearWheels(FrontRearWheelsDistance, frontWheelRotation);

            // var angularVelocity = this.CalculateAngularVelocity(rotationOffset, Speed.FromPixelsPerSecond(this.car.Speed));
            var angularVelocity = this.CalculateAngularVelocity(rotationOffset, Speed.FromPixelsPerSecond(180));
            var turningRotationPoint = this.CalculateNewRotationPoint(this.car.RotationPoint, rotationOffset, this.car.Rotation);

            // Rotate car
            this.car.Rotation += angularVelocity;

            // Move car
            var moveVector = this.CalculateMoveVector(
                new PointF((float)this.car.XD, (float)this.car.YD),
                turningRotationPoint,
                (float)angularVelocity);
            this.car.XD += moveVector.X;
            this.car.YD += moveVector.Y;
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
            var offset = (int)(carLength / Math.Tan(wheelRotationAbs.ToRadian()));

            return wheelRotation < 0 ? -offset : offset;
        }

        /// <summary>
        /// Calculates the angular velocity of the car.
        /// </summary>
        /// <param name="radius">The turning radius in pixels.</param>
        /// <param name="speed">The speed of the car.</param>
        /// <returns>The angular velocity in degrees.</returns>
        private double CalculateAngularVelocity(int radius, Speed speed)
        {
            var angularVelocity = (speed.InPixelsPerTick() / radius).ToDegree();
            return angularVelocity;
        }

        /// <summary>
        /// Calculates the movement of the car.
        /// </summary>
        /// <param name="carPosition">The current position of the car.</param>
        /// <param name="rotationPoint">The point which the car rotates around.</param>
        /// <param name="angularVelocity">The angular velocity of the car.</param>
        /// <returns>The movement vector of the car.</returns>
        private Vector2 CalculateMoveVector(PointF carPosition, PointF rotationPoint, float angularVelocity)
        {
            float angularVelocityRad = (float)((double)angularVelocity).ToRadian();

            var absoluteRotationPoint = this.AbsoluteRotationPoint(carPosition, rotationPoint);
            var carOldPositionToRotationPoint = new Vector2(absoluteRotationPoint.X - carPosition.X, absoluteRotationPoint.Y - carPosition.Y);
            var rotationPointToCarNewPosition = new Vector2(carPosition.X - absoluteRotationPoint.X, carPosition.Y - absoluteRotationPoint.Y).Rotate(angularVelocityRad);

            return new Vector2(carOldPositionToRotationPoint.X + rotationPointToCarNewPosition.X, carOldPositionToRotationPoint.Y + rotationPointToCarNewPosition.Y);
        }

        /// <summary>
        /// Calculates the absolute coordinates of the rotation point of the car (since it is relative to the position of the car).
        /// </summary>
        /// <param name="carPosition">The position of the car.</param>
        /// <param name="rotationPoint">The relative rotation point of the car.</param>
        /// <returns>The absolut coordinates of the rotation point.</returns>
        private PointF AbsoluteRotationPoint(PointF carPosition, PointF rotationPoint)
        {
            return new PointF(carPosition.X + rotationPoint.X, carPosition.Y + rotationPoint.Y);
        }

        /// <summary>
        /// Calculates where the new rotation point is relative to the car.
        /// </summary>
        /// <param name="oldRotationPoint">The original rotation point of the car.</param>
        /// <param name="rotationOffset">The offset of the rotation point relative to the rear wheels.</param>
        /// <param name="carRotation">The current rotation of the car.</param>
        /// <returns>The new rotation point is relative to the car.</returns>
        private Point CalculateNewRotationPoint(Point oldRotationPoint, int rotationOffset, double carRotation)
        {
            var rotatedRotationPoint =
                new Vector2(rotationOffset + 54 - oldRotationPoint.X, CarFrontRearWheelsDistance - oldRotationPoint.Y)
                .Rotate((float)carRotation.ToRadian());
            return new Point((int)rotatedRotationPoint.X, (int)rotatedRotationPoint.Y);
        }
    }
}
