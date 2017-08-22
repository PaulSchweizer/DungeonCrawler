using NUnit.Framework;
using DungeonCrawler.Core;
using System;

namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class GameEventsLoggerTest
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

            hero.Inventory.AddItem(weapon);
            hero.Inventory.AddItem(armour);

            Utilities.LoadRulebook();
        }

        [Test]
        public void Log_only_serves_not_yet_served_parts()
        {
            GameEventsLogger.LogAttack(hero, rat, "MeleeWeapons", 2, hero.Skills["MeleeWeapons"], 0);

            Assert.AreNotEqual("", GameEventsLogger.Next);
            Assert.AreEqual("", GameEventsLogger.Next);

            GameEventsLogger.LogAttack(rat, hero, "MeleeWeapons", 1, hero.Skills["MeleeWeapons"], 0);
            GameEventsLogger.LogAttack(rat, hero, "MeleeWeapons", 1, hero.Skills["MeleeWeapons"], 0);

            Assert.AreEqual(2, GameEventsLogger.Next.Split('\n').Length);

            GameEventsLogger.LogAttack(rat, hero, "MeleeWeapons", 1, hero.Skills["MeleeWeapons"], 0);

            GameEventsLogger.Reset();
            Assert.AreEqual("", GameEventsLogger.Next);
        }

    }
}
