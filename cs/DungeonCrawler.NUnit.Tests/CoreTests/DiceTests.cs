using NUnit.Framework;
using DungeonCrawler.Core;
using System;
using System.Collections.Generic;

namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class DiceTests
    {
        [Test]
        public void Dice_roll_maps_to_table()
        {
            Dice.Die = new NonRandomDie(1);
            Dice.Distribution = new Dictionary<int, int>() { { 0, 0 }, { 1, 10 }};
            Assert.AreEqual(10 , Dice.Roll());
        }

        [Test]
        public void Dice_roll_bigger_than_possible_results_returns_0()
        {
            Dice.Die = new NonRandomDie(2);
            Dice.Distribution = new Dictionary<int, int>() { { 0, 0 }, { 1, 1 } };
            Assert.AreEqual(0, Dice.Roll());
        }
    }
}
