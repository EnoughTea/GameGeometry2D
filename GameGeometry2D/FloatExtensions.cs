using System;
using System.Diagnostics.Contracts;

namespace GameGeometry2D {
    /// <summary> Extension methods for <see cref="float" />. </summary>
    internal static class FloatExtensions {
        internal const float Tolerance = 0.0001f;

        /// <summary> Test to see if a value equals zero using epsilon. </summary>
        /// <param name="self">The value to test.</param>
        /// <param name="epsilon">The epsilon.</param>
        /// <returns>True if value nearly equals zero, false otherwise.</returns>
        [Pure]
        public static bool EqualsZero(this float self, float epsilon = Tolerance) {
            Contract.Requires(epsilon > 0);

            return NearlyEquals(self, 0f, epsilon);
        }

        /// <summary> Checks if two values are considered equal using given absolute epsilon. </summary>
        /// <remarks>
        ///     Good article about floating point comparisons:
        ///     https://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/
        /// </remarks>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <param name="epsilon">The epsilon used for absolute tolerance.</param>
        /// <returns>
        ///     True if values are considered equal to each other, false otherwise.
        /// </returns>
        [Pure]
        public static bool NearlyEquals(this float a, float b, float epsilon = Tolerance) {
            Contract.Requires(epsilon > 0);

            var diff = Math.Abs(a - b);
            return diff <= epsilon;
        }
    }
}