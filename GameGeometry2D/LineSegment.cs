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
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace GameGeometry2D {
    [StructLayout(LayoutKind.Sequential, Size = Size), DataContract(Name = "lineSeg", Namespace = "")]
    public struct LineSegment : IEquatable<LineSegment> {
        public const int Size = (sizeof(float) * 2) * 2;

        /// <summary> Zero line segment (point at coordinate system origin). </summary>
        public static readonly LineSegment Zero = new LineSegment(Vector2.Zero, Vector2.Zero);

        public LineSegment(Vector2 vertex1, Vector2 vertex2) {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
        }

        [DataMember(Name = "v1", Order = 0)]
        public Vector2 Vertex1;

        [DataMember(Name = "v2", Order = 1)]
        public Vector2 Vertex2;


        public float GetDistance(Vector2 point) {
            float result;
            GetDistance(ref point, out result);
            return result;
        }

        public void GetDistance(ref Vector2 point, out float result) {
            GetDistance(ref Vertex1, ref Vertex2, ref point, out result);
        }

        public float Intersects(Ray2D ray) {
            float result;
            Intersects(ref ray, out result);
            return result;
        }

        public void Intersects(ref Ray2D ray, out float result) {
            Intersects(ref Vertex1, ref Vertex2, ref ray, out result);
        }


        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() {
            return "(" + Vertex1.Format() + ") ↔ (" + Vertex2.Format() + ")";
        }

        public override int GetHashCode() {
            return Vertex1.GetHashCode() ^ Vertex2.GetHashCode();
        }

        public override bool Equals(object obj) {
            return obj is LineSegment && Equals((LineSegment) obj);
        }

        public bool Equals(LineSegment other) {
            return Equals(ref this, ref other);
        }

        public static void Intersects(ref Vector2 v1, ref Vector2 v2, ref Vector2 v3, ref Vector2 v4, out bool result) {
            float div, ua, ub;
            div = 1 / ((v4.Y - v3.Y) * (v2.X - v1.X) - (v4.X - v3.X) * (v2.Y - v1.Y));
            ua = ((v4.X - v3.X) * (v1.Y - v3.Y) - (v4.Y - v3.Y) * (v1.X - v3.X)) * div;
            ub = ((v2.X - v1.X) * (v1.Y - v3.Y) - (v2.Y - v1.Y) * (v1.X - v3.X)) * div;
            result = ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1;
        }

        public static bool Intersects(ref Vector2 v1, ref Vector2 v2, ref Vector2 v3, ref Vector2 v4,
            out Vector2 result) {
            var div = 1 / ((v4.Y - v3.Y) * (v2.X - v1.X) - (v4.X - v3.X) * (v2.Y - v1.Y));
            var ua = ((v4.X - v3.X) * (v1.Y - v3.Y) - (v4.Y - v3.Y) * (v1.X - v3.X)) * div;
            var ub = ((v2.X - v1.X) * (v1.Y - v3.Y) - (v2.Y - v1.Y) * (v1.X - v3.X)) * div;

            if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1) {
                Vector2.Lerp(ref v1, ref v2, ua, out result);
                return true;
            }

            result = Vector2.Zero;
            return false;
        }

        public static void Intersects(ref Vector2 vertex1, ref Vector2 vertex2, ref Ray2D ray, out float result,
            float tolerance = 0.0001f) {
            Vector2 tangent, normal;
            float edgeMagnitude;
            Vector2.Subtract(ref vertex1, ref vertex2, out tangent);
            Vectors2.Normalize(ref tangent, out edgeMagnitude, out tangent);
            Vectors2.RightNormal(ref tangent, out normal);

            float dir;
            Vector2.Dot(ref normal, ref ray.Direction, out dir);
            if (Math.Abs(dir) >= tolerance) {
                Vector2 originDiff;
                Vector2.Subtract(ref ray.Origin, ref vertex2, out originDiff);
                float actualDistance;
                Vector2.Dot(ref normal, ref originDiff, out actualDistance);
                var distanceFromOrigin = -(actualDistance / dir);
                if (distanceFromOrigin >= 0) {
                    Vector2 intersectPos;
                    Vector2.Multiply(ref ray.Direction, distanceFromOrigin, out intersectPos);
                    Vector2.Add(ref intersectPos, ref originDiff, out intersectPos);

                    float distanceFromSecond;
                    Vector2.Dot(ref intersectPos, ref tangent, out distanceFromSecond);

                    if (distanceFromSecond >= 0 && distanceFromSecond <= edgeMagnitude) {
                        result = distanceFromOrigin;
                        return;
                    }
                }
            }
            result = -1;
        }

        public static void GetDistance(ref Vector2 vertex1, ref Vector2 vertex2, ref Vector2 point, out float result) {
            float edgeLength;
            Vector2 edge, local;

            Vector2.Subtract(ref point, ref vertex2, out local);
            Vector2.Subtract(ref vertex1, ref vertex2, out edge);
            Vectors2.Normalize(ref edge, out edgeLength, out edge);

            var nProj = local.Y * edge.X - local.X * edge.Y;
            var tProj = local.X * edge.X + local.Y * edge.Y;
            if (tProj < 0) {
                result = (float) Math.Sqrt(tProj * tProj + nProj * nProj);
            } else if (tProj > edgeLength) {
                tProj -= edgeLength;
                result = (float) Math.Sqrt(tProj * tProj + nProj * nProj);
            } else {
                result = Math.Abs(nProj);
            }
        }

        public static void GetDistanceSq(ref Vector2 vertex1, ref Vector2 vertex2, ref Vector2 point,
            out float result) {
            float edgeLength;
            Vector2 edge, local;

            Vector2.Subtract(ref point, ref vertex2, out local);
            Vector2.Subtract(ref vertex1, ref vertex2, out edge);
            Vectors2.Normalize(ref edge, out edgeLength, out edge);

            var nProj = local.Y * edge.X - local.X * edge.Y;
            var tProj = local.X * edge.X + local.Y * edge.Y;
            if (tProj < 0) {
                result = tProj * tProj + nProj * nProj;
            } else if (tProj > edgeLength) {
                tProj -= edgeLength;
                result = tProj * tProj + nProj * nProj;
            } else {
                result = nProj * nProj;
            }
        }

        public static bool Equals(LineSegment line1, LineSegment line2) {
            return Equals(ref line1, ref line2);
        }

        [CLSCompliant(false)]
        public static bool Equals(ref LineSegment line1, ref LineSegment line2) {
            return Vectors2.Equals(ref line1.Vertex1, ref line2.Vertex1) &&
                   Vectors2.Equals(ref line1.Vertex2, ref line2.Vertex2);
        }

        public static LineSegment Parse(string source) {
            Contract.Requires(!String.IsNullOrWhiteSpace(source));

            LineSegment result;
            if (!TryParse(source, out result)) {
                throw new ArgumentException("Source string does not contain 4 numbers.", "source");
            }

            return result;
        }

        public static bool TryParse(string source, out LineSegment result) {
            bool validParse = NumberTools.ParseTwoVectors(source, out result.Vertex1, out result.Vertex2);
            if (validParse) { return true; }

            result = Zero;
            return false;
        }

        public static bool operator ==(LineSegment line1, LineSegment line2) {
            return Equals(ref line1, ref line2);
        }

        public static bool operator !=(LineSegment line1, LineSegment line2) {
            return !Equals(ref line1, ref line2);
        }
    }
}