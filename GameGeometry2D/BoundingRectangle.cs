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
    /// <summary>Axis-aligned bounding rectangle.</summary>
    [StructLayout(LayoutKind.Sequential, Size = ByteSize), DataContract(Name = "aabb", Namespace = ""), 
    KnownType(typeof(Vector2))]
    public struct BoundingRectangle : IEquatable<BoundingRectangle> {
        public const int ByteSize = (sizeof(float) * 2) * 2;

        /// <summary> Empty bounding rectangle (point at coordinate system origin). </summary>
        public static readonly BoundingRectangle Empty = new BoundingRectangle(0, 0, 0, 0);

        /// <summary>Initializes a new instance of the <see cref="BoundingRectangle"/> struct.</summary>
        /// <param name="minX">The lower bound on the X axis.</param>
        /// <param name="minY">The lower bound on the Y axis.</param>
        /// <param name="maxX">The upper bound on the X axis.</param>
        /// <param name="maxY">The upper bound on the Y axis.</param>
        public BoundingRectangle(float minX, float minY, float maxX, float maxY) {
            NumberTools.Sort(ref minX, ref maxX, out minX, out maxX);
            NumberTools.Sort(ref minY, ref maxY, out minY, out maxY);
            Max.X = maxX;
            Max.Y = maxY;
            Min.X = minX;
            Min.Y = minY;
        }

        /// <summary>Initializes a new instance of the <see cref="BoundingRectangle"/> struct.</summary>
        /// <param name="min">Corner with lower coordinates.</param>
        /// <param name="max">Corner with upper coordinates.</param>
        public BoundingRectangle(Vector2 min, Vector2 max) 
            : this (min.X, min.Y, max.X, max.Y) {
        }

        [DataMember(Name = "min", Order = 0)]
        public Vector2 Min;

        [DataMember(Name = "max", Order = 1)]
        public Vector2 Max;

        public float Area { get { return (Max.X - Min.X) * (Max.Y - Min.Y); } }

        public float Perimeter { get { return ((Max.X - Min.X) + (Max.Y - Min.Y)) * 2; } }

        public Vector2[] Corners() {
            return new[] {
                Min,
                new Vector2(Max.X, Min.Y),
                Max,
                new Vector2(Min.X, Max.Y)
            };
        }

        /// <summary>Gets the center position.</summary>
        public Vector2 Center { get { return new Vector2(Min.X + (Max.X - Min.X) / 2, Min.Y + (Max.Y - Min.Y) / 2); } }

        public float Width { get { return Max.X - Min.X; } }

        public float Height { get { return Max.Y - Min.Y; } }

        public Vector2 Size { get { return new Vector2(Max.X - Min.X, Max.Y - Min.Y); } }

        /// <summary> Finds point the edge of the rectangle nearest to the specified point. </summary>
        /// <param name="target">The target point.</param>
        /// <returns>Point the edge of the rectangle nearest to the specified point.</returns>
        public Vector2 ClosestEdgePoint(Vector2 target) {
            Vector2 result;
            ClosestEdgePoint(ref target, out result);
            return result;
        }

        /// <summary> Finds point the edge of the rectangle nearest to the specified point. </summary>
        /// <param name="target">The target point.</param>
        /// <param name="closestEdgePoint">Point the edge of the rectangle nearest to the specified point.</param>
        public void ClosestEdgePoint(ref Vector2 target, out Vector2 closestEdgePoint) {
            float x, y;
            NumberTools.Clamp(ref target.X, ref Min.X, ref Max.X, out x);
            NumberTools.Clamp(ref target.Y, ref Min.Y, ref Max.Y, out y);

            float dl = Math.Abs(x - Min.X);
            float dr = Math.Abs(x - Max.X);
            float dt = Math.Abs(y - Min.Y);
            float db = Math.Abs(y - Max.Y);
            float m = Math.Min(Math.Min(Math.Min(dl, dr), dt), db);

            if (m.NearlyEquals(dt)) {
                closestEdgePoint.X = x;
                closestEdgePoint.Y = Min.Y;
            } else if (m.NearlyEquals(db)) {
                closestEdgePoint.X = x;
                closestEdgePoint.Y = Max.Y;
            } else if (m.NearlyEquals(dl)) {
                closestEdgePoint.X = Min.X;
                closestEdgePoint.Y = y;
            } else {
                closestEdgePoint.X = Max.X;
                closestEdgePoint.Y = y;
            }
        }

        /// <summary>Gets distance from the edge to the specified point.</summary>
        /// <param name="point">The target point.</param>
        /// <returns>Distance from the edge to the specified point.</returns>
        public float DistanceTo(Vector2 point) {
            float result;
            DistanceTo(ref point, out result);
            return result;
        }

        public void DistanceTo(ref Vector2 point, out float distance) {
            Vector2 offset;
            offset.X = Math.Abs(point.X - ((Max.X + Min.X) * .5f)) - (Max.X - Min.X) * .5f;
            offset.Y = Math.Abs(point.Y - ((Max.Y + Min.Y) * .5f)) - (Max.Y - Min.Y) * .5f;
            if (offset.X > 0 && offset.Y > 0) {
                distance =  (float) Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y);
            } else {
                distance = Math.Max(offset.X, offset.Y);
            }
        }

        /// <summary>Gets distance from the edge of this AABB to the closest edge of the specified AABB.</summary>
        /// <param name="target">The target AABB.</param>
        /// <param name="distance">Distance from the edge of this AABB to the closest edge of the specified AABB.
        /// </param>
        public void DistanceTo(ref BoundingRectangle target, out float distance) {
            // We shrink target down to a point and expand this AABB by the extents of target, 
            // then we can use a simple distance from AABB to point function.
            Vector2 shrinked = target.Center;
            BoundingRectangle expanded = this;
            expanded.Grow(target.Width, target.Height);
            expanded.DistanceTo(ref shrinked, out distance);
        }

        /// <summary>Clamps the specified point to be contained within the box.</summary>
        /// <param name="target">The target point.</param>
        /// <returns>Clamped point.</returns>
        public Vector2 Clamp(Vector2 target) {
            Vector2 clamped;
            Clamp(ref target, out clamped);
            return clamped;
        }

        /// <summary>Clamps the specified point to be contained within the box.</summary>
        /// <param name="target">The target point.</param>
        /// <param name="clamped">The clamped point.</param>
        public void Clamp(ref Vector2 target, out Vector2 clamped) {
            Vectors2.Clamp(ref target, ref Min, ref Max, out clamped);
        }

        public Containment Contains(Vector2 point) {
            if (point.X <= Max.X && point.X >= Min.X && point.Y <= Max.Y && point.Y >= Min.Y) {
                return Containment.Contains;
            }

            return Containment.Disjoint;
        }

        public void Contains(ref Vector2 point, out Containment result) {
            if (point.X <= Max.X && point.X >= Min.X && point.Y <= Max.Y && point.Y >= Min.Y) {
                result = Containment.Contains;
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
            if (Min.X > rect.Max.X ||
                Min.Y > rect.Max.Y ||
                Max.X < rect.Min.X ||
                Max.Y < rect.Min.Y) {
                result = Containment.Disjoint;
            } else if (
                Min.X <= rect.Min.X &&
                Min.Y <= rect.Min.Y &&
                Max.X >= rect.Max.X &&
                Max.Y >= rect.Max.Y) {
                result = Containment.Contains;
            } else {
                result = Containment.Intersects;
            }
        }

        public Containment Contains(BoundingCircle circle) {
            Containment result;
            Contains(ref circle, out result);
            return result;
        }

        public void Contains(ref BoundingCircle circle, out Containment result) {
            if ((circle.Center.X + circle.Radius) <= Max.X &&
                (circle.Center.X - circle.Radius) >= Min.X &&
                (circle.Center.Y + circle.Radius) <= Max.Y &&
                (circle.Center.Y - circle.Radius) >= Min.Y) {
                result = Containment.Contains;
            } else {
                bool intersects;
                circle.Intersects(ref this, out intersects);
                result = intersects ? Containment.Intersects : Containment.Disjoint;
            }
        }

        public Containment Contains(BoundingPolygon polygon) {
            Contract.Requires(polygon != null);
            Contract.Requires(polygon.Vertices != null);

            Containment result;
            Contains(ref polygon, out result);
            return result;
        }

        // Duplicates with BoundingCircle.
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

        /// <summary> Adds to the box size while preserving its center position. </summary>
        /// <param name="amount">Size increase.</param>
        public void Grow(float amount) {
            Grow(amount, amount);
        }

        /// <summary> Adds to the box size while preserving its center position. </summary>
        /// <param name="amountX">Size increase on X axis.</param>
        /// <param name="amountY">Size increase on Y axis.</param>
        public void Grow(float amountX, float amountY) {
            Min.X -= amountX / 2;
            Min.Y -= amountY / 2;
            Max.X += amountX / 2;
            Max.Y += amountY / 2;
        }

        public float Intersects(Ray2D ray) {
            float result;
            Intersects(ref ray, out result);
            return result;
        }

        public bool Intersects(BoundingRectangle rect) {
            return
                Min.X < rect.Max.X &&
                Max.X > rect.Min.X &&
                Max.Y > rect.Min.Y &&
                Min.Y < rect.Max.Y;
        }

        public bool Intersects(BoundingCircle circle) {
            bool result;
            circle.Intersects(ref this, out result);
            return result;
        }

        public bool Intersects(Line line) {
            bool result;
            line.Intersects(ref this, out result);
            return result;
        }

        public bool Intersects(BoundingPolygon polygon) {
            Contract.Requires(polygon != null);
            Contract.Requires(polygon.Vertices != null);
            Contract.Requires(polygon.Vertices.Length > 1);

            bool result;
            polygon.Intersects(ref this, out result);
            return result;
        }

        public void Intersects(ref Ray2D ray, out float result) {
            if (Contains(ray.Origin) == Containment.Contains) {
                result = 0;
                return;
            }
            
            float distance;
            float intersectValue;
            result = -1;
            if (ray.Origin.X < Min.X && ray.Direction.X > 0) {
                distance = (Min.X - ray.Origin.X) / ray.Direction.X;
                if (distance > 0) {
                    intersectValue = ray.Origin.Y + ray.Direction.Y * distance;
                    if (intersectValue >= Min.Y && intersectValue <= Max.Y &&
                        (result == -1 || distance < result)) {
                        result = distance;
                    }
                }
            }

            if (ray.Origin.X > Max.X && ray.Direction.X < 0) {
                distance = (Max.X - ray.Origin.X) / ray.Direction.X;
                if (distance > 0) {
                    intersectValue = ray.Origin.Y + ray.Direction.Y * distance;
                    if (intersectValue >= Min.Y && intersectValue <= Max.Y &&
                        (result == -1 || distance < result)) {
                        result = distance;
                    }
                }
            }

            if (ray.Origin.Y < Min.Y && ray.Direction.Y > 0) {
                distance = (Min.Y - ray.Origin.Y) / ray.Direction.Y;
                if (distance > 0) {
                    intersectValue = ray.Origin.X + ray.Direction.X * distance;
                    if (intersectValue >= Min.X && intersectValue <= Max.X &&
                        (result == -1 || distance < result)) {
                        result = distance;
                    }
                }
            }

            if (ray.Origin.Y > Max.Y && ray.Direction.Y < 0) {
                distance = (Max.Y - ray.Origin.Y) / ray.Direction.Y;
                if (distance > 0) {
                    intersectValue = ray.Origin.X + ray.Direction.X * distance;
                    if (intersectValue >= Min.X && intersectValue <= Max.X &&
                        (result == -1 || distance < result)) {
                        result = distance;
                    }
                }
            }
        }

        public void Intersects(ref BoundingRectangle rect, out bool result) {
            result =
                Min.X <= rect.Max.X &&
                Max.X >= rect.Min.X &&
                Max.Y >= rect.Min.Y &&
                Min.Y <= rect.Max.Y;
        }

        public void Intersects(ref BoundingCircle circle, out bool result) {
            circle.Intersects(ref this, out result);
        }

        public void Intersects(ref BoundingPolygon polygon, out bool result) {
            Contract.Requires(polygon != null);
            Contract.Requires(polygon.Vertices != null);
            Contract.Requires(polygon.Vertices.Length > 1);

            polygon.Intersects(ref this, out result);
        }

        public void Intersects(ref Line line, out bool result) {
            line.Intersects(ref this, out result);
        }

        public override int GetHashCode() {
            return Min.GetHashCode() ^ Max.GetHashCode();
        }

        public override string ToString() {
            return "(" + Min.Format() + ") < (" + Max.Format() + ")";
        }

        public override bool Equals(object obj) {
            return obj is BoundingRectangle && Equals((BoundingRectangle) obj);
        }

        public bool Equals(BoundingRectangle other) {
            return Equals(ref this, ref other);
        }


        public static void Transform(ref Matrix matrix, ref BoundingRectangle rect, out BoundingRectangle result) {
            FromVectors(rect.Corners(), ref matrix, out result);
        }

        /// <summary>Creates a new bounding rectangle instance from 2 vectors.</summary>
        /// <param name="first">the first vector.</param>
        /// <param name="second">the second vector.</param>
        /// <returns>a new bounding rectangle</returns>
        /// <remarks>The Max and Min values are automatically determined.</remarks>
        public static BoundingRectangle FromVectors(Vector2 first, Vector2 second) {
            BoundingRectangle result;
            FromVectors(ref first, ref second, out result);
            return result;
        }

        public static void FromVectors(ref Vector2 first, ref Vector2 second, out BoundingRectangle result) {
            if (first.X > second.X) {
                result.Max.X = first.X;
                result.Min.X = second.X;
            } else {
                result.Max.X = second.X;
                result.Min.X = first.X;
            }
            if (first.Y > second.Y) {
                result.Max.Y = first.Y;
                result.Min.Y = second.Y;
            } else {
                result.Max.Y = second.Y;
                result.Min.Y = first.Y;
            }
        }

        /// <summary>
        ///     Creates a new BoundingRectangle Instance from multiple Vector2Ds.
        /// </summary>
        /// <param name="vectors">the list of vectors</param>
        /// <returns>a new BoundingRectangle</returns>
        /// <remarks>The Max and Min values are automatically determined.</remarks>
        public static BoundingRectangle FromVectors(Vector2[] vectors) {
            BoundingRectangle result;
            FromVectors(vectors, out result);
            return result;
        }

        public static void FromVectors(Vector2[] vectors, out BoundingRectangle result) {
            Contract.Requires(vectors != null);
            Contract.Requires(vectors.Length > 0);

            Matrix identity = Matrix.Identity;
            FromVectors(vectors, ref identity, out result);
        }

        public static void FromVectors(Vector2[] vectors, ref Matrix matrix, out BoundingRectangle result) {
            Contract.Requires(vectors != null);
            Contract.Requires(vectors.Length > 0);
            bool useMatrix = (matrix != Matrix.Identity);

            Vector2 current;
            if (useMatrix) {
                Vector2.Transform(ref vectors[0], ref matrix, out current);
            } else {
                current = vectors[0];
            }

            result.Max = current;
            result.Min = current;
            for (var index = 1; index < vectors.Length; ++index) {
                if (useMatrix) {
                    Vector2.Transform(ref vectors[index], ref matrix, out current);
                } else {
                    current = vectors[index];
                }

                if (current.X > result.Max.X) {
                    result.Max.X = current.X;
                } else if (current.X < result.Min.X) {
                    result.Min.X = current.X;
                }
                if (current.Y > result.Max.Y) {
                    result.Max.Y = current.Y;
                } else if (current.Y < result.Min.Y) {
                    result.Min.Y = current.Y;
                }
            }
        }

        public static void FromNormalVectors(Vector2[] vectors, ref Matrix matrix, out BoundingRectangle result) {
            Contract.Requires(vectors != null);
            Contract.Requires(vectors.Length > 0);

            Vector2 current;
            Vector2.TransformNormal(ref vectors[0], ref matrix, out current);
            result.Max = current;
            result.Min = current;
            for (var index = 1; index < vectors.Length; ++index) {
                Vector2.TransformNormal(ref vectors[index], ref matrix, out current);
                if (current.X > result.Max.X) {
                    result.Max.X = current.X;
                } else if (current.X < result.Min.X) {
                    result.Min.X = current.X;
                }
                if (current.Y > result.Max.Y) {
                    result.Max.Y = current.Y;
                } else if (current.Y < result.Min.Y) {
                    result.Min.Y = current.Y;
                }
            }
            result.Max.X += matrix.M13;
            result.Max.Y += matrix.M23;
            result.Min.X += matrix.M13;
            result.Min.Y += matrix.M23;
        }

        /// <summary>
        ///     Makes a BoundingRectangle that can contain the 2 BoundingRectangles passed.
        /// </summary>
        /// <param name="first">The First BoundingRectangle.</param>
        /// <param name="second">The Second BoundingRectangle.</param>
        /// <returns>The BoundingRectangle that can contain the 2 BoundingRectangles passed.</returns>
        public static BoundingRectangle FromUnion(BoundingRectangle first, BoundingRectangle second) {
            BoundingRectangle result;
            FromUnion(ref first, ref second, out result);
            return result;
        }

        public static void FromUnion(ref BoundingRectangle first, ref BoundingRectangle second,
            out BoundingRectangle result) {
            Vector2.Max(ref first.Max, ref second.Max, out result.Max);
            Vector2.Min(ref first.Min, ref second.Min, out result.Min);
        }

        /// <summary>
        ///     Makes a BoundingRectangle that contains the area where the BoundingRectangles Intersect.
        /// </summary>
        /// <param name="first">The First BoundingRectangle.</param>
        /// <param name="second">The Second BoundingRectangle.</param>
        /// <returns>The BoundingRectangle that can contain the 2 BoundingRectangles passed.</returns>
        public static BoundingRectangle FromIntersection(BoundingRectangle first, BoundingRectangle second) {
            BoundingRectangle result;
            FromIntersection(ref first, ref second, out result);
            return result;
        }

        public static void FromIntersection(ref BoundingRectangle first, ref BoundingRectangle second,
            out BoundingRectangle result) {
            Vector2.Min(ref first.Max, ref second.Max, out result.Max);
            Vector2.Max(ref first.Min, ref second.Min, out result.Min);
            if (result.Min.X > result.Max.X || result.Min.Y > result.Max.Y) {
                result.Min = result.Max = Vector2.Zero;
            }
        }

        public static BoundingRectangle FromCircle(BoundingCircle circle) {
            BoundingRectangle result;
            FromCircle(ref circle, out result);
            return result;
        }

        public static void FromCircle(ref BoundingCircle circle, out BoundingRectangle result) {
            result.Max.X = circle.Center.X + circle.Radius;
            result.Max.Y = circle.Center.Y + circle.Radius;
            result.Min.X = circle.Center.X - circle.Radius;
            result.Min.Y = circle.Center.Y - circle.Radius;
        }

        public static BoundingRectangle FromCircle(Matrix matrix, float radius) {
            BoundingRectangle result;
            FromCircle(ref matrix, ref radius, out result);
            return result;
        }

        public static void FromCircle(ref Matrix matrix, ref float radius, out BoundingRectangle result) {
            var xRadius = matrix.M12 * matrix.M12 + matrix.M11 * matrix.M11;
            xRadius = (xRadius.NearlyEquals(1) ? (radius) : (radius * (float) Math.Sqrt(xRadius)));
            var yRadius = matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22;
            yRadius = (yRadius.NearlyEquals(1) ? (radius) : (radius * (float) Math.Sqrt(yRadius)));

            result.Max.X = matrix.M13 + xRadius;
            result.Min.X = matrix.M13 - xRadius;
            result.Max.Y = matrix.M23 + yRadius;
            result.Min.Y = matrix.M23 - yRadius;
        }

        public static void MinkowskiDifference(ref BoundingRectangle a, ref BoundingRectangle b, 
            out BoundingRectangle result) {
            Vector2.Subtract(ref a.Min, ref b.Max, out result.Min);
            result.Max = new Vector2(result.Min.X + a.Width + b.Width, result.Min.Y + a.Height + b.Height);
        }

        public static BoundingRectangle Parse(string source) {
            Contract.Requires(!String.IsNullOrWhiteSpace(source));

            BoundingRectangle result;
            if (!TryParse(source, out result)) {
                throw new ArgumentException("Source string does not contain 4 numbers.", "source");
            }

            return result;
        }

        public static bool TryParse(string source, out BoundingRectangle result) {
            bool validParse = NumberTools.ParseTwoVectors(source, out result.Min, out result.Max);
            if (validParse) { return true; }

            result = Empty;
            return false;
        }

        public static bool Equals(BoundingRectangle rect1, BoundingRectangle rect2) {
            return Equals(ref rect1, ref rect2);
        }

        [CLSCompliant(false)]
        public static bool Equals(ref BoundingRectangle rect1, ref BoundingRectangle rect2) {
            return Vectors2.Equals(ref rect1.Min, ref rect2.Min) && Vectors2.Equals(ref rect1.Max, ref rect2.Max);
        }

        public static bool operator ==(BoundingRectangle rect1, BoundingRectangle rect2) {
            return Equals(ref rect1, ref rect2);
        }

        public static bool operator !=(BoundingRectangle rect1, BoundingRectangle rect2) {
            return !Equals(ref rect1, ref rect2);
        }
    }
}