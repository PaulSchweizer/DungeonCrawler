using DungeonCrawler.Core;
using NUnit.Framework;
using System;

namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    public class CharacterTransformTests
    {
        Character.Character hero;
        [SetUp]
        public void SetUp()
        {
            Location location = Utilities.Location();
            GameMaster.CurrentLocation = location;
            hero = Utilities.Hero();
        }

        [Test]
        public void Character_registers_on_Cell()
        {
            hero.MoveTo(11, 0);
            Assert.AreEqual(GameMaster.CurrentLocation.CellAt(11, 0), hero.CurrentCell);
        }
    }
}
