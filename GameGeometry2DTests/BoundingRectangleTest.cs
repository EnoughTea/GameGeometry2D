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
            polygon1 = new BoundingPolygon(rect1.Corners());
            rect0 = new BoundingRectangle(-1, 3.01f, 1, 6);
        }

        private BoundingCircle circle1;
        private BoundingCircle circle2;
        private BoundingCircle circle3;
        private BoundingRectangle rect0;
        private BoundingRectangle rect1;
        private BoundingRectangle rect2;
        private BoundingPolygon polygon1;

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
            Assert.Fail("Not Implimented");
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
            Assert.Fail("Not Implimented");
        }

        [Test]
        public void IntersectsPolygon() {
            Assert.Fail("Not Implimented");
        }

        [Test]
        public void IntersectsRectangle() {
            Assert.Fail("Not Implimented");
        }
    }
}