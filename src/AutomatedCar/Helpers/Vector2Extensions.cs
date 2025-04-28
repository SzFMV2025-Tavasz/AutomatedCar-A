namespace AutomatedCar.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;

    public static class Vector2Extensions
    {
        /// <summary>
        /// Rotates a Vector2 object.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="angle">The rotation angle in radians.</param>
        /// <returns>A rotated vector.</returns>
        public static Vector2 Rotate(this Vector2 vector, float angle)
        {
            float x = vector.X;
            float y = vector.Y;

            vector.X = (float)((x * Math.Cos(angle)) - (y * Math.Sin(angle)));
            vector.Y = (float)((x * Math.Sin(angle)) + (y * Math.Cos(angle)));

            return vector;
        }
    }
}
