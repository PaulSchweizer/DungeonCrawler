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
            Assert.AreEqual(1, inventory.Amount(weapon.Identifier));

            // Add another, new Weapon
            weapon = Utilities.Weapon();
            inventory.AddItem(weapon);

            Assert.AreEqual(2, inventory.Weapons.Count);
            Assert.AreEqual(1, inventory.Amount(weapon.Identifier));
        }

        [Test]
        public void Lookup_items_by_name()
        {
            inventory.AddItem(Utilities.Item(), 1);
            Item item = inventory.Item("Item");
            Assert.AreSame(item, Rulebook.Item("Item"));

            Weapon weapon = Utilities.Weapon();
            inventory.AddItem(weapon);
            Assert.AreSame(weapon, inventory.Item(weapon.Identifier) as Weapon);
        }

        [Test]
        public void Remove_item()
        {
            Item item = Utilities.Item();
            inventory.AddItem(item, 10);

            inventory.RemoveItem(item, 1);
            Assert.AreEqual(1, inventory.Items.Count);
            Assert.AreEqual(9, inventory.Amount(item.Name));

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

        [Test]
        public void Move_Inventory_to_another_inventory_by_adding_them()
        {
            Inventory thisInventory = new Inventory();
            Inventory thatInventory = new Inventory();

            thisInventory.AddItem(Rulebook.Item("Item"), 1);

            thatInventory.AddItem(Rulebook.Item("Item"), 1);
            thatInventory.AddItem(Rulebook.Item("Weapon"), 2);
            thatInventory.AddItem(Rulebook.Item("Armour"), 3);

            thisInventory += thatInventory;

            Assert.AreEqual(2, thisInventory.Amount("Item"));
            Assert.AreEqual(1, thisInventory.Weapons.Count);
            Assert.AreEqual(1, thisInventory.Armour.Count);
        }
    }
}
