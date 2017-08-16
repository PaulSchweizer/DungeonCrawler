using NUnit.Framework;
using DungeonCrawler.Core;
using System.Collections.Generic;

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
        }

        [Test]
        public void Hero_suffers_damage_from_rat_attack()
        {
            Dice.Die = new NonRandomDie(0);
            hero.Skills["MeleeWeapons"] = 0;
            hero.Aspects = new List<Aspect>();
            rat.Skills["MeleeWeapons"] = 1;

            rat.Attack(hero);

            Assert.AreEqual(1, hero.PhysicalStress.Value);
        }
    }
}
