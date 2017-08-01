using DungeonCrawler.Items;
using NUnit.Framework;
using System;

namespace DungeonCrawler.NUnit.Tests.ItemTests
{
    [TestFixture]
    public class ItemSerializationTests
    {
        [Test]
        public void Deserialize_item_from_json()
        {
            string json = Utilities.JsonResource("Weapon");
            Item deserializedItem = Item.DeserializeFromJson(json);
        }
    }
}
