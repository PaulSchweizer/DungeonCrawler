using NUnit.Framework;
using System;
using DungeonCrawler.Character;
using DungeonCrawler.Core;

namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    public class InventoryTests
    {

        Inventory inventory;

        [SetUp]
        public void SetUp()
        {
            inventory = new Inventory();
            Utilities.LoadRulebook();
        }

        [Test]
        public void Add_item_objects()
        {
            // Add a new Weapon
            Assert.AreEqual(0, inventory.Weapons.Count);
            Weapon weapon = Utilities.Weapon();
            inventory.AddItem(weapon);

            Assert.AreEqual(1, inventory.Weapons.Count);
            Assert.AreEqual(1, inventory.Amounts[weapon.Name]);

            // Add another, new Weapon
            weapon = Utilities.Weapon();
            inventory.AddItem(weapon);

            Assert.AreEqual(2, inventory.Weapons.Count);
            Assert.AreEqual(2, inventory.Amounts[weapon.Name]);
        }

        [Test]
        public void Lookup_items_by_name()
        {
            inventory.AddItem(Utilities.Item(), 1);
            Item item = inventory.Item("Item");
            Assert.AreSame(item, Rulebook.Item("Item"));

            Weapon weapon = Utilities.Weapon();
            inventory.AddItem(weapon);
            Assert.AreSame(weapon, inventory.Item("Weapon") as Weapon);
        }

        [Test]
        public void Remove_item()
        {
            Item item = Utilities.Item();
            inventory.AddItem(item, 10);

            inventory.RemoveItem(item, 1);
            Assert.AreEqual(1, inventory.Items.Count);
            Assert.AreEqual(9, inventory.Amounts[item.Name]);

            inventory.RemoveItem(item, 9);
            Assert.AreEqual(0, inventory.Items.Count);
            Assert.IsFalse(inventory.Amounts.ContainsKey(item.Name));

            // Unique items
            Weapon weapon1 = Utilities.Weapon();
            inventory.AddItem(weapon1);
            Weapon weapon2 = Utilities.Weapon();
            inventory.AddItem(weapon2);

            // Specifically only remove Weapon1
            inventory.RemoveItem(weapon1);
            Assert.AreEqual(1, inventory.Weapons.Count);
            Assert.AreSame(inventory.Weapons[0], weapon2);
        }
    }
}
