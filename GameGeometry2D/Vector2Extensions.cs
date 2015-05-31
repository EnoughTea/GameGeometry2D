using System;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace GameGeometry2D {
    /// <summary> Vector2 extension methods. </summary>
    public static class Vector2Extensions {
        private const float Tolerance = 0.001f;

        /// <summary>Returns formatted  <see cref="Vector2" /> string representation.</summary>
        /// <param name="current">The current <see cref="Vector2" />.</param>
        /// <param name="format">The format to use.</param>
        /// <param name="delimiter">The delimiter between vector value strings.</param>
        public static string Format(this Vector2 current, string format = "0.##",
            string delimiter = ", ") {
            return current.X.ToString(format, CultureInfo.InvariantCulture) + delimiter +
                   current.Y.ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>Return Point representation of the current  <see cref="Vector2" />.</summary>
        /// <param name="current">The current vector.</param>
        /// <returns> <see cref="Vector2" /> converted to a Point.</returns>
        public static Point ToPoint(this Vector2 current) {
            return new Point((int) current.X, (int) current.Y);
        }

        /// <summary>Returns the absoluteX coordinate of the current <see cref="Vector2" />.</summary>
        /// <param name="current">The current <see cref="Vector2" />.</param>
        /// <returns>The absolute X coordinate of the <see cref="Vector2" />.</returns>
        public static float AbsX(this Vector2 current) {
            return Math.Abs(current.X);
        }

        /// <summary>Returns the absolute Y coordinate of the current <see cref="Vector2" />.</summary>
        /// <param name="current">The current <see cref="Vector2" />.</param>
        /// <returns>The absolute Y coordinate of the <see cref="Vector2" />.</returns>
        public static float AbsY(this Vector2 current) {
            return Math.Abs(current.Y);
        }

        /// <summary>Returns current  <see cref="Vector2" /> with absolute values.</summary>
        /// <param name="current">The current  <see cref="Vector2" />.</param>
        /// <returns>Current  <see cref="Vector2" /> with absolute values.</returns>
        public static Vector2 Abs(this Vector2 current) {
            Vector2 result;
            Vectors2.Abs(ref current, out result);
            return result;
        }

        /// <summary>
        ///     Checks whether this <see cref="Vector2" /> nearly (+-epsilon) equals other  <see cref="Vector2" />.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="other">The other.</param>
        /// <returns>True if two vectors are equal with epsilon.</returns>
        public static bool NearlyEquals(this Vector2 current, Vector2 other, float epsilon = Tolerance) {
            return NearlyEquals(current, ref other, epsilon);
        }

        /// <summary>
        ///     Checks whether this <see cref="Vector2" /> nearly (+-epsilon) equals other  <see cref="Vector2" />.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="other">The other.</param>
        /// <returns>True if two vectors are equal with epsilon.</returns>
        [CLSCompliant(false)]
        public static bool NearlyEquals(this Vector2 current, ref Vector2 other, float epsilon = Tolerance) {
            return current.X.NearlyEquals(other.X, epsilon) && current.Y.NearlyEquals(other.Y, epsilon);
        }

        /// <summary>Determines whether <see cref="Vector2" /> nearly (+-epsilon) equals zero.</summary>
        /// <param name="current">The current <see cref="Vector2" />.</param>
        /// <returns><c>true</c> if <see cref="Vector2" /> only has zero values; otherwise, <c>false</c>.</returns>
        public static bool EqualsZero(this Vector2 current, float epsilon = Tolerance) {
            return current.X.EqualsZero(epsilon) && current.Y.EqualsZero(epsilon);
        }

        /// <summary>Returns a signed unit vector containing axis for largest coordinate.</summary>
        public static Vector2 MajorAxis(this Vector2 current) {
            return (Math.Abs(current.X) > Math.Abs(current.Y))
                ? new Vector2(Math.Sign(current.X), 0)
                : new Vector2(0, Math.Sign(current.Y));
        }

        /// <summary>Returns a signed unit vector containing axis for smallest coordinate.</summary>
        public static Vector2 MinorAxis(this Vector2 current) {
            return (Math.Abs(current.X) < Math.Abs(current.Y))
                ? new Vector2(Math.Sign(current.X), 0)
                : new Vector2(0, Math.Sign(current.Y));
        }

        /// <summary>
        ///     Returns a new <see cref="Vector2" /> whose coordinates are the coordinates of this
        ///     <see cref="Vector2" /> with the given values added. This <see cref="Vector2" /> is not modified.
        /// </summary>
        /// <param name="current">The current vector.</param>
        /// <param name="x">Distance to offset the X coordinate.</param>
        /// <param name="y">Distance to offset the Y coordinate.</param>
        /// <returns> A new <see cref="Vector2" /> offset by the given coordinates. </returns>
        public static Vector2 Offset(this Vector2 current, float x, float y) {
            return new Vector2(current.X + x, current.Y + y);
        }

        /// <summary>
        ///     Determines the current angle in radians of the current <see cref="Vector2" /> and returns it.
        /// </summary>
        /// <param name="current">The current <see cref="Vector2" />.</param>
        /// <returns>The angle in radians of the current <see cref="Vector2" />.</returns>
        public static float ToAngle(this Vector2 current) {
            float result;
            Vectors2.Angle(ref current, out result);
            return result;
        }
    }
}