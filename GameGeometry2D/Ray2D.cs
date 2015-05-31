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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace GameGeometry2D {
    [StructLayout(LayoutKind.Sequential, Size = Size), DataContract(Name = "ray", Namespace = "")]
    public struct Ray2D : IEquatable<Ray2D> {
        public const int Size = (sizeof(float) * 2) * 2;

        /// <summary> Zero ray (point at coordinate system origin). </summary>
        public static readonly Ray2D Zero = new Ray2D(Vector2.Zero, Vector2.Zero);

        /// <summary> Initializes a new instance of the <see cref="Ray2D"/> struct. </summary>
        /// <param name="origin">Ray origin.</param>
        /// <param name="direction">Ray direction.</param>
        public Ray2D(Vector2 origin, Vector2 direction) {
            Origin = origin;
            Direction = direction;
        }

        [DataMember(Name = "o", EmitDefaultValue = false, Order = 0)]
        public Vector2 Origin;

        [DataMember(Name = "d", EmitDefaultValue = false, Order = 1)]
        public Vector2 Direction;

        public float Intersects(BoundingRectangle rect) {
            float result;
            rect.Intersects(ref this, out result);
            return result;
        }

        public float Intersects(Line line) {
            float result;
            line.Intersects(ref this, out result);
            return result;
        }

        public float Intersects(LineSegment line) {
            float result;
            line.Intersects(ref this, out result);
            return result;
        }

        public float Intersects(BoundingCircle circle) {
            float result;
            circle.Intersects(ref this, out result);
            return result;
        }

        public float Intersects(BoundingPolygon polygon) {
            Contract.Requires(polygon != null);
            Contract.Requires(polygon.Vertices != null);

            float result;
            polygon.Intersects(ref this, out result);
            return result;
        }

        public void Intersects(ref BoundingRectangle rect, out float result) {
            rect.Intersects(ref this, out result);
        }

        public void Intersects(ref Line line, out float result) {
            line.Intersects(ref this, out result);
        }

        public void Intersects(ref LineSegment line, out float result) {
            line.Intersects(ref this, out result);
        }

        public void Intersects(ref BoundingCircle circle, out float result) {
            circle.Intersects(ref this, out result);
        }

        public void Intersects(ref BoundingPolygon polygon, out float result) {
            Contract.Requires(polygon != null);
            Contract.Requires(polygon.Vertices != null);

            polygon.Intersects(ref this, out result);
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() {
            return "(" + Origin.Format() + ") ↦ (" + Direction.Format() + ")";
        }

        public override int GetHashCode() {
            return Origin.GetHashCode() ^ Direction.GetHashCode();
        }

        public override bool Equals(object obj) {
            return obj is Ray2D && Equals((Ray2D) obj);
        }

        public bool Equals(Ray2D other) {
            return Equals(ref this, ref other);
        }

        public static bool Equals(Ray2D ray1, Ray2D ray2) {
            return Equals(ref ray1, ref ray2);
        }

        [CLSCompliant(false)]
        public static bool Equals(ref Ray2D ray1, ref Ray2D ray2) {
            return Vectors2.Equals(ref ray1.Origin, ref ray2.Origin) &&
                   Vectors2.Equals(ref ray1.Direction, ref ray2.Direction);
        }

        public static Ray2D Parse(string source) {
            Contract.Requires(!String.IsNullOrWhiteSpace(source));

            Ray2D result;
            if (!TryParse(source, out result)) {
                throw new ArgumentException("Source string does not contain 4 numbers.", "source");
            }

            return result;
        }

        public static bool TryParse(string source, out Ray2D result) {
            bool validParse = NumberTools.ParseTwoVectors(source, out result.Origin, out result.Direction);
            if (validParse) { return true; }

            result = Zero;
            return false;
        }

        public static bool operator ==(Ray2D ray1, Ray2D ray2) {
            return Equals(ref ray1, ref ray2);
        }

        public static bool operator !=(Ray2D ray1, Ray2D ray2) {
            return !Equals(ref ray1, ref ray2);
        }
    }
}