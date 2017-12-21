using NUnit.Framework;
using DungeonCrawler.Core;
using System;


namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class LocationTests
    {

        Location location;

        [SetUp]
        public void SetUp()
        {
            string json = Utilities.JsonResource("GameData.Locations.Location");
            location = Location.DeserializeFromJson(json);
        }

        [Test]
        public void LocationAddsTagsToCells()
        {
            Assert.AreEqual(new string[] { "dark", "forest" }, location.Tags);
            foreach (Cell cell in location.Cells)
            {
                Assert.AreEqual(2, cell.Tags.Length);
                Assert.IsTrue(Array.Exists(cell.Tags, element => element == "dark"));
                Assert.IsTrue(Array.Exists(cell.Tags, element => element == "forest"));
            }
        }

        [Test]
        public void BBox_from_top_left_to_bottom_right()
        {
            //      [ 2 ]
            //      [ 1 ]
            // [-1 ][ 0 ][ 1 ][ 2 ]
            //      [-1 ]
            // Dimensions: [-1, 2, 2, -1]
            //
            Assert.AreEqual(-1, location.BBox[0]);
            Assert.AreEqual(2, location.BBox[1]);
            Assert.AreEqual(2, location.BBox[2]);
            Assert.AreEqual(-1, location.BBox[3]);
        }

        [Test]
        public void Location_visualized_as_text()
        {
            Console.WriteLine(location.ToString());
        }
    }
}
