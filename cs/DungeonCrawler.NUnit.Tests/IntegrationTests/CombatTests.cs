using NUnit.Framework;
using DungeonCrawler.Core;
using System.Collections.Generic;
using System;

namespace DungeonCrawler.NUnit.Tests.IntegrationTests
{
    [TestFixture]
    public class CombatTests
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
            Dice.Die = new NonRandomDie(0);
            hero.Aspects = new List<Aspect>();
            rat.Equipment["RightHand"] = null;
        }

        [Test]
        public void Hero_suffers_damage_from_rat_attack()
        {
            hero.Skills["MeleeWeapons"] = 0;
            rat.Skills["MeleeWeapons"] = 1;

            rat.Attack(hero);
            Assert.AreEqual(1, hero.PhysicalStress.Value);

            rat.Inventory.AddItem(weapon);
            rat.Equip(weapon.Name, "RightHand");

            rat.Attack(hero);
            Assert.AreEqual(1 + 1 + weapon.Damage, hero.PhysicalStress.Value);
        }

        [Test]
        public void Hero_generates_spin_by_defending()
        {
            hero.Skills["MeleeWeapons"] = 5;
            rat.Skills["MeleeWeapons"] = 1;

            rat.Attack(hero);
            Assert.AreEqual(2, hero.Spin);
        }
    }
}
