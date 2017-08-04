using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DungeonCrawler.Core;

namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    public class CharacterEquipmentTests
    {
        Character.Character hero;
        Character.Character rat;
        Weapon weapon;
        Armour armour;

        [SetUp]
        public void SetUp()
        {
            hero = Utilities.Hero();
            rat = Utilities.Rat();
            weapon = Utilities.Weapon();
            armour = Utilities.Armour();
            Utilities.LoadRulebook();
        }

        [Test]
        public void Aspects_of_equipment_are_taken_into_account()
        {
            // No weapon equipped yet
            Assert.AreEqual(3, hero.SkillValue(skill: "MeleeWeapons", tags: new string[] { "rat" }));

            // Equipped weapon raises the MeleeWeapons skill
            hero.Equip(weapon.Name, "RightHand");
            Assert.AreEqual(4, hero.SkillValue(skill: "MeleeWeapons", tags: new string[] { "rat" }));

            // Equipped armour raises the MeleeWeapons skill as well
            hero.Equip(armour.Name, "Torso");
            Assert.AreEqual(5, hero.SkillValue(skill: "MeleeWeapons", tags: new string[] { "rat" }));

            // Unequip the weapon 
            hero.UnEquip(weapon.Name);
            Assert.AreEqual(4, hero.SkillValue(skill: "MeleeWeapons", tags: new string[] { "rat" }));
        }

        [Test]
        public void Equipped_armour_adds_to_consequences()
        {
            // No armor equipped yet
            Assert.AreEqual(2, hero.AllConsequences.Count);

            // Equipped armour increases the consequences
            hero.Equip(armour.Name, "Torso");
            Assert.AreEqual(3, hero.AllConsequences.Count);

            // Unequip the armour 
            hero.UnEquip(armour.Name);
            Assert.AreEqual(2, hero.AllConsequences.Count);
        }

        [Test]
        public void Consequence_order_puts_armour_first()
        {
            hero.Equip(armour.Name, "Torso");
            Assert.AreEqual(3, hero.AllConsequences.Count);
            Assert.AreSame(hero.AllConsequences[0], Rulebook.Armour(armour.Name).Consequences[0]);
        }
    }
}
