using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace GameGeometry2D.Tests {
    [TestFixture]
    public class BoundingRectangleTest {
        // BoundingPolygon polygon2;

        [SetUp]
        public void Init() {
            circle1 = new BoundingCircle(Vector2.UnitY, 2);
            circle2 = new BoundingCircle(Vector2.UnitX, 2);
            rect1 = BoundingRectangle.FromCircle(circle1);
            circle3 = BoundingCircle.FromRectangle(rect1);
            rect2 = BoundingRectangle.FromCircle(circle3);
            rect0 = new BoundingRectangle(-1, 3.01f, 1, 6);
            _testAabb = new BoundingRectangle(0, 0, 20, 20);
        }

        private BoundingCircle circle1;
        private BoundingCircle circle2;
        private BoundingCircle circle3;
        private BoundingRectangle rect0;
        private BoundingRectangle rect1;
        private BoundingRectangle rect2;
        private BoundingRectangle _testAabb;

        [Test]
        public void ContainsCircle() {
            Assert.AreEqual(Containment.Contains, rect2.Contains(circle3), "1");
            Assert.AreEqual(Containment.Intersects, rect1.Contains(circle3), "2");
            Assert.AreEqual(Containment.Intersects, rect1.Contains(circle2), "3");
            Assert.AreEqual(Containment.Disjoint, rect0.Contains(circle1), "4");
        }

        [Test]
        public void ContainsPoint() {
            var rect = new BoundingRectangle(0, 0, 2, 2);
            Assert.AreEqual(Containment.Contains, rect.Contains(new Vector2(1, 1)), "1");
            Assert.AreEqual(Containment.Contains, rect.Contains(new Vector2(2, 2)), "2");
            Assert.AreEqual(Containment.Contains, rect.Contains(new Vector2(0, 2)), "3");
            Assert.AreEqual(Containment.Contains, rect.Contains(new Vector2(0, 0)), "4");
            Assert.AreEqual(Containment.Disjoint, rect.Contains(new Vector2(2, 3)), "5");
            Assert.AreEqual(Containment.Disjoint, rect.Contains(new Vector2(-1, 0)), "6");
            Assert.AreEqual(Containment.Disjoint, rect.Contains(new Vector2(-.0001f, 0)), "7");
            Assert.AreEqual(Containment.Disjoint, rect.Contains(new Vector2(3, 1)), "8");
            Assert.AreEqual(Containment.Disjoint, rect.Contains(new Vector2(1, -1)), "9");
        }

        [Test]
        public void ContainsPolygon() {
            var exactlyOverlappingPolygon = new BoundingPolygon(_testAabb.Corners());
            var closelyContainedPolygon =  new BoundingPolygon(
                new BoundingRectangle(0.001f, 0.001f, 19.999f, 19.999f).Corners());
            var closelyNonIntersectingPolygon =  new BoundingPolygon(
                new BoundingRectangle(-1, -1, -0.001f, -0.001f).Corners());
            var closelyIntersectingPolygon =  new BoundingPolygon(
                new BoundingRectangle(-1, -1, 0.001f, 0.001f).Corners());

            Assert.That(_testAabb.Contains(exactlyOverlappingPolygon), Is.EqualTo(Containment.Contains));
            Assert.That(_testAabb.Contains(closelyContainedPolygon), Is.EqualTo(Containment.Contains));
            Assert.That(_testAabb.Contains(closelyNonIntersectingPolygon), Is.EqualTo(Containment.Disjoint));
            Assert.That(_testAabb.Contains(closelyIntersectingPolygon), Is.EqualTo(Containment.Intersects));
        }

        [Test]
        public void ContainsRectangle() {
            Assert.AreEqual(Containment.Contains, rect0.Contains(rect0), "1");
            Assert.AreEqual(Containment.Contains, rect1.Contains(rect1), "2");
            Assert.AreEqual(Containment.Contains, rect2.Contains(rect2), "3");
            Assert.AreEqual(Containment.Contains, rect2.Contains(rect1), "4");
            Assert.AreEqual(Containment.Disjoint, rect0.Contains(rect1), "5");
            Assert.AreEqual(Containment.Intersects, rect1.Contains(rect2), "6");
        }

        [Test]
        public void IntersectsCircle() {
            var nonOverlappingCircle = new BoundingCircle(21, 21, 1);
            var closelyNonOverlappingCircle = new BoundingCircle(20.711f, 20.711f, 1);
            var closelyOverlappingCircle = new BoundingCircle(20.699f, 20.699f, 1);
            var containedCircle = new BoundingCircle(10, 10, 5);
            Assert.That(_testAabb.Intersects(nonOverlappingCircle), Is.False);
            Assert.That(_testAabb.Intersects(closelyNonOverlappingCircle), Is.False);
            Assert.That(_testAabb.Intersects(closelyOverlappingCircle), Is.True);
            Assert.That(_testAabb.Intersects(containedCircle), Is.True);
        }

        [Test]
        public void IntersectsPolygon() {
            var exactlyNonOverlappingPolygon = new BoundingPolygon(new BoundingRectangle(20, 20, 20, 20).Corners());
            var exactlyOverlappingPolygon = new BoundingPolygon(_testAabb.Corners());
            var closelyNonIntersectingPolygon = new BoundingPolygon(
                new BoundingRectangle(-1, -1, -0.001f, -0.001f).Corners());
            var closelyIntersectingPolygon = new BoundingPolygon(
                new BoundingRectangle(-1, -1, 0.001f, 0.001f).Corners());
            Assert.That(_testAabb.Intersects(exactlyNonOverlappingPolygon), Is.False);
            Assert.That(_testAabb.Intersects(exactlyOverlappingPolygon), Is.True);
            Assert.That(_testAabb.Intersects(closelyNonIntersectingPolygon), Is.False);
            Assert.That(_testAabb.Intersects(closelyIntersectingPolygon), Is.True);
        }

        [Test]
        public void IntersectsRectangle() {
            var exactlyNonOverlappingAabb = new BoundingRectangle(20, 20, 20, 20);
            var closelyNonOverlappingAabb = new BoundingRectangle(20.001f, 20.001f, 20, 20);
            var closelyOverlappingAabb = new BoundingRectangle(19.999f, 19.999f, 20.001f, 20.001f);
            var containedAabb = new BoundingRectangle(10, 10, 8, 8);
            Assert.That(_testAabb.Intersects(exactlyNonOverlappingAabb), Is.False);
            Assert.That(_testAabb.Intersects(closelyNonOverlappingAabb), Is.False);
            Assert.That(_testAabb.Intersects(closelyOverlappingAabb), Is.True);
            Assert.That(_testAabb.Intersects(containedAabb), Is.True);
        }

        [Test]
        public void WhenAABBIsEnlargedExtentsAreChangedAppropriately() {
            _testAabb.Grow(5);

            Assert.That(_testAabb.Center, Is.EqualTo(new Vector2(10, 10)));
            Assert.That(_testAabb.Min, Is.EqualTo(new Vector2(-2.5f, -2.5f)));
            Assert.That(_testAabb.Max, Is.EqualTo(new Vector2(22.5f, 22.5f)));
        }


        [Test]
        public void WhenDisjointedVectorIsClampedToAABBResultIsContainedWithin() {
            var minClamped = _testAabb.Clamp(new Vector2(-100, -100));
            var maxClamped = _testAabb.Clamp(new Vector2(100, 100));
            var nearClamped = _testAabb.Clamp(new Vector2(20.001f, 20.001f));

            Assert.That(minClamped, Is.EqualTo(_testAabb.Min));
            Assert.That(maxClamped, Is.EqualTo(_testAabb.Max));
            Assert.That(nearClamped, Is.EqualTo(_testAabb.Max));
        }

        [Test]
        public void WhenContainedVectorIsClampedToAABBResultIsUnchanged() {
            var minClamped = _testAabb.Clamp(new Vector2(0.001f, 0.001f));
            var maxClamped = _testAabb.Clamp(new Vector2(19.999f, 19.999f));
            var centerClamped = _testAabb.Clamp(new Vector2(10, 10));

            Assert.That(minClamped, Is.EqualTo(new Vector2(0.001f, 0.001f)));
            Assert.That(maxClamped, Is.EqualTo(new Vector2(19.999f, 19.999f)));
            Assert.That(centerClamped, Is.EqualTo(new Vector2(10, 10)));
        }

        [Test]
        public void WhenMeasuringDistanceToPointsOnAllSidesResultsAreValid() {
            var someAabb = new BoundingRectangle(100, 100, 120, 120);
            var diagonalPoint = new Vector2(121, 121);
            var topSidePoint = new Vector2(110, 99);
            var rightSidePoint = new Vector2(121, 110);
            var bottomSidePoint = new Vector2(110, 121);
            var leftSidePoint = new Vector2(99, 110);
            var farAwayPoint = new Vector2(-100000000, 0);
            var containedPoint = new Vector2(110, 110);
            var edgePoint = new Vector2(100, 110);

            Assert.That(someAabb.DistanceTo(diagonalPoint), Is.EqualTo(1.41421354f));
            Assert.That(someAabb.DistanceTo(topSidePoint), Is.EqualTo(1f));
            Assert.That(someAabb.DistanceTo(rightSidePoint), Is.EqualTo(1f));
            Assert.That(someAabb.DistanceTo(bottomSidePoint), Is.EqualTo(1f));
            Assert.That(someAabb.DistanceTo(leftSidePoint), Is.EqualTo(1f));
            Assert.That(someAabb.DistanceTo(farAwayPoint), Is.EqualTo(100000104f));
            Assert.That(someAabb.DistanceTo(containedPoint), Is.EqualTo(-10f));
            Assert.That(someAabb.DistanceTo(edgePoint), Is.EqualTo(0f));
        }

        [Test]
        public void WhenFindingClosestEdgePointResultsAreValid() {
            var someAabb = new BoundingRectangle(100, 100, 120, 120);
            var diagonalPoint = new Vector2(121, 121);
            var topSidePoint = new Vector2(110, 99);
            var rightSidePoint = new Vector2(121, 110);
            var bottomSidePoint = new Vector2(110, 121);
            var leftSidePoint = new Vector2(99, 110);
            var farAwayPoint = new Vector2(-100000000, 0);
            var containedCenterPoint = new Vector2(110, 110);
            var containedPoint = new Vector2(115, 115);
            var edgePoint = new Vector2(100, 110);

            Assert.That(someAabb.ClosestEdgePoint(diagonalPoint), Is.EqualTo(new Vector2(120, 120)));
            Assert.That(someAabb.ClosestEdgePoint(topSidePoint), Is.EqualTo(new Vector2(110, 100)));
            Assert.That(someAabb.ClosestEdgePoint(rightSidePoint), Is.EqualTo(new Vector2(120, 110)));
            Assert.That(someAabb.ClosestEdgePoint(bottomSidePoint), Is.EqualTo(new Vector2(110, 120)));
            Assert.That(someAabb.ClosestEdgePoint(leftSidePoint), Is.EqualTo(new Vector2(100, 110)));
            Assert.That(someAabb.ClosestEdgePoint(farAwayPoint), Is.EqualTo(new Vector2(100, 100)));
            Assert.That(someAabb.ClosestEdgePoint(containedCenterPoint), Is.EqualTo(new Vector2(110, 100)));
            Assert.That(someAabb.ClosestEdgePoint(containedPoint), Is.EqualTo(new Vector2(115, 120)));
            Assert.That(someAabb.ClosestEdgePoint(edgePoint), Is.EqualTo(edgePoint));
        }

        [Test]
        public void FromIntersectionTests() {
            var someAabb = new BoundingRectangle(100, 100, 120, 120);
            var intersectingAabb = new BoundingRectangle(90, 90, 110, 110);
            var containedAabb = new BoundingRectangle(110, 110, 115, 115);
            var disjointedAabb = new BoundingRectangle(0, 0, 50, 50);

            Assert.That(BoundingRectangle.FromIntersection(someAabb, intersectingAabb), 
                Is.EqualTo(new BoundingRectangle(100, 100, 110, 110)));
            Assert.That(BoundingRectangle.FromIntersection(someAabb, containedAabb), Is.EqualTo(containedAabb));
            Assert.That(BoundingRectangle.FromIntersection(someAabb, disjointedAabb), 
                Is.EqualTo(BoundingRectangle.Empty));
        }
    }
}