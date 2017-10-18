using NUnit.Framework;
using DungeonCrawler.Utility;
using System;

namespace DungeonCrawler.NUnit.Tests.UtilityTests
{
    [TestFixture]
    public class TransformTests
    {
        [Test]
        public void Map_GridPoint_to_Transform()
        {
            int[] point = new int[] { 0, 1 };
            Transform transform = new Transform(0, 0, 0);

            int[] mapped = transform.Map(point[0], point[1]);

            Assert.AreEqual(point, mapped);

            transform.Position.X = 10;
            transform.Position.Y = 20;
            mapped = transform.Map(point[0], point[1]);
            Assert.AreEqual(10, mapped[0]);
            Assert.AreEqual(21, mapped[1]);

            transform.Position.X = 0;
            transform.Position.Y = 0;
            transform.Rotation = (float)(Math.PI);
            mapped = transform.Map(point);
            Assert.AreEqual(0, mapped[0]);
            Assert.AreEqual(-1, mapped[1]);
        }

        [Test]
        public void GridPoints_equality()
        {
            GridPoint a = new GridPoint(6, 6);
            GridPoint b = new GridPoint(a);
            GridPoint c = new GridPoint(new int[] { 12, 6 });
            Assert.AreEqual(a, b);
            Assert.AreNotEqual(a, c);
        }

        [Test]
        public void Initialize_Transform_options()
        {
            Transform transformA = new Transform(new GridPoint(6, 6), 0.5f);
            Transform transformB = new Transform(transformA);
            Assert.AreEqual(transformA.Position, transformB.Position);
            Assert.AreEqual(transformA.Rotation, transformB.Rotation);
        }
    }
}
