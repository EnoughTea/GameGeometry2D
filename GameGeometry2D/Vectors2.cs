#region MIT License

/*
 * Copyright (c) 2005-2008 Jonathan Mark Porter. http://physics2d.googlepages.com/
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights to 
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of 
 * the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be 
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

#endregion

using System;
using Microsoft.Xna.Framework;

namespace GameGeometry2D {
    /// <summary>Various useful methods for working with <see cref="Vector2" />s.</summary>
    public static class Vectors2 {
        /// <summary>Returns current <see cref="Vector2" /> with absolute values.</summary>
        /// <param name="current">Current  <see cref="Vector2" />.</param>
        /// <param name="absed">Current <see cref="Vector2" /> with absolute values.</param>
        public static void Abs(ref Vector2 current, out Vector2 absed) {
            absed.X = Math.Abs(current.X);
            absed.Y = Math.Abs(current.Y);
        }

        /// <summary>Determines the current angle [0, 2 Pi] of the given vector.</summary>
        /// <param name="source">The given <see cref="Vector2" />.</param>
        /// <returns>The angle in radians of the given <see cref="Vector2" />.</returns>
        public static float Angle(Vector2 source) {
            float result;
            Angle(ref source, out result);
            return result;
        }

        /// <summary>Determines the current angle [0, 2 Pi] of the given vector.</summary>
        /// <param name="source">The given <see cref="Vector2" />.</param>
        /// <param name="result">The angle in radians of the given <see cref="Vector2" />.</param>
        public static void Angle(ref Vector2 source, out float result) {
            result = (float) Math.Atan2(source.Y, source.X);
            if (result < 0) { result += MathHelper.TwoPi; }
        }

        /// <summary>Right-hand cross product of two vectors.</summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <param name="perpendicular">Perpendicular directional vector to the two vectors.</param>
        public static void CrossRight(ref Vector2 a, ref Vector2 b, out Vector2 perpendicular) {
            perpendicular.X = -b.Y + a.X;
            perpendicular.Y = b.X + a.Y;
        }

        /// <summary>Left-hand cross product of two vectors.</summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <param name="perpendicular">Perpendicular directional vector to the two vectors.</param>
        public static void CrossLeft(ref Vector2 a, ref Vector2 b, out Vector2 perpendicular) {
            perpendicular.X = b.Y + a.X;
            perpendicular.Y = -b.X + a.Y;
        }

        /// <summary>Clamps the given vector, so its coordinates will be within the specified bounds (inclusive).
        /// </summary>
        /// <param name="current">Vector to clamp.</param>
        /// <param name="min">The minimum bound.</param>
        /// <param name="max">The maximum bound.</param>
        /// <param name="clamped">Clamped vector.</param>
        public static void Clamp(ref Vector2 current, ref Vector2 min, ref Vector2 max, out Vector2 clamped) {
            NumberTools.Clamp(ref current.X, ref min.X, ref max.X, out clamped.X);
            NumberTools.Clamp(ref current.Y, ref min.Y, ref max.Y, out clamped.Y);
        }

        /// <summary>Clamps the given vector, so its coordinates will be within the specified bounds (inclusive), 
        /// but also wraps them around. So that X plus <paramref name="max"/> would result in X plus 
        /// <paramref name="min"/>.</summary>
        /// <param name="current">Vector to clamp.</param>
        /// <param name="min">The minimum bound.</param>
        /// <param name="max">The maximum bound.</param>
        /// <param name="clamped">Clamped vector.</param>
        public static void ClampWrap(ref Vector2 current, ref Vector2 min, ref Vector2 max, out Vector2 clamped) {
            NumberTools.ClampWrap(ref current.X, ref min.X, ref max.X, out clamped.X);
            NumberTools.ClampWrap(ref current.Y, ref min.Y, ref max.Y, out clamped.Y);
        }

        /// <summary>Calculates euclidean distance between two specified vectors.</summary>
        /// <param name="left">First vector.</param>
        /// <param name="right">Second vector.</param>
        /// <returns>Euclidean distance between two specified vectors.</returns>
        public static float Distance(Vector2 left, Vector2 right) {
            float result;
            Distance(ref left, ref right, out result);
            return result;
        }

        /// <summary>Calculates euclidean distance between two specified vectors.</summary>
        /// <param name="left">First vector.</param>
        /// <param name="right">Second vector.</param>
        /// <param name="result">Euclidean distance between two specified vectors.</param>
        public static void Distance(ref Vector2 left, ref Vector2 right, out float result) {
            float x, y;
            x = left.X - right.X;
            y = left.Y - right.Y;
            result = (float) Math.Sqrt(x * x + y * y);
        }

        /// <summary>Calculates squared euclidean distances between two specified vectors.</summary>
        /// <param name="left">First vector.</param>
        /// <param name="right">Second vector.</param>
        /// <returns>Squared euclidean distance between two specified vectors.</returns>
        public static float DistanceSq(Vector2 left, Vector2 right) {
            float result;
            DistanceSq(ref left, ref right, out result);
            return result;
        }

        /// <summary>Calculates squared euclidean distance between two specified vectors.</summary>
        /// <param name="left">First vector.</param>
        /// <param name="right">Second vector.</param>
        /// <param name="result">Squared euclidean distance between two specified vectors.</param>
        public static void DistanceSq(ref Vector2 left, ref Vector2 right, out float result) {
            float x, y;
            x = left.X - right.X;
            y = left.Y - right.Y;
            result = x * x + y * y;
        }

        /// <summary>Checks the specified vectors for equality using epsilon.</summary>
        /// <param name="left">First vector.</param>
        /// <param name="right">Second vector.</param>
        /// <param name="epsilon">Epsilon used for absolute tolerance.</param>
        /// <returns>
        ///     true if vectors are equal; false otherwise.
        /// </returns>
        public static bool Equals(Vector2 left, Vector2 right, float epsilon = FloatExtensions.Tolerance) {
            return Equals(ref left, ref right, epsilon);
        }

        /// <summary> Checks the specified vectors for equality using epsilon. </summary>
        /// <param name="left">First vector.</param>
        /// <param name="right">Second vector.</param>
        /// <param name="epsilon">Epsilon used for absolute tolerance.</param>
        /// <returns>true if vectors are equal; false otherwise.</returns>
        [CLSCompliant(false)]
        public static bool Equals(ref Vector2 left, ref Vector2 right, float epsilon = FloatExtensions.Tolerance) {
            return left.X.NearlyEquals(right.X) && left.Y.NearlyEquals(right.Y);
        }

        /// <summary>
        ///     Creates a Vector2 With the given length (<see cref="Vector2.Length" />) and the given angle.
        /// </summary>
        /// <param name="length">The length (<see cref="Vector2.Length" />) of the Vector2 to be created</param>
        /// <param name="radianAngle">The angle of the from the (<see cref="Vector2.UnitX" />) in Radians</param>
        /// <returns>a Vector2 With the given length and angle.</returns>
        /// <remarks>
        ///     <code>FromLengthAndAngle(1,Math.PI/2)</code>
        ///     would create a Vector2 equil to
        ///     <code>new Vector2(0,1)</code>.
        ///     And <code>FromLengthAndAngle(1,0)</code>
        ///     would create a Vector2 equil to
        ///     <code>new Vector2(1,0)</code>.
        /// </remarks>
        public static Vector2 FromLengthAndAngle(float length, float radianAngle) {
            Vector2 result;
            FromLengthAndAngle(length, radianAngle, out result);
            return result;
        }

        public static void FromLengthAndAngle(float length, float radianAngle, out Vector2 result) {
            result.X = length * (float) Math.Cos(radianAngle);
            result.Y = length * (float) Math.Sin(radianAngle);
        }

        /// <summary>Gets the direction angle between two <see cref="Vector2" />s.</summary>
        /// <param name="from">The first <see cref="Vector2" />.</param>
        /// <param name="to">The second <see cref="Vector2" />.</param>
        /// <returns>Feta between two <see cref="Vector2" />s.</returns>
        public static float SignedAngle(Vector2 from, Vector2 to) {
            return SignedAngle(ref from, ref to);
        }

        /// <summary>Gets the direction angle between two <see cref="Vector2" />s.</summary>
        /// <param name="from">The first <see cref="Vector2" />.</param>
        /// <param name="to">The second <see cref="Vector2" />.</param>
        /// <returns>Feta between two <see cref="Vector2" />s.</returns>
        [CLSCompliant(false)]
        public static float SignedAngle(ref Vector2 from, ref Vector2 to) {
            var sin = from.X * to.Y - to.X * from.Y;
            var cos = from.X * to.X + from.Y * to.Y;
            return (float) (Math.Atan2(sin, cos));
        }

        /// <summary>
        ///     Gets a Vector2 that is perpendicular(orthogonal) to the passed Vector2 while staying on the XY Plane.
        /// </summary>
        /// <param name="source">The Vector2 whose perpendicular(orthogonal) is to be determined.</param>
        /// <returns>An perpendicular(orthogonal) Vector2 using the Left Hand Rule (opposite of the Right hand Rule)</returns>
        /// <remarks>
        ///     <seealso href="http://en.wikipedia.org/wiki/Right-hand_rule#Left-hand_rule" />
        /// </remarks>
        public static Vector2 LeftNormal(Vector2 source) {
            Vector2 result;
            LeftNormal(ref source, out result);
            return result;
        }

        public static void LeftNormal(ref Vector2 source, out Vector2 result) {
            var sourceX = source.X;
            result.X = source.Y;
            result.Y = -sourceX;
        }

        /// <summary>
        ///     Gets a Vector2 that is perpendicular(orthogonal) to the passed Vector2 while staying on the XY Plane.
        /// </summary>
        /// <param name="source">The Vector2 whose perpendicular(orthogonal) is to be determined.</param>
        /// <returns>An perpendicular(orthogonal) Vector2 using the Right Hand Rule</returns>
        /// <remarks>
        ///     <seealso href="http://en.wikipedia.org/wiki/Right-hand_rule" />
        /// </remarks>
        public static Vector2 RightNormal(Vector2 source) {
            Vector2 result;
            RightNormal(ref source, out result);
            return result;
        }

        public static void RightNormal(ref Vector2 source, out Vector2 result) {
            var sourceX = source.X;
            result.X = -source.Y;
            result.Y = sourceX;
        }

        /// <summary>
        ///     Finds a right-handed perpendicular vector that is <paramref name="outwardsDistance" /> units from the
        ///     <paramref name="lineDistancePercentage" /> of a line segment (a to b).
        /// </summary>
        /// <param name="a">The first point of the line segment.</param>
        /// <param name="b">The second point of the line segment.</param>
        /// <param name="lineDistancePercentage">
        ///     A percentage of distance across the line, the midpoint would be 0.5f.
        /// </param>
        /// <param name="outwardsDistance">Outward distance from the point we found across the line.</param>
        /// <param name="result">The resulting perpendicular vector.</param>
        public static void RightNormalPosition(ref Vector2 a, ref Vector2 b, float lineDistancePercentage,
            float outwardsDistance, out Vector2 result) {
            Vector2 scaledDiff;
            Vector2 targetPoint;
            CalculateCrossPoints(ref a, ref b, lineDistancePercentage, outwardsDistance, out scaledDiff,
                out targetPoint);
            CrossRight(ref scaledDiff, ref targetPoint, out result);
        }

        /// <summary>
        ///     Finds a left-handed perpendicular vector that is <paramref name="outwardsDistance" /> units from the
        ///     <paramref name="lineDistancePercentage" /> of a line segment (a to b).
        /// </summary>
        /// <param name="a">The first point of the line segment.</param>
        /// <param name="b">The second point of the line segment.</param>
        /// <param name="lineDistancePercentage">
        ///     A percentage of distance across the line, the midpoint would be 0.5f.
        /// </param>
        /// <param name="outwardsDistance">Outward distance from the point we found across the line.</param>
        /// <param name="result">The resulting perpendicular vector.</param>
        public static void LeftNormalPosition(ref Vector2 a, ref Vector2 b, float lineDistancePercentage,
            float outwardsDistance, out Vector2 result) {
            Vector2 scaledDiff;
            Vector2 targetPoint;
            CalculateCrossPoints(ref a, ref b, lineDistancePercentage, outwardsDistance, out scaledDiff,
                out targetPoint);
            CrossLeft(ref scaledDiff, ref targetPoint, out result);
        }

        /// <summary>
        ///     This returns the Normalized Vector2 that is passed. This is also known as a Unit Vector.
        /// </summary>
        /// <param name="source">The Vector2 to be Normalized.</param>
        /// <returns>The Normalized Vector2. (Unit Vector)</returns>
        /// <remarks>
        ///     <seealso href="http://en.wikipedia.org/wiki/Vector_%28spatial%29#Unit_vector" />
        /// </remarks>
        public static Vector2 Normalize(Vector2 source) {
            Vector2 result;
            Normalize(ref source, out result);
            return result;
        }

        public static void Normalize(ref Vector2 source, out Vector2 result) {
            var magnitude = source.Length();
            if (magnitude > 0) {
                magnitude = (1 / magnitude);
                result.X = source.X * magnitude;
                result.Y = source.Y * magnitude;
            } else {
                result = Vector2.Zero;
            }
        }

        [CLSCompliant(false)]
        public static void Normalize(ref Vector2 source) {
            Normalize(ref source, out source);
        }

        /// <summary>
        ///     This returns the Normalized Vector2 that is passed. This is also known as a Unit Vector.
        /// </summary>
        /// <param name="source">The Vector2 to be Normalized.</param>
        /// <param name="magnitude">the magitude of the Vector2 passed</param>
        /// <returns>The Normalized Vector2. (Unit Vector)</returns>
        /// <remarks>
        ///     <seealso href="http://en.wikipedia.org/wiki/Vector_%28spatial%29#Unit_vector" />
        /// </remarks>
        public static Vector2 Normalize(Vector2 source, out float magnitude) {
            Vector2 result;
            Normalize(ref source, out magnitude, out result);
            return result;
        }

        public static void Normalize(ref Vector2 source, out float magnitude, out Vector2 result) {
            magnitude = source.Length();
            if (magnitude > 0) {
                var magnitudeInv = (1 / magnitude);
                result.X = source.X * magnitudeInv;
                result.Y = source.Y * magnitudeInv;
            } else {
                result = Vector2.Zero;
            }
        }

        /// <summary>
        ///     Rotates a Vector2.
        /// </summary>
        /// <param name="source">The Vector2 to be Rotated.</param>
        /// <param name="radianAngle">The angle in radians of the amount it is to be rotated.</param>
        /// <returns>The Rotated Vector2</returns>
        public static Vector2 Rotate(Vector2 source, float radianAngle) {
            Vector2 result;
            Rotate(ref source, radianAngle, out result);
            return result;
        }

        public static void Rotate(ref Vector2 source, float radianAngle, out Vector2 result) {
            var negradianAngle = -radianAngle;
            var cos = (float) Math.Cos(negradianAngle);
            var sin = (float) Math.Sin(negradianAngle);
            result.X = source.X * cos + source.Y * sin;
            result.Y = source.Y * cos - source.X * sin;
        }

        /// <summary>Rotates the <see cref="Vector2" /> around the specified point.</summary>
        /// <param name="target">The target <see cref="Vector2" />.</param>
        /// <param name="radians">The angle in radians.</param>
        /// <param name="origin">The pivot point for rotation.</param>
        /// <returns>Rotated <see cref="Vector2" />.</returns>
        public static Vector2 RotateAroundPoint(Vector2 target, float radians, Vector2 origin) {
            Vector2 result;
            RotateAroundPoint(ref target, radians, ref origin, out result);
            return result;
        }

        /// <summary>Rotates the <see cref="Vector2" /> around the specified point.</summary>
        /// <param name="target">The target <see cref="Vector2" />.</param>
        /// <param name="radians">The angle in radians.</param>
        /// <param name="origin">The pivot point for rotation.</param>
        /// <param name="result">Rotated <see cref="Vector2" />.</param>
        public static void RotateAroundPoint(ref Vector2 target, float radians, ref Vector2 origin,
            out Vector2 result) {
            Vector2 translatedPoint;
            Vector2.Subtract(ref target, ref origin, out translatedPoint);
            if (translatedPoint.EqualsZero()) {
                result = target;
            } else {
                var cosRadians = (float) Math.Cos(radians);
                var sinRadians = (float) Math.Sin(radians);
                result.X = (translatedPoint.X * cosRadians) - (translatedPoint.Y * sinRadians) + origin.X;
                result.Y = (translatedPoint.X * sinRadians) + (translatedPoint.Y * cosRadians) + origin.Y;
            }
        }

        /// <summary>
        ///     Sets the angle of a Vector2 without changing the <see cref="Vector2.Length" />.
        /// </summary>
        /// <param name="source">The Vector2 to have its Angle set.</param>
        /// <param name="radianAngle">The angle of the from the (<see cref="Vector2.UnitX" />) in Radians</param>
        /// <returns>A Vector2 with a new Angle.</returns>
        public static Vector2 SetAngle(Vector2 source, float radianAngle) {
            Vector2 result;
            SetAngle(ref source, radianAngle, out result);
            return result;
        }

        public static void SetAngle(ref Vector2 source, float radianAngle, out Vector2 result) {
            var magnitude = source.Length();
            result.X = magnitude * (float) Math.Cos(radianAngle);
            result.Y = magnitude * (float) Math.Sin(radianAngle);
        }

        public static bool PointInTri2D(Vector2 point, Vector2 a, Vector2 b, Vector2 c) {
            Vector2 vect1, vect2;
            float temp;
            Vector2.Subtract(ref b, ref a, out vect1);
            Vector2.Subtract(ref point, ref b, out vect2);
            ZCross(ref vect1, ref vect2, out temp);
            var bClockwise = temp >= 0;
            Vector2.Subtract(ref c, ref b, out vect1);
            Vector2.Subtract(ref point, ref c, out vect2);
            ZCross(ref vect1, ref vect2, out temp);
            if (temp < 0 ^ bClockwise) { return true; }
            Vector2.Subtract(ref a, ref c, out vect1);
            Vector2.Subtract(ref point, ref a, out vect2);
            ZCross(ref vect1, ref vect2, out temp);
            return temp < 0 ^ bClockwise;

            /* bool bClockwise = (((b - a) ^ (point - b)) >= 0);
             return !(((((c - b) ^ (point - c)) >= 0) ^ bClockwise) && ((((a - c) ^ (point - a)) >= 0) ^ bClockwise));*/
        }

        /// <summary>
        ///     Thie Projects the left Vector2 onto the Right Vector2.
        /// </summary>
        /// <param name="left">The left Vector2 operand.</param>
        /// <param name="right">The right Vector2 operand.</param>
        /// <returns>The Projected Vector2.</returns>
        /// <remarks>
        ///     <seealso href="http://en.wikipedia.org/wiki/Projection_%28linear_algebra%29" />
        /// </remarks>
        public static Vector2 Project(Vector2 left, Vector2 right) {
            Vector2 result;
            Project(ref left, ref right, out result);
            return result;
        }

        public static void Project(ref Vector2 left, ref Vector2 right, out Vector2 result) {
            float tmp;
            Vector2.Dot(ref left, ref right, out tmp);
            var magsq = right.LengthSquared();
            tmp /= magsq;
            Vector2.Multiply(ref right, tmp, out result);
        }

        /// <summary>
        ///     Does a "2D" Cross Product also know as an Outer Product.
        /// </summary>
        /// <param name="left">The left Vector2 operand.</param>
        /// <param name="right">The right Vector2 operand.</param>
        /// <returns>The Z value of the resulting vector.</returns>
        /// <remarks>
        ///     This 2D Cross Product is using a cheat. Since the Cross product (in 3D space)
        ///     always generates a vector perpendicular (orthogonal) to the 2 vectors used as
        ///     arguments. The cheat is that the only vector that can be perpendicular to two
        ///     vectors in the XY Plane will parallel to the Z Axis. Since any vector that is
        ///     parallel to the Z Axis will have zeros in both the X and Y Fields I can represent
        ///     the cross product of 2 vectors in the XY plane as single float: Z. Also the
        ///     Cross Product of and Vector on the XY plan and that of one ont on the Z Axis
        ///     will result in a vector on the XY Plane. So the ZCross Methods were well thought
        ///     out and can be trusted.
        ///     <seealso href="http://en.wikipedia.org/wiki/Cross_product" />
        /// </remarks>
        public static float ZCross(Vector2 left, Vector2 right) {
            float result;
            ZCross(ref left, ref right, out result);
            return result;
        }

        public static void ZCross(ref Vector2 left, ref Vector2 right, out float result) {
            result = left.X * right.Y - left.Y * right.X;
        }

        /// <summary> Does a "2D" Cross Product also know as an Outer Product. </summary>
        /// <param name="leftZ">The Z value of the left vector operand.</param>
        /// <param name="right">The right Vector2 operand.</param>
        /// <returns>The Vector2 that fully represents the resulting vector.</returns>
        /// <remarks>
        ///     This 2D Cross Product is using a cheat. Since the Cross product (in 3D space)
        ///     always generates a vector perpendicular (orthogonal) to the 2 vectors used as
        ///     arguments. The cheat is that the only vector that can be perpendicular to two
        ///     vectors in the XY Plane will parallel to the Z Axis. Since any vector that is
        ///     parallel to the Z Axis will have zeros in both the X and Y Fields I can represent
        ///     the cross product of 2 vectors in the XY plane as single float: Z. Also the
        ///     Cross Product of and Vector on the XY plan and that of one ont on the Z Axis
        ///     will result in a vector on the XY Plane. So the ZCross Methods were well thought
        ///     out and can be trusted.
        ///     <seealso href="http://en.wikipedia.org/wiki/Cross_product" />
        /// </remarks>
        public static Vector2 ZCross(float leftZ, Vector2 right) {
            Vector2 result;
            ZCross(leftZ, ref right, out result);
            return result;
        }

        public static void ZCross(float leftZ, ref Vector2 right, out Vector2 result) {
            var rightX = right.X;
            result.X = -leftZ * right.Y;
            result.Y = leftZ * rightX;
        }

        /// <summary> Does a "2D" Cross Product also know as an Outer Product. </summary>
        /// <param name="left">The left Vector2 operand.</param>
        /// <param name="rightZ">The Z value of the right vector operand.</param>
        /// <returns>The Vector2 that fully represents the resulting vector.</returns>
        /// <remarks>
        ///     This 2D Cross Product is using a cheat. Since the Cross product (in 3D space)
        ///     always generates a vector perpendicular (orthogonal) to the 2 vectors used as
        ///     arguments. The cheat is that the only vector that can be perpendicular to two
        ///     vectors in the XY Plane will parallel to the Z Axis. Since any vector that is
        ///     parallel to the Z Axis will have zeros in both the X and Y Fields I can represent
        ///     the cross product of 2 vectors in the XY plane as single float: Z. Also the
        ///     Cross Product of and Vector on the XY plan and that of one ont on the Z Axis
        ///     will result in a vector on the XY Plane. So the ZCross Methods were well thought
        ///     out and can be trusted.
        ///     <seealso href="http://en.wikipedia.org/wiki/Cross_product" />
        /// </remarks>
        public static Vector2 ZCross(Vector2 left, float rightZ) {
            Vector2 result;
            ZCross(ref left, rightZ, out result);
            return result;
        }

        public static void ZCross(ref Vector2 left, float rightZ, out Vector2 result) {
            var leftX = left.X;
            result.X = left.Y * rightZ;
            result.Y = -leftX * rightZ;
        }

        /// <summary>Calculates the cross points for a line segment.</summary>
        /// <param name="a">The first point of the line segment.</param>
        /// <param name="b">The second point of the line segment.</param>
        /// <param name="lineDistancePercentage">
        ///     A percentage of distance across the line, the midpoint would be 0.5f.
        /// </param>
        /// <param name="outwardsDistance">Outward distance from the point we found across the line.</param>
        /// <param name="cp0">First point.</param>
        /// <param name="cp1">Second point.</param>
        internal static void CalculateCrossPoints(ref Vector2 a, ref Vector2 b, float lineDistancePercentage,
            float outwardsDistance, out Vector2 cp0, out Vector2 cp1) {
            Vector2 diff;
            Vector2.Subtract(ref b, ref a, out diff);
            Vector2 scaledDiff;
            Vector2.Multiply(ref diff, lineDistancePercentage, out scaledDiff);

            Vector2.Normalize(ref diff, out diff);
            Vector2 linePoint;
            Vector2.Multiply(ref diff, outwardsDistance, out linePoint);

            Vector2 targetPoint;
            Vector2.Add(ref scaledDiff, ref a, out targetPoint);
            Vector2.Add(ref linePoint, ref targetPoint, out targetPoint);
            cp0 = scaledDiff;
            cp1 = targetPoint;
        }
    }
}