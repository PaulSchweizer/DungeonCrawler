using DungeonCrawler.Core;
using NUnit.Framework;
using System;

namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    public class CharacterXPTests
    {
        Character.Character hero;
        Character.Character rat;

        [SetUp]
        public void SetUp()
        {
            hero = Utilities.Hero();
            rat = Utilities.Rat();
            Utilities.LoadRulebook();
            hero.Inventory.AddItem(Rulebook.Weapon("Weapon"));
            hero.Inventory.AddItem(Rulebook.Armour("Armour"));
        }

        [Test]
        public void Cost_depends_on_various_values()
        {
            Assert.AreEqual(13, hero.Cost);
            Assert.AreEqual(8, rat.Cost);

            hero.Equip(Rulebook.Weapon("Weapon").Identifier, "RightHand");
            hero.Equip(Rulebook.Armour("Armour").Identifier, "Torso");
            Assert.AreEqual(19, hero.Cost);
        }

        [Test]
        public void Reach_next_level()
        {
            Assert.AreEqual(0, hero.SkillPoints);
            hero.ReceiveXP(100);
            Assert.AreEqual(1, hero.SkillPoints);
        }

        [Test]
        public void Learn_new_skills()
        {
            hero.SkillPoints = 1;

            hero.LevelUpSkill("NewSkill");

            Assert.AreEqual(0, hero.Skills["NewSkill"]);
        }

        [Test]
        public void Level_up_skills()
        {
            hero.SkillPoints = 1;
            hero.Skills["MeleeWeapons"] = 0;

            hero.LevelUpSkill("MeleeWeapons");
            Assert.AreEqual(1, hero.Skills["MeleeWeapons"]);

            hero.SkillPoints = 1;
            hero.LevelUpSkill("MeleeWeapons");
            Assert.AreEqual(1, hero.Skills["MeleeWeapons"]);

            hero.SkillPoints = 2;
            hero.LevelUpSkill("MeleeWeapons");
            Assert.AreEqual(2, hero.Skills["MeleeWeapons"]);
        }
    }
}
