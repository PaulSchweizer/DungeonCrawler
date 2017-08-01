using DungeonCrawler.Utilities;
using NUnit.Framework;
using System;

namespace DungeonCrawler.NUnit.Tests.ItemTests
{
    [TestFixture]
    class ItemDatabaseTests
    {
        [SetUp]
        public void SetUp()
        {
            Items.Item weapon = Items.Item.DeserializeFromJson(Utilities.JsonResource("Weapon"));
            Items.ItemDatabase.Instance.Items["Weapon"] = weapon;
        }

        [Test]
        public void Access_item_by_name_in_database()
        {
            Items.Item weapon = Items.ItemDatabase.Item("Weapon");
            Assert.AreEqual("Ratbane", weapon.Name);
        }

        //[Test]
        //public void Find_weapon_by_name_in_database()
        //{
        //    //Weapon weapon = ItemDatabase.Weapon("NameOfWeapon");
        //}

        //[Test]
        //public void Add_item_to_database()
        //{
        //    Assert.IsTrue(false);
        //}

        //[Test]
        //public void Deserialize_item_database()
        //{
        //    Assert.IsTrue(false);
        //}

    }
}
