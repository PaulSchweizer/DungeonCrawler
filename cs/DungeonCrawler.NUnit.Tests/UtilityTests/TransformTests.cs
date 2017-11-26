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
            Vector vector = new Vector( 0, 1 );
            Transform transform = new Transform(0, 0, 0);

            Vector mapped = transform.Map(vector.X, vector.Y);

            Assert.AreEqual(vector, mapped);

            transform.Position.X = 10;
            transform.Position.Y = 20;
            mapped = transform.Map(vector.X, vector.Y);
            Assert.AreEqual(10, mapped.X);
            Assert.AreEqual(21, mapped.Y);

            transform.Position.X = 0;
            transform.Position.Y = 0;
            transform.Rotation = (float)(Math.PI);
            mapped = transform.Map(vector);
            Assert.AreEqual(0, mapped.X, 0.0000001);
            Assert.AreEqual(-1, mapped.Y);
        }

        [Test]
        public void Map_Point_with_zero_zero()
        {
            Vector vector = new Vector( 0, 0 );
            Transform transform = new Transform(0, 0, 0);
            Assert.AreEqual(0, transform.Map(vector).X);
            Assert.AreEqual(0, transform.Map(vector).Y);
        }

        [Test]
        public void GridPoints_equality()
        {
            Vector a = new Vector(6, 6);
            Vector b = new Vector(a);
            Vector c = new Vector(new float[] { 12, 6 });
            Assert.AreEqual(a, b);
            Assert.AreNotEqual(a, c);
            Assert.True(a == b);
            Assert.False(a == c);
            Assert.True(a == a);
            Assert.True(a != c);
            Assert.True(a != null);
            Assert.AreNotEqual(a, "some other object");
        }

        [Test]
        public void Initialize_Transform_options()
        {
            Transform transformA = new Transform(new Vector(6, 6), 0.5f);
            Transform transformB = new Transform(transformA);
            Assert.AreEqual(transformA.Position, transformB.Position);
            Assert.AreEqual(transformA.Rotation, transformB.Rotation);
        }

        [Test]
        public void Transforms_as_pretty_strings()
        {
            Console.WriteLine(new Transform(new Vector(6, 6), 0.5f));
        }

        [Test]
        public void RotateVector_expects_radians()
        {
            float[] vector = Transform.RotateVector(1, 0, (float)(Math.PI * 1.5));
            Assert.AreEqual(0, Math.Round(vector[0], 2));
            Assert.AreEqual(-1, vector[1]);
        }

        [Test]
        public void AngleBetween()
        {
            float[] v1 = new float[] { 1, 0 };
            float[] v2 = new float[] { 0, 1 };
            Console.WriteLine(Transform.AngleBetween(v1, v2) * (180 / Math.PI));

            v1 = new float[] { -0.5f, -0.5f };
            v2 = new float[] { 0, 1 };
            Console.WriteLine(Transform.AngleBetween(v1, v2) * (180 / Math.PI));
        }
    }
}
