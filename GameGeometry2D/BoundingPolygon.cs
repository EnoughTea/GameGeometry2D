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
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace GameGeometry2D {
    [DataContract(Name = "bPoly", Namespace = ""), KnownType(typeof(Vector2))]
    public sealed class BoundingPolygon : IEquatable<BoundingPolygon> {
        public BoundingPolygon(Vector2[] vertices) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            _vertices = vertices;
        }

        [DataMember(Name = "vs", EmitDefaultValue = false)]
        private Vector2[] _vertices;

        public Vector2[] Vertices {
            get { return _vertices; }

            set {
                Contract.Requires(value != null);
                Contract.Requires(value.Length > 2);

                _vertices = value;
            }
        }

        public float Area {
            get {
                float result;
                GetArea(Vertices, out result);
                return result;
            }
        }

        public float Perimeter {
            get {
                float result;
                GetPerimeter(Vertices, out result);
                return result;
            }
        }

        public bool Equals(BoundingPolygon other) {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }
            if (Vertices == null || other.Vertices == null || Vertices.Length != other.Vertices.Length) {
                return false;
            }

            for (var index = 0; index < Vertices.Length; index++) {
                var vertex1 = Vertices[index];
                var vertex2 = other.Vertices[index];
                if (!Vectors2.Equals(ref vertex1, ref vertex2)) {
                    return false;
                }
            }

            return true;
        }

        public float GetDistance(Vector2 point) {
            float result;
            GetDistance(Vertices, ref point, out result);
            return result;
        }

        public void GetDistance(ref Vector2 point, out float result) {
            GetDistance(Vertices, ref point, out result);
        }

        public Containment Contains(Vector2 point) {
            Containment result;
            Contains(ref point, out result);
            return result;
        }

        public void Contains(ref Vector2 point, out Containment result) {
            ContainsInclusive(Vertices, ref point, out result);
        }

        public Containment Contains(BoundingCircle circle) {
            Containment result;
            Contains(ref circle, out result);
            return result;
        }

        public void Contains(ref BoundingCircle circle, out Containment result) {
            float distance;
            GetDistance(ref circle.Center, out distance);
            distance += circle.Radius;
            if (distance <= 0) {
                result = Containment.Contains;
            } else if (distance <= circle.Radius) {
                result = Containment.Intersects;
            } else {
                result = Containment.Disjoint;
            }
        }

        public Containment Contains(BoundingRectangle rect) {
            Containment result;
            Contains(ref rect, out result);
            return result;
        }

        public void Contains(ref BoundingRectangle rect, out Containment result) {
            Contains(rect.Corners(), out result);
        }

        public Containment Contains(BoundingPolygon polygon) {
            Contract.Requires(polygon != null);
            Contract.Requires(polygon.Vertices != null);
            Contract.Requires(polygon.Vertices.Length > 2);

            Containment result;
            Contains(ref polygon, out result);
            return result;
        }

        public void Contains(ref BoundingPolygon polygon, out Containment result) {
            if (polygon == null) { throw new ArgumentNullException("polygon"); }
            Contains(polygon.Vertices, out result);
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
            Intersects(ref circle, out result);
            return result;
        }

        public bool Intersects(BoundingPolygon polygon) {
            Contract.Requires(polygon != null);
            Contract.Requires(polygon.Vertices != null);
            Contract.Requires(polygon.Vertices.Length > 1);

            bool result;
            Intersects(ref polygon, out result);
            return result;
        }

        public void Intersects(ref Ray2D ray, out float result) {
            result = -1;
            for (var index = 0; index < Vertices.Length; ++index) {
                var index2 = (index + 1) % Vertices.Length;
                float temp;
                LineSegment.Intersects(ref Vertices[index], ref Vertices[index2], ref ray, out temp);
                if (temp >= 0 && (result == -1 || temp < result)) {
                    result = temp;
                }
            }
        }

        public void Intersects(ref BoundingRectangle rect, out bool result) {
            Intersects(Vertices, rect.Corners(), out result);
        }

        public void Intersects(ref BoundingCircle circle, out bool result) {
            result = false;
            for (var index = 0; index < Vertices.Length; ++index) {
                var index2 = (index + 1) % Vertices.Length;
                float temp;
                LineSegment.GetDistance(ref Vertices[index], ref Vertices[index2], ref circle.Center, out temp);
                if (temp <= circle.Radius) {
                    result = true;
                    break;
                }
            }
        }

        public void Intersects(ref BoundingPolygon polygon, out bool result) {
            Contract.Requires(polygon != null);
            Contract.Requires(polygon.Vertices != null);
            Contract.Requires(polygon.Vertices.Length > 1);

            Intersects(Vertices, polygon.Vertices, out result);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return obj is BoundingPolygon && Equals((BoundingPolygon) obj);
        }

        public override int GetHashCode() {
            return (Vertices != null ? Vertices.GetHashCode() : 0);
        }

        private void Contains(Vector2[] otherVertexes, out Containment result) {
            Contract.Requires(otherVertexes != null);
            Contract.Requires(otherVertexes.Length > 2);

            Containment contains;
            result = Containment.Unknown;
            for (var index = 0; index < Vertices.Length; ++index) {
                ContainsExclusive(otherVertexes, ref Vertices[index], out contains);
                if (contains == Containment.Contains) {
                    result = Containment.Intersects;
                    return;
                }
            }
            for (var index = 0; index < otherVertexes.Length && result != Containment.Intersects; ++index) {
                ContainsInclusive(Vertices, ref otherVertexes[index], out contains);
                result |= contains;
            }
            if (result == Containment.Disjoint) {
                bool test;
                Intersects(Vertices, otherVertexes, out test);
                if (test) { result = Containment.Intersects; }
            }
        }

        public static Containment ContainsExclusive(Vector2[] vertices, Vector2 point) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            Containment result;
            ContainsExclusive(vertices, ref point, out result);
            return result;
        }

        public static void ContainsExclusive(Vector2[] vertices, ref Vector2 point, out Containment result) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            var intersectionCount = 0;
            Vector2 v1, v2;
            v1 = vertices[vertices.Length - 1];
            for (var index = 0; index < vertices.Length; ++index, v1 = v2) {
                v2 = vertices[index];
                var t1 = (v1.Y <= point.Y);
                if (t1 ^ (v2.Y <= point.Y)) {
                    var temp = ((point.Y - v1.Y) * (v2.X - v1.X) - (point.X - v1.X) * (v2.Y - v1.Y));
                    if (t1) {
                        if (temp > 0) { intersectionCount++; }
                    } else {
                        if (temp < 0) { intersectionCount--; }
                    }
                }
            }
            result = (intersectionCount != 0) ? (Containment.Contains) : (Containment.Disjoint);
        }

        public static Containment ContainsInclusive(Vector2[] vertices, Vector2 point) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            Containment result;
            ContainsInclusive(vertices, ref point, out result);
            return result;
        }

        public static void ContainsInclusive(Vector2[] vertices, ref Vector2 point, out Containment result) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            var count = 0; // the crossing count
            var v1 = vertices[vertices.Length - 1];
            Vector2 v2;
            for (var index = 0; index < vertices.Length; index++, v1 = v2) {
                v2 = vertices[index];
                if (((v1.Y <= point.Y) ^ (v2.Y <= point.Y)) ||
                    (v1.Y.NearlyEquals(point.Y)) || (v2.Y.NearlyEquals(point.Y))) {
                    var xIntersection = (v1.X + ((point.Y - v1.Y) / (v2.Y - v1.Y)) * (v2.X - v1.X));
                    if (point.X < xIntersection) // P.X < intersect
                    {
                        ++count;
                    } else if (xIntersection.NearlyEquals(point.X)) {
                        result = Containment.Contains;
                        return;
                    }
                }
            }
            result = ((count & 1) != 0) ? (Containment.Contains) : (Containment.Disjoint); //true if odd.
        }

        public static bool Intersects(Vector2[] vertexes1, Vector2[] vertexes2) {
            Contract.Requires(vertexes1 != null);
            Contract.Requires(vertexes1.Length > 1);
            Contract.Requires(vertexes2 != null);
            Contract.Requires(vertexes2.Length > 1);

            bool result;
            Intersects(vertexes1, vertexes2, out result);
            return result;
        }

        public static void Intersects(Vector2[] vertexes1, Vector2[] vertexes2, out bool result) {
            Contract.Requires(vertexes1 != null);
            Contract.Requires(vertexes1.Length > 1);
            Contract.Requires(vertexes2 != null);
            Contract.Requires(vertexes2.Length > 1);

            var v1 = vertexes1[vertexes1.Length - 1];
            Vector2 v2;
            var v3 = vertexes2[vertexes2.Length - 1];
            Vector2 v4;
            result = false;
            for (var index1 = 0; index1 < vertexes1.Length; ++index1, v1 = v2) {
                v2 = vertexes1[index1];
                for (var index2 = 0; index2 < vertexes2.Length; ++index2, v3 = v4) {
                    v4 = vertexes2[index2];
                    LineSegment.Intersects(ref v1, ref v2, ref v3, ref v4, out result);
                    if (result) { return; }
                }
            }
        }

        public static float GetDistance(Vector2[] vertices, Vector2 point) {
            float result;
            GetDistance(vertices, ref point, out result);
            return result;
        }

        public static void GetDistance(Vector2[] vertices, ref Vector2 point, out float result) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            var count = 0; //intersection count
            var v1 = vertices[vertices.Length - 1];
            Vector2 v2;
            var goodDistance = float.PositiveInfinity;
            for (var index = 0; index < vertices.Length; ++index, v1 = v2) {
                v2 = vertices[index];
                var t1 = (v1.Y <= point.Y);
                if (t1 ^ (v2.Y <= point.Y)) {
                    var temp = ((point.Y - v1.Y) * (v2.X - v1.X) - (point.X - v1.X) * (v2.Y - v1.Y));
                    if (t1) { if (temp > 0) { count++; } } else { if (temp < 0) { count--; } }
                }

                float distance;
                LineSegment.GetDistanceSq(ref v1, ref v2, ref point, out distance);
                if (distance < goodDistance) { goodDistance = distance; }
            }
            result = (float) Math.Sqrt(goodDistance);
            if (count != 0) {
                result = -result;
            }
        }

        /// <summary> Calculates the centroid of a polygon.</summary>
        /// <param name="vertices">The vertices of the polygon.</param>
        /// <returns>The centroid of a polygon.</returns>
        /// <remarks>This is also known as center of gravity/mass.</remarks>
        public static Vector2 GetCentroid(Vector2[] vertices) {
            Vector2 result;
            GetCentroid(vertices, out result);
            return result;
        }

        /// <summary> Calculates the centroid of a polygon.</summary>
        /// <param name="vertices">The vertices of the polygon.</param>
        /// <param name="centroid">The centroid of a polygon.</param>
        /// <remarks>This is also known as center of gravity/mass.</remarks>
        public static void GetCentroid(Vector2[] vertices, out Vector2 centroid) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            centroid = Vector2.Zero;
            float temp;
            float area = 0;
            var v1 = vertices[vertices.Length - 1];
            Vector2 v2;
            for (var index = 0; index < vertices.Length; ++index, v1 = v2) {
                v2 = vertices[index];
                Vectors2.ZCross(ref v1, ref v2, out temp);
                area += temp;
                centroid.X += ((v1.X + v2.X) * temp);
                centroid.Y += ((v1.Y + v2.Y) * temp);
            }

            temp = 1 / (Math.Abs(area) * 3);
            centroid.X *= temp;
            centroid.Y *= temp;
        }

        /// <summary>Calculates the area of a polygon.</summary>
        /// <param name="vertices">The vertices of the polygon.</param>
        /// <returns>the area.</returns>
        public static float GetArea(Vector2[] vertices) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            float result;
            GetArea(vertices, out result);
            return result;
        }

        /// <summary>
        ///     Calculates the area of a polygon.
        /// </summary>
        /// <param name="vertices">The vertices of the polygon.</param>
        /// <param name="result">the area.</param>
        public static void GetArea(Vector2[] vertices, out float result) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            float area = 0;
            var v1 = vertices[vertices.Length - 1];
            Vector2 v2;
            for (var index = 0; index < vertices.Length; ++index, v1 = v2) {
                v2 = vertices[index];
                float temp;
                Vectors2.ZCross(ref v1, ref v2, out temp);
                area += temp;
            }
            result = Math.Abs(area * .5f);
        }

        public static float GetPerimeter(Vector2[] vertices) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            float result;
            GetPerimeter(vertices, out result);
            return result;
        }

        public static void GetPerimeter(Vector2[] vertices, out float result) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            var v1 = vertices[vertices.Length - 1];
            Vector2 v2;
            result = 0;
            for (var index = 0; index < vertices.Length; ++index, v1 = v2) {
                v2 = vertices[index];
                float dist;
                Vector2.Distance(ref v1, ref v2, out dist);
                result += dist;
            }
        }

        public static float GetInertia(Vector2[] vertices) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 0);

            float result;
            GetInertia(vertices, out result);
            return result;
        }

        public static void GetInertia(Vector2[] vertices, out float result) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 0);

            if (vertices.Length == 1) {
                result = 0;
                return;
            }

            float denom = 0;
            float numer = 0;
            var v1 = vertices[vertices.Length - 1];
            Vector2 v2;
            for (var index = 0; index < vertices.Length; index++, v1 = v2) {
                v2 = vertices[index];
                float a, b, c, d;
                Vector2.Dot(ref v2, ref v2, out a);
                Vector2.Dot(ref v2, ref v1, out b);
                Vector2.Dot(ref v1, ref v1, out c);
                Vectors2.ZCross(ref v1, ref v2, out d);
                d = Math.Abs(d);
                numer += d;
                denom += (a + b + c) * d;
            }
            result = denom / (numer * 6);
        }

        public static bool operator ==(BoundingPolygon left, BoundingPolygon right) {
            return Equals(left, right);
        }

        public static bool operator !=(BoundingPolygon left, BoundingPolygon right) {
            return !Equals(left, right);
        }
    }
}