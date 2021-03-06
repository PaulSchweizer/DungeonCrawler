﻿using NUnit.Framework;
using DungeonCrawler.Core;
using System;

namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class RulebookTests
    {
        [Test]
        public void Rulebook_initializes_itself()
        {
            Rulebook.Instance = null;
            Assert.IsNotNull(Rulebook.Instance);
        }

        [Test]
        public void DeserializeRulebook()
        {
            string json = Utilities.JsonResource("GameData.Rulebook");
            Rulebook.DeserializeFromJson(json);
            Console.WriteLine(Rulebook.SerializeToJson());
        }

        [Test]
        public void Lookup_Items_in_the_Rulebook()
        {
            // Get the Rulebook
            Utilities.LoadRulebook();

            // Add some Items
            string json = Utilities.JsonResource("GameData.Items.Weapons.Weapon");
            Weapon weapon = Weapon.DeserializeFromJson(json);
            json = Utilities.JsonResource("GameData.Items.Armour.Armour");
            Armour armour = Armour.DeserializeFromJson(json);
            Rulebook.Instance.Weapons.Add(weapon);
            Rulebook.Instance.Armours.Add(armour);

            // Retrieve Items
            weapon = Rulebook.Weapon("Weapon");
            armour = Rulebook.Armour("Armour");
            Assert.AreEqual(3, weapon.Damage);
            Assert.AreEqual(1, armour.Protection);

            // Item does not exist
            Item item = Rulebook.Item("DoesNotExist");
            Assert.IsNull(item);
        }

        [Test]
        public void Define_Celltypes_in_Rulebook()
        {
            Utilities.LoadRulebook();

            string cellType = "Forest";
            CellBlueprint cellBlueprint = Rulebook.Instance.CellBlueprints[cellType];
            Console.WriteLine(cellBlueprint.Type);
        }
    }
}
