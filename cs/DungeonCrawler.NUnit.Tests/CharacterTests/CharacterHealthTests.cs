using DungeonCrawler.Character;
using DungeonCrawler.Core;
using NUnit.Framework;
using System;

namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    public class CharacterHealthTests
    {
        Character.Character hero;
        Character.Character rat;
        Armour armour;

        [SetUp]
        public void SetUp()
        {
            hero = Utilities.Hero();
            rat = Utilities.Rat();
            armour = Utilities.Armour();
            Utilities.LoadRulebook();
            Utilities.SetupTestDice();
            Dice.Die = new NonRandomDie(0);
        }

        [Test]
        public void Unarmored_hero_receives_damage()
        {
            // 4 Damage only adds to physical stress, hero can take 5 stress
            hero.ReceiveDamage(4);
            Assert.AreEqual(4, hero.PhysicalStress.Value);
            foreach(Consequence consequence in hero.AllConsequences)
            {
                Assert.IsFalse(consequence.IsTaken);
            }

            // Another 2 damage will trigger the first Consequence to be taken
            hero.ReceiveDamage(2);
            Assert.AreEqual(4, hero.PhysicalStress.Value);
            Assert.IsTrue(hero.AllConsequences[0].IsTaken);

            // Another 2 damage will trigger the second Consequence to be taken
            hero.ReceiveDamage(2);
            Assert.AreEqual(4, hero.PhysicalStress.Value);
            Assert.IsTrue(hero.AllConsequences[0].IsTaken);
            Assert.IsTrue(hero.AllConsequences[1].IsTaken);

            // Another 2 damage will take out the character
            hero.ReceiveDamage(2);
            Assert.AreEqual(4, hero.PhysicalStress.Value);
            Assert.IsTrue(hero.AllConsequences[0].IsTaken);
            Assert.IsTrue(hero.AllConsequences[1].IsTaken);
            Assert.IsTrue(hero.IsTakenOut);
        }

        [Test]
        public void Armored_hero_receives_damage()
        {
            hero.Inventory.AddItem(armour);
            hero.Equip(armour.Identifier, "Torso");

            // 0 Damage will NOT result in a stress value of -1
            hero.ReceiveDamage(0);
            Assert.AreEqual(hero.PhysicalStress.Value, 0);

            // 4 Damage only adds to physical stress, hero can take 5 stress
            hero.ReceiveDamage(4);
            Assert.AreEqual(4 - armour.Protection, hero.PhysicalStress.Value);
            foreach (Consequence consequence in hero.AllConsequences)
            {
                Assert.IsFalse(consequence.IsTaken);
            }
        }

        [Test]
        public void Hero_heals_consequence()
        {
            hero.Skills["Healing"] = 1;
            hero.Consequences[0].Take();
            hero.Consequences[1].Take();

            // Not enough skill value to heal minor consequence of 2
            hero.Heal(hero, hero.Consequences[0]);
            Assert.IsTrue(hero.Consequences[0].IsTaken);

            Dice.Die = new NonRandomDie(1);
            hero.Heal(hero, hero.Consequences[0]);
            Assert.IsFalse(hero.Consequences[0].IsTaken);

            Dice.Die = new NonRandomDie(3);
            hero.Heal(hero, hero.Consequences[1]);
            Assert.IsFalse(hero.Consequences[1].IsTaken);

            Console.WriteLine(GameEventsLogger.Next);
        }
    }
}
