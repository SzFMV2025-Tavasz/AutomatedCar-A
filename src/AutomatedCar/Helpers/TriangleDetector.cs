namespace AutomatedCar.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutomatedCar.Models;

    /// <summary>
    /// Detects objects within a triangular vision area.
    /// </summary>
    public class TriangleDetector
    {
        private const int BiggestObjectSize = 20;
        private readonly double visionHypotenuse;
        private readonly int preValidationEpsilon;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriangleDetector"/> class.
        /// </summary>
        /// <param name="visionLength">The length of the vision area.</param>
        public TriangleDetector(int visionLength)
        {
            this.visionHypotenuse = (2 * visionLength) / Math.Cbrt(3);
            this.preValidationEpsilon = (int)Math.Ceiling(this.visionHypotenuse + BiggestObjectSize);
        }

        /// <summary>
        /// Scans for visible objects within the triangular vision area.
        /// </summary>
        /// <param name="worldObjects">The collection of world objects to scan.</param>
        /// <param name="sensorCarrierPosition">The position of the sensor carrier.</param>
        /// <param name="sensorPosition">The position of the sensor.</param>
        /// <param name="sensorRotation">The rotation of the sensor.</param>
        /// <returns>A collection of visible world objects.</returns>
        public IEnumerable<WorldObject> ScanVisibleObjects(IEnumerable<WorldObject> worldObjects, (int x, int y) sensorCarrierPosition, (int x, int y) sensorPosition, double sensorRotation)
        {
            this.CalculateEdges(sensorCarrierPosition, sensorRotation, out var leftEdge, out var rightEdge);
            var validatedObjects = worldObjects
                .Where(currentObject =>
                    this.PreValidateObjects(currentObject, sensorPosition, leftEdge, rightEdge, sensorCarrierPosition)
                    && currentObject.Collideable);

            foreach (var obj in validatedObjects)
            {
                var checkSide1 = Side(sensorPosition.x, sensorPosition.y, leftEdge.x, leftEdge.y, obj.X, obj.Y) >= 0;
                var checkSide2 = Side(leftEdge.x, leftEdge.y, rightEdge.x, rightEdge.y, obj.X, obj.Y) >= 0;
                var checkSide3 = Side(rightEdge.x, rightEdge.y, sensorPosition.x, sensorPosition.y, obj.X, obj.Y) >= 0;

                if (checkSide1 && checkSide2 && checkSide3)
                {
                    yield return obj;
                }
            }
        }

        /// <summary>
        /// Calculates the side of a point relative to a line segment defined by two points.
        /// </summary>
        /// <param name="x1">The x-coordinate of the first point of the line segment.</param>
        /// <param name="y1">The y-coordinate of the first point of the line segment.</param>
        /// <param name="x2">The x-coordinate of the second point of the line segment.</param>
        /// <param name="y2">The y-coordinate of the second point of the line segment.</param>
        /// <param name="x">The x-coordinate of the point to check.</param>
        /// <param name="y">The y-coordinate of the point to check.</param>
        /// <returns>A positive value if the point is on one side of the line, a negative value if on the other side, and zero if on the line.</returns>
        private static int Side(int x1, int y1, int x2, int y2, int x, int y)
        {
            return ((y2 - y1) * (x - x1)) + ((-x2 + x1) * (y - y1));
        }

        /// <summary>
        /// Calculates the left and right edges of the triangular vision area.
        /// </summary>
        /// <param name="sensorCarrierPosition">The position of the sensor carrier.</param>
        /// <param name="sensorRotation">The rotation of the sensor.</param>
        /// <param name="leftEdge">The calculated left edge.</param>
        /// <param name="rightEdge">The calculated right edge.</param>
        private void CalculateEdges((int x, int y) sensorCarrierPosition, double sensorRotation, out (int x, int y) leftEdge, out (int x, int y) rightEdge)
        {
            leftEdge = this.CalculateEdge(30, sensorCarrierPosition, sensorRotation);
            rightEdge = this.CalculateEdge(-30, sensorCarrierPosition, sensorRotation);
        }

        /// <summary>
        /// Calculates the edge of the triangular vision area at a given angle.
        /// </summary>
        /// <param name="angle">The angle of the edge.</param>
        /// <param name="sensorCarrierPosition">The position of the sensor carrier.</param>
        /// <param name="sensorRotation">The rotation of the sensor.</param>
        /// <returns>The calculated edge position.</returns>
        private (int x, int y) CalculateEdge(int angle, (int x, int y) sensorCarrierPosition, double sensorRotation)
        {
            var angleInRadian = (angle + sensorRotation) / 180 * Math.PI;
            var x = sensorCarrierPosition.x + (Math.Cos(angleInRadian) * this.visionHypotenuse);
            var y = sensorCarrierPosition.y + (Math.Sin(angleInRadian) * this.visionHypotenuse);

            return ((int)x, (int)y);
        }

        /// <summary>
        /// Pre-validates objects to check if they are within the bounding box of the camera's view.
        /// </summary>
        /// <param name="worldObject">The world object to validate.</param>
        /// <param name="sensorPosition">The position of the sensor.</param>
        /// <param name="leftEdge">The left edge of the vision area.</param>
        /// <param name="rightEdge">The right edge of the vision area.</param>
        /// <param name="sensorCarrierPosition">The position of the sensor carrier.</param>
        /// <returns>True if the object is within the bounding box, otherwise false.</returns>
        private bool PreValidateObjects(WorldObject worldObject, (int x, int y) sensorPosition, (int x, int y) leftEdge, (int x, int y) rightEdge, (int x, int y) sensorCarrierPosition)
        {
            // Check if the world object is within the bounding box of the camera's view, slightly inflated by the epsilon value.
            return worldObject.X > Math.Min(sensorPosition.x, Math.Min(leftEdge.x, rightEdge.x)) - this.preValidationEpsilon
                && worldObject.X < Math.Max(sensorPosition.x, Math.Max(leftEdge.x, rightEdge.x)) + this.preValidationEpsilon
                && worldObject.Y > Math.Min(sensorPosition.y, Math.Min(leftEdge.y, rightEdge.y)) - this.preValidationEpsilon
                && worldObject.Y < Math.Max(sensorPosition.y, Math.Max(leftEdge.y, rightEdge.y)) + this.preValidationEpsilon
                && !(worldObject.X == sensorCarrierPosition.x && worldObject.Y == sensorCarrierPosition.y);
        }
    }
}
