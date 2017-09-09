using NUnit.Framework;
using DungeonCrawler.Core;
using System;


namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class LocationTests
    {
        [Test]
        public void LocationAddsTagsToCells()
        {
            string json = Utilities.JsonResource("Location");
            Location location = Location.DeserializeFromJson(json);

            Assert.AreEqual(new string[] { "dark", "forest" }, location.Tags);

            foreach (Cell cell in location.Floorplan)
            {
                Assert.AreEqual(2, cell.Tags.Length);
                Assert.IsTrue(Array.Exists(cell.Tags, element => element == "dark"));
                Assert.IsTrue(Array.Exists(cell.Tags, element => element == "forest"));
            }
        }
    }
}
