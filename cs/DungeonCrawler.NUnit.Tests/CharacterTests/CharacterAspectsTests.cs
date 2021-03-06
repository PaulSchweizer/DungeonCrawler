﻿using DungeonCrawler.Core;
using NUnit.Framework;
using System;

namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    public class CharacterAspectsTests
    {
        Character.Character hero;
        Character.Character rat;

        [SetUp]
        public void SetUp()
        {
            hero = Utilities.Hero();
            hero.Inventory.AddItem(Utilities.Weapon());
            hero.Inventory.AddItem(Utilities.Armour());
            rat = Utilities.Rat();
            Utilities.LoadRulebook();
            Console.WriteLine(Rulebook.SerializeToJson());
        }

        [Test]
        public void All_aspects_affecting_Skill()
        {
            Assert.IsNotEmpty(hero.AspectsAffectingSkill("MeleeWeapons"));
            Assert.IsEmpty(hero.AspectsAffectingSkill("DoesNotExist"));
        }

        [Test]
        public void Aspects_change_skill_values()
        {
            int combatValue = hero.SkillValue("MeleeWeapons", new string[] { "Rat" });
            Assert.AreEqual(3, combatValue);
        }

        [Test]
        public void AllAspects_contains_Equipment()
        {
            Assert.AreEqual(1, hero.AllAspects.Count);
            hero.Equip(hero.Inventory.Weapons[0].Identifier, "RightHand");
            hero.Equip(hero.Inventory.Armour[0].Identifier, "Torso");
            Assert.AreEqual(3, hero.AllAspects.Count);
        }

        [Test]
        public void AllAspects_contains_taken_Consequences()
        {
            Assert.AreEqual(1, hero.AllAspects.Count);
            hero.Consequences[0].Take();
            Assert.AreEqual(2, hero.AllAspects.Count);
        }
    }
}
