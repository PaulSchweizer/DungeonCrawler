using NUnit.Framework;
using System;

namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    class CharacterSerializationTests
    {
        [Test]
        public void Serialize_deserialize_Character()
        {
            string json = Utilities.JsonResource("GameData.PCs.Hero");

            Character.Character deserializedChar = Character.Character.DeserializeFromJson(json);
            string deserialized = Character.Character.SerializeToJson(deserializedChar);

            Console.WriteLine(json);
            Console.WriteLine(deserialized);

            Assert.AreEqual(json.Replace("\n", "").Replace("\r", ""), deserialized.Replace("\n", "").Replace("\r", ""));
        }

        [Test]
        public void Deserialized_Items_are_replaced_with_objects_from_Rulebook_if_possible()
        {
            //Utilities.LoadRulebook();
            Character.Character hero = Utilities.Hero();
            Character.Character rat = Utilities.Rat();

            Assert.AreEqual(1, hero.Inventory.Items.Count);

            hero.Inventory += rat.Inventory;

            Assert.AreEqual(1, hero.Inventory.Items.Count);

        }
    }
}
