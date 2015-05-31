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
    /// <summary> </summary>
    [StructLayout(LayoutKind.Sequential, Size = Size), DataContract(Name = "bCircle", Namespace = "")]
    public struct BoundingCircle : IEquatable<BoundingCircle> {
        public const int Size = (sizeof(float) * 2) + sizeof(float);

        /// <summary> Empty bounding circle (point at coordinate system origin). </summary>
        public static readonly BoundingCircle Zero = new BoundingCircle(Vector2.Zero, 0);

        public BoundingCircle(Vector2 center, float radius) {
            Center = center;
            Radius = radius;
        }

        public BoundingCircle(float x, float y, float radius) {
            Center.X = x;
            Center.Y = y;
            Radius = radius;
        }

        /// <summary> Center coordinates. </summary>
        [DataMember(Name = "c", Order = 0)]
        public Vector2 Center;

        [DataMember(Name = "r", Order = 1)]
        public float Radius;

        public float Area { get { return MathHelper.Pi * Radius * Radius; } }

        public float Perimeter { get { return MathHelper.TwoPi * Radius; } }

        public float GetDistance(Vector2 point) {
            float result;
            GetDistance(ref point, out result);
            return result;
        }

        public void GetDistance(ref Vector2 point, out float result) {
            Vector2 diff;
            Vector2.Subtract(ref point, ref Center, out diff);
            result = diff.Length();
            result -= Radius;
        }

        public Containment Contains(Vector2 point) {
            float distance;
            GetDistance(ref point, out distance);
            return ((distance <= 0) ? (Containment.Contains) : (Containment.Disjoint));
        }

        public void Contains(ref Vector2 point, out Containment result) {
            float distance;
            GetDistance(ref point, out distance);
            result = ((distance <= 0) ? (Containment.Contains) : (Containment.Disjoint));
        }

        public Containment Contains(BoundingCircle circle) {
            float distance;
            GetDistance(ref circle.Center, out distance);
            if (-distance >= circle.Radius) {
                return Containment.Contains;
            }
            if (distance > circle.Radius) {
                return Containment.Disjoint;
            }
            return Containment.Intersects;
        }

        public void Contains(ref BoundingCircle circle, out Containment result) {
            float distance;
            GetDistance(ref circle.Center, out distance);
            if (-distance >= circle.Radius) {
                result = Containment.Contains;
            } else if (distance > circle.Radius) {
                result = Containment.Disjoint;
            } else {
                result = Containment.Intersects;
            }
        }

        public Containment Contains(BoundingRectangle rect) {
            Containment result;
            Contains(ref rect, out result);
            return result;
        }

        public void Contains(ref BoundingRectangle rect, out Containment result) {
            float mag;
            Vector2 maxDistance, minDistance;
            Vector2.Subtract(ref rect.Max, ref Center, out maxDistance);
            Vector2.Subtract(ref Center, ref rect.Min, out minDistance);
            NumberTools.Sort(ref minDistance.X, ref maxDistance.X, out minDistance.X, out maxDistance.X);
            NumberTools.Sort(ref minDistance.Y, ref maxDistance.Y, out minDistance.Y, out maxDistance.Y);
            mag = maxDistance.Length();
            if (mag <= Radius) {
                result = Containment.Contains;
            } else {
                mag = minDistance.Length();
                result = (mag <= Radius) ? Containment.Intersects : Containment.Disjoint;
            }
        }

        public Containment Contains(BoundingPolygon polygon) {
            Containment result;
            Contains(ref polygon, out result);
            return result;
        }

        // Duplicates with BoundingRectangle.
        public void Contains(ref BoundingPolygon polygon, out Containment result) {
            Contract.Requires(polygon != null);
            Contract.Requires(polygon.Vertices != null);

            var vertices = polygon.Vertices;
            result = Containment.Unknown;
            for (var index = 0; index < vertices.Length && result != Containment.Intersects; ++index) {
                Containment con;
                Contains(ref vertices[index], out con);
                result |= con;
            }

            if (result == Containment.Disjoint) {
                bool test;
                polygon.Intersects(ref this, out test);
                if (test) {
                    result = Containment.Intersects;
                }
            }
        }

        public float Intersects(Ray2D ray) {
            float result;
            Intersects(ref ray, true, out result);
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
            bool result;
            polygon.Intersects(ref this, out result);
            return result;
        }

        public bool Intersects(LineSegment line) {
            bool result;
            Intersects(ref line, out result);
            return result;
        }

        public bool Intersects(Line line) {
            bool result;
            Intersects(ref line, out result);
            return result;
        }

        public void Intersects(ref Ray2D ray, out float result) {
            Intersects(ref ray, true, out result);
        }

        public void Intersects(ref Ray2D ray, bool discardInside, out float result) {
            Vector2 rayOriginRelativeToCircle2D;
            Vector2.Subtract(ref ray.Origin, ref Center, out rayOriginRelativeToCircle2D);
            var radiusSq = Radius * Radius;
            var magSq = rayOriginRelativeToCircle2D.LengthSquared();

            if ((magSq <= radiusSq) && !discardInside) {
                result = 0;
                return;
            }
            var a = ray.Direction.LengthSquared();
            Vector2 rayOriginRelativeToCircle2DTwice;
            Vector2.Multiply(ref rayOriginRelativeToCircle2D, 2, out rayOriginRelativeToCircle2DTwice);
            float b;
            Vector2.Dot(ref rayOriginRelativeToCircle2DTwice, ref ray.Direction, out b);
            var c = magSq - radiusSq;
            float minus;
            float plus;
            if (NumberTools.TrySolveQuadratic(ref a, ref b, ref c, out plus, out minus)) {
                if (minus < 0) {
                    if (plus > 0) {
                        result = plus;
                        return;
                    }
                } else {
                    result = minus;
                    return;
                }
            }
            result = -1;
        }

        public void Intersects(ref LineSegment line, out bool result) {
            float distance;
            line.GetDistance(ref Center, out distance);
            result = Math.Abs(distance) <= Radius;
        }

        public void Intersects(ref Line line, out bool result) {
            float distance;
            Vector2.Dot(ref line.Normal, ref Center, out distance);
            result = (distance + line.D) <= Radius;
        }

        public void Intersects(ref BoundingRectangle rect, out bool result) {
            Vector2 proj;
            Vector2.Clamp(ref Center, ref rect.Min, ref rect.Max, out proj);
            float distSq;
            Vectors2.DistanceSq(ref Center, ref proj, out distSq);
            result = distSq <= Radius * Radius;
        }

        public void Intersects(ref BoundingCircle circle, out bool result) {
            float distSq;
            Vectors2.DistanceSq(ref Center, ref circle.Center, out distSq);
            result = distSq <= (Radius * Radius + circle.Radius * circle.Radius);
        }

        public void Intersects(ref BoundingPolygon polygon, out bool result) {
            Contract.Requires(polygon != null);

            polygon.Intersects(ref this, out result);
        }


        public override string ToString() {
            return "(" + Center.Format() + "), r = " + Radius.ToString("0.##", CultureInfo.InvariantCulture);
        }

        public override int GetHashCode() {
            return Center.GetHashCode() ^ Radius.GetHashCode();
        }

        public override bool Equals(object obj) {
            return obj is BoundingCircle && Equals((BoundingCircle) obj);
        }

        public bool Equals(BoundingCircle other) {
            return Equals(ref this, ref other);
        }

        public static bool Collide(BoundingCircle circle1, BoundingCircle circle2, out Vector2 contactPoint, 
            out float distance, out Vector2 normal) {
            Vector2.Subtract(ref circle2.Center, ref circle1.Center, out normal);
            Vectors2.Normalize(ref normal, out distance, out normal);
            distance = distance - (circle1.Radius + circle2.Radius);
            bool collision = (distance <= 0);
            if (collision) {
                contactPoint.X = circle2.Center.X - normal.X * circle2.Radius;
                contactPoint.Y = circle2.Center.Y - normal.Y * circle2.Radius;
            } else {
                contactPoint = Vector2.Zero;
            }

            return collision;
        }

        public static BoundingCircle FromRectangle(BoundingRectangle rect) {
            BoundingCircle result;
            FromRectangle(ref rect, out result);
            return result;
        }

        public static void FromRectangle(ref BoundingRectangle rect, out BoundingCircle result) {
            result.Center.X = (rect.Min.X + rect.Max.X) * .5f;
            result.Center.Y = (rect.Min.Y + rect.Max.Y) * .5f;
            var xRadius = (rect.Max.X - rect.Min.X) * .5f;
            var yRadius = (rect.Max.Y - rect.Min.Y) * .5f;
            result.Radius = (float) Math.Sqrt(xRadius * xRadius + yRadius * yRadius);
        }

        public static BoundingCircle FromVectors(Vector2[] vertices) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            BoundingCircle result;
            FromVectors(vertices, out result);
            return result;
        }

        public static void FromVectors(Vector2[] vertices, out BoundingCircle result) {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Length > 2);

            BoundingPolygon.GetCentroid(vertices, out result.Center);
            result.Radius = -1;
            for (var index = 0; index < vertices.Length; ++index) {
                float distSq;
                Vectors2.DistanceSq(ref result.Center, ref vertices[index], out distSq);
                if (result.Radius == -1 || (distSq < result.Radius)) {
                    result.Radius = distSq;
                }
            }

            result.Radius = (float) Math.Sqrt(result.Radius);
        }

        public static bool Equals(BoundingCircle circle1, BoundingCircle circle2) {
            return Equals(ref circle1, ref circle2);
        }

        [CLSCompliant(false)]
        public static bool Equals(ref BoundingCircle circle1, ref BoundingCircle circle2) {
            return Vectors2.Equals(ref circle1.Center, ref circle2.Center) &&
                   circle1.Radius.NearlyEquals(circle2.Radius);
        }


        public static BoundingCircle Parse(string source) {
            Contract.Requires(!String.IsNullOrWhiteSpace(source));

            BoundingCircle result;
            if (!TryParse(source, out result)) {
                throw new ArgumentException("Source string does not contain 3 numbers.", "source");
            }

            return result;
        }

        public static bool TryParse(string source, out BoundingCircle result) {
            bool validParse = NumberTools.ParseVectorAndNumber(source, out result.Center, out result.Radius);
            if (validParse) { return true; }

            result = Zero;
            return false;
        }

        public static bool operator ==(BoundingCircle circle1, BoundingCircle circle2) {
            return Equals(ref circle1, ref circle2);
        }

        public static bool operator !=(BoundingCircle circle1, BoundingCircle circle2) {
            return !Equals(ref circle1, ref circle2);
        }
    }
}