using NUnit.Framework;
using DungeonCrawler.Core;
using System;

namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class RulebookTests
    {
        [Test]
        public void DeserializeRulebook()
        {
            string json = Utilities.JsonResource("Rulebook");
            Rulebook.DeserializeFromJson(json);
            Console.WriteLine(Rulebook.SerializeToJson());
        }

        [Test]
        public void Lookup_Items_in_the_Rulebook()
        {
            // Get the Rulebook
            string json = Utilities.JsonResource("Rulebook");
            Rulebook.DeserializeFromJson(json);

            // Add some Items
            json = Utilities.JsonResource("Weapon");
            Weapon weapon = Weapon.DeserializeFromJson(json);
            json = Utilities.JsonResource("Armour");
            Armour armour = Armour.DeserializeFromJson(json);
            Rulebook.Instance.Items["Weapon"] = weapon;
            Rulebook.Instance.Items["Armour"] = armour;

            // Retrieve Items
            weapon = (Weapon)Rulebook.Item("Weapon");
            armour = (Armour)Rulebook.Item("Armour");
            Assert.AreEqual(3, weapon.Damage);
            Assert.AreEqual(1, armour.Protection);
        }
    }
}
