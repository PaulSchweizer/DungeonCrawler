using NUnit.Framework;
using DungeonCrawler.Core;
using System;

namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class ItemTests
    {
        [Test]
        public void Deserialize_items_to_the_correct_type()
        {
            string json = Utilities.JsonResource("GameData.Items.Weapons.Weapon");
            Weapon weapon = Weapon.DeserializeFromJson(json);
            json = Utilities.JsonResource("GameData.Items.Armour.Armour");
            Armour armour = Armour.DeserializeFromJson(json);
        }

        [Test]
        public void Item_cost_depends_on_its_parameters()
        {
            Weapon weapon = Utilities.Weapon();
            Assert.AreEqual(7, weapon.Cost);

            Armour armour = Utilities.Armour();
            Console.WriteLine(armour.Cost);
            Assert.AreEqual(8, armour.Cost);
        }

        [Test]
        public void Ids_are_assigned_if_not_given()
        {
            Item item = Utilities.Item();
            Assert.AreEqual("0", item.Id);

            Weapon weapon = Utilities.Weapon();
            Assert.IsNotNull(item.Id);
        }
    }
}
