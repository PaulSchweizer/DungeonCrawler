using DungeonCrawler.Character;
using DungeonCrawler.Core;
using NUnit.Framework;
using System;

namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    public class AttackMarkerTests
    {
        [Test]
        public void Progress_of_Attack()
        {
            Character.Character attacker = new Character.Character();
            AttackMarker marker = new AttackMarker(attacker);
            marker.Start(new int[][] { new int[] { 0, 1 } }, "MeleeWeapons", 1, 2);
            Assert.AreEqual(0f, marker.Progress());

            marker.CurrentTime = 0.5f;
            Assert.AreEqual(0.25f, marker.Progress());

            marker.CurrentTime = 1f;
            Assert.AreEqual(0.5f, marker.Progress());

            marker.CurrentTime = 1.5f;
            Assert.AreEqual(0.625f, marker.Progress());

            marker.CurrentTime = 2f;
            Assert.AreEqual(0.75f, marker.Progress());

            marker.CurrentTime = 2.5f;
            Assert.AreEqual(0.875f, marker.Progress());

            marker.CurrentTime = 3f;
            Assert.AreEqual(1f, marker.Progress());
        }

        [Test]
        public void TotalTime_consists_of_Pre_and_PostTime()
        {
            Character.Character attacker = new Character.Character();
            AttackMarker marker = new AttackMarker(attacker);
            marker.PreTime = 3;
            marker.PostTime = 3;
            Assert.AreEqual(6, marker.TotalTime);
        }

        [Test]
        public void Stop_to_reset_AttackMarker()
        {
            Character.Character attacker = new Character.Character();
            AttackMarker marker = new AttackMarker(attacker);
            marker.Start(new int[][] { new int[] { 0, 1 } }, "MeleeWeapons", 1, 2);
            marker.CurrentTime = 1f;
            marker.Stop();
            Assert.AreEqual(0, marker.CurrentTime);
            Assert.IsFalse(marker.IsActive);
        }

        [Test]
        public void Hit_applies_attack_to_Enemies_on_AttackShape()
        {
            Dice.Die = new NonRandomDie(0);
            Character.Character hero = Utilities.Hero();
            Character.Character rat = Utilities.Rat();
            Utilities.LoadRulebook();
            GameMaster.CurrentLocation = Utilities.Location();
            GameMaster.RegisterCharacter(hero);
            GameMaster.RegisterCharacter(rat);
            hero.MoveTo(0, 0);
            rat.MoveTo(0, 1);

            AttackMarker marker = new AttackMarker(hero);
            marker.Start(new int[][] { new int[] { 0, 1 } }, "MeleeWeapons", 1, 2);
            marker.Hit();
            Assert.AreEqual(2, rat.PhysicalStress.Value);
            Assert.IsTrue(marker.HitOccurred);
        }
    }
}
