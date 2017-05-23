using DungeonCrawler.Utilities;
using NUnit.Framework;
using System;

namespace DungeonCrawler.NUnit.Tests.ConflictsTests
{
    [TestFixture]
    public class CombatTests
    {

        Character.Character hero;
        Character.Character rat;

        [SetUp]
        public void SetUp()
        {
            hero = Character.Character.DeserializeFromJson(Utilities.JsonResource("Hero"));
            rat = Character.Character.DeserializeFromJson(Utilities.JsonResource("Rat"));
            Dice.Die = new NonRandomDie();
        }

        [Test]
        public void Hero_attacks_Rat()
        {
            int attackValue = Actions.AttackAction.AttackValue(
                attacker: hero, defender: rat,
                skill: "Combat", tags: new string[] { });
            int defendValue = Actions.AttackAction.DefendValue(
                attacker: hero, defender: rat,
                skill: "Combat", tags: new string[] { });
            Assert.AreEqual(1, defendValue);

            // Attack the Rat, the rat takes 2 PhysicalStress
            int shifts = Actions.AttackAction.Attack(
                attacker: hero, defender: rat,
                skill: "Combat", tags: new string[] { });

            Assert.AreEqual(2, shifts);
            Assert.AreEqual(2, rat.PhysicalStress.Value);

            // A second attack adds 2 more PhysicalStress
            Actions.AttackAction.Attack(
                attacker: hero, defender: rat,
                skill: "Combat", tags: new string[] { });
            Assert.AreEqual(4, rat.PhysicalStress.Value);

            // Too much stress taken, the Rat will take a Consequence now
            Actions.AttackAction.Attack(
                attacker: hero, defender: rat,
                skill: "Combat", tags: new string[] { });
            Assert.AreEqual(4, rat.PhysicalStress.Value);
            Assert.IsTrue(rat.Consequences[0].IsTaken);
            Assert.IsFalse(rat.IsTakenOut);

            //The next hit will take the Rat out
            Actions.AttackAction.Attack(
                attacker: hero, defender: rat,
                skill: "Combat", tags: new string[] { });
            Assert.IsTrue(rat.IsTakenOut);
        }
    }
}
