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
            Utilities.LoadRulebook();

            // Add some Items
            string json = Utilities.JsonResource("Weapon");
            Weapon weapon = Weapon.DeserializeFromJson(json);
            json = Utilities.JsonResource("Armour");
            Armour armour = Armour.DeserializeFromJson(json);
            Rulebook.Instance.Weapons["Weapon"] = weapon;
            Rulebook.Instance.Armours["Armour"] = armour;

            // Retrieve Items
            weapon = Rulebook.Weapon("Weapon");
            armour = Rulebook.Armour("Armour");
            Assert.AreEqual(3, weapon.Damage);
            Assert.AreEqual(1, armour.Protection);
        }
    }
}
