using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    public class CharacterEquipmentTests
    {
        Character.Character hero;
        Character.Character rat;
        Items.Item weapon;

        [SetUp]
        public void SetUp()
        {
            hero = Utilities.Hero();
            rat = Utilities.Rat();
            weapon = Utilities.Weapon();
            Utilities.InitializeItemDatabase();
        }

        [Test]
        public void Equipped_weapon_affects_skill_value()
        {
            // No weapon equipped yet
            Assert.AreEqual(2, hero.SkillValue(skill: "Combat", tags: new string[] { }));

            // Equipped weapon raises the Combat skill
            hero.Equip(weapon.Name, "RightHand");
            Assert.AreEqual(5, hero.SkillValue(skill: "Combat", tags: new string[] { }));

            // Unequip the weapon 
            hero.UnEquip(weapon.Name);
            Assert.AreEqual(2, hero.SkillValue(skill: "Combat", tags: new string[] { }));
        }
    }
}
