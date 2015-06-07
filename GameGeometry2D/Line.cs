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
    [StructLayout(LayoutKind.Sequential, Size = ByteSize), DataContract(Name = "line", Namespace = ""), 
    KnownType(typeof(Vector2))]
    public struct Line : IEquatable<Line> {
        public const int ByteSize = (sizeof(float) * 2) + sizeof(float);

        /// <summary> Zero line (point at coordinate system origin). </summary>
        public static readonly Line Zero = new Line(Vector2.Zero, 0);

        public Line(Vector2 normal, float d) {
            Normal = normal;
            D = d;
        }

        public Line(float nX, float nY, float d) {
            Normal.X = nX;
            Normal.Y = nY;
            D = d;
        }

        /// <summary>Normal vector for the line.</summary>
        [DataMember(Name = "n", Order = 0)]
        public Vector2 Normal;

        /// <summary>The perpendicular distance from the coordinate system origin to the line.</summary>
        [DataMember(Name = "d", Order = 1)]
        public float D;

        public float GetDistance(Vector2 point) {
            float result;
            GetDistance(ref point, out result);
            return result;
        }

        public void GetDistance(ref Vector2 point, out float result) {
            Vector2.Dot(ref point, ref Normal, out result);
            result -= D;
        }

        public float Intersects(Ray2D ray) {
            float result;
            Intersects(ref ray, out result);
            return result;
        }

        public bool Intersects(BoundingRectangle rect) {
            bool result;
            Intersects(ref rect, out result);
            return result;
        }

        public bool Intersects(BoundingCircle circle) {
            bool result;
            circle.Intersects(ref this, out result);
            return result;
        }

        public bool Intersects(BoundingPolygon polygon) {
            bool result;
            Intersects(ref polygon, out result);
            return result;
        }

        public void Intersects(ref Ray2D ray, out float result) {
            float dir;
            Vector2.Dot(ref Normal, ref ray.Direction, out dir);
            if (-dir > 0) {
                float distanceFromOrigin;
                Vector2.Dot(ref Normal, ref ray.Origin, out distanceFromOrigin);
                distanceFromOrigin += D;

                distanceFromOrigin = -((distanceFromOrigin + D) / dir);
                if (distanceFromOrigin >= 0) {
                    result = distanceFromOrigin;
                    return;
                }
            }
            result = -1;
        }

        public void Intersects(ref BoundingRectangle box, out bool result) {
            var vertices = box.Corners();
            CheckVerticesForIntersection(vertices, out result);
        }

        public void Intersects(ref BoundingCircle circle, out bool result) {
            circle.Intersects(ref this, out result);
        }

        public void Intersects(ref BoundingPolygon polygon, out bool result) {
            Contract.Requires(polygon != null);
            Contract.Requires(polygon.Vertices != null);
            Contract.Requires(polygon.Vertices.Length > 2);

            var vertices = polygon.Vertices;
            CheckVerticesForIntersection(vertices, out result);
        }

        private void CheckVerticesForIntersection(Vector2[] vertices, out bool result) {
            float distance;
            GetDistance(ref vertices[0], out distance);
            var sign = Math.Sign(distance);
            result = false;

            for (var index = 1; index < vertices.Length; ++index) {
                GetDistance(ref vertices[index], out distance);

                if (Math.Sign(distance) != sign) {
                    result = true;
                    break;
                }
            }
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() {
            return "(" + Normal.Format() + "), d = " + D.ToString("0.##", CultureInfo.InvariantCulture);
        }

        public override int GetHashCode() {
            return Normal.GetHashCode() ^ D.GetHashCode();
        }

        public override bool Equals(object obj) {
            return obj is Line && Equals((Line) obj);
        }

        public bool Equals(Line other) {
            return Equals(ref this, ref other);
        }

        public static bool Equals(Line line1, Line line2) {
            return Equals(ref line1, ref line2);
        }

        [CLSCompliant(false)]
        public static bool Equals(ref Line line1, ref Line line2) {
            return Vectors2.Equals(ref line1.Normal, ref line2.Normal) && line1.D.NearlyEquals(line2.D);
        }

        public static Line Transform(Line line, Matrix matrix) {
            Line result;
            Transform(ref line, ref matrix, out result);
            return result;
        }

        public static void Transform(ref Line line, ref Matrix matrix, out Line result) {
            Vector2 point;
            var origin = Vector2.Zero;
            Vector2.Multiply(ref line.Normal, line.D, out point);
            Vector2.Transform(ref point, ref matrix, out point);
            Vector2.Transform(ref origin, ref matrix, out origin);
            Vector2.Subtract(ref point, ref origin, out result.Normal);
            Vector2.Normalize(ref result.Normal, out result.Normal);
            Vector2.Dot(ref point, ref result.Normal, out result.D);
        }


        public static Line Parse(string source) {
            Contract.Requires(!String.IsNullOrWhiteSpace(source));

            Line result;
            if (!TryParse(source, out result)) {
                throw new ArgumentException("Source string does not contain 3 numbers.", "source");
            }

            return result;
        }

        public static bool TryParse(string source, out Line result) {
            bool validParse = NumberTools.ParseVectorAndNumber(source, out result.Normal, out result.D);
            if (validParse) { return true; }

            result = Zero;
            return false;
        }

        public static bool operator ==(Line line1, Line line2) {
            return Equals(ref line1, ref line2);
        }

        public static bool operator !=(Line line1, Line line2) {
            return !Equals(ref line1, ref line2);
        }
    }
}