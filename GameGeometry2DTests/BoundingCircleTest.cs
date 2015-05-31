using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace GameGeometry2D.Tests {
    [TestFixture]
    public class BoundingCircleTest {
        [SetUp]
        public void Init() {
            circle1 = new BoundingCircle(Vector2.UnitY, 2);
            circle2 = new BoundingCircle(Vector2.UnitX, 2);
            rect1 = BoundingRectangle.FromCircle(circle1);
            circle3 = BoundingCircle.FromRectangle(rect1);
            rect2 = BoundingRectangle.FromCircle(circle3);
            polygon1 = new BoundingPolygon(rect1.Corners());
        }

        private BoundingCircle circle1;
        private BoundingCircle circle2;
        private BoundingCircle circle3;
        private BoundingRectangle rect1;
        private BoundingRectangle rect2;
        private BoundingPolygon polygon1;

        [Test]
        public void ContainsCircle() {
            Assert.IsTrue(circle1.Contains(circle1) == Containment.Contains, "1");
            Assert.IsFalse(circle1.Contains(circle2) == Containment.Contains, "2");
            Assert.IsTrue(circle3.Contains(circle1) == Containment.Contains, "3");
            Assert.IsFalse(circle1.Contains(circle3) == Containment.Contains, "4");
        }

        [Test]
        public void ContainsPoint() {
            Assert.IsTrue(circle1.Contains(Vector2.UnitY + new Vector2(2, 0)) == Containment.Contains, "1");
            Assert.IsTrue(circle1.Contains(Vector2.UnitY + new Vector2(0, 2)) == Containment.Contains, "2");
            Assert.IsFalse(circle1.Contains(Vector2.UnitY + new Vector2(1, 2)) == Containment.Contains, "3");
            Assert.IsTrue(circle1.Contains(Vector2.UnitY) == Containment.Contains, "4");
        }

        [Test]
        public void ContainsPolygon() {
            Assert.IsTrue(circle3.Contains(polygon1) == Containment.Contains, "1");
            Assert.IsFalse(circle1.Contains(polygon1) == Containment.Contains, "2");
        }

        [Test]
        public void ContainsRectangle() {
            Assert.IsTrue(circle3.Contains(rect1) == Containment.Contains, "1");
            Assert.IsFalse(circle1.Contains(rect1) == Containment.Contains, "2");
        }

        [Test]
        public void IntersectsCircle() {
            Assert.IsTrue(circle1.Intersects(circle1), "1");
            Assert.IsTrue(circle1.Intersects(circle2), "2");
            Assert.IsFalse(circle1.Intersects(new BoundingCircle(0, 6, 3)), "3");
        }

        [Test]
        public void IntersectsRectangle() {
            Assert.IsTrue(circle3.Intersects(rect1), "1");
            Assert.IsTrue(circle2.Intersects(rect1), "2");
            var c = new BoundingCircle(5, 5, 1);
            Assert.IsFalse(c.Intersects(rect2), "1");
            Assert.IsFalse(c.Intersects(rect2), "2");
        }
    }
}