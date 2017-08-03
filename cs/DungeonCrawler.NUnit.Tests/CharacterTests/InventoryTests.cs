using NUnit.Framework;
using System;
using DungeonCrawler.Character;

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
        }

        [Test]
        public void Add_new_item()
        {
            Assert.AreEqual(0, inventory.Items.Count);
            inventory.AddItem("Sword", 1, 100);
            Assert.AreEqual(1, inventory.Items.Count);
        }

        [Test]
        public void Add_item_that_already_exists()
        {
            inventory.AddItem("Sword", 1, 100);
            inventory.AddItem("Sword", 1, 100);
            Assert.AreEqual(2, inventory.Items["Sword"]["Amount"]);
        }

        [Test]
        public void Lower_quality_is_kept()
        {
            inventory.AddItem("Sword", 1, 100);
            inventory.AddItem("Sword", 1, 50);
            inventory.AddItem("Sword", 1, 100);
            Assert.AreEqual(50, inventory.Items["Sword"]["Quality"]);
        }

        [Test]
        public void Remove_item()
        {
            inventory.AddItem("Sword", 2, 100);
            inventory.RemoveItem("Sword", 1);
            Assert.AreEqual(1, inventory.Items["Sword"]["Amount"]);
            inventory.RemoveItem("Sword", 1);
            Assert.IsFalse(inventory.Items.ContainsKey("Sword"));
        }
    }
}
