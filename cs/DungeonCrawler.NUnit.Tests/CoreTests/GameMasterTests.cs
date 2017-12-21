using NUnit.Framework;
using DungeonCrawler.Core;
using DungeonCrawler.Utility;
using System.Collections.Generic;

namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class GameMasterTests
    {

        Character.Character characterA = new Character.Character();
        Character.Character characterB = new Character.Character();

        [SetUp]
        public void SetUp()
        {
            GameMaster.Characters = new List<Character.Character>();
            GameMaster.RegisterCharacter(characterA);
            GameMaster.RegisterCharacter(characterB);
            characterA.Transform.Position = new Vector(0, 0);
            characterB.Transform.Position = new Vector(0, 0);
        }

        [Test]
        public void Current_tags_depend_on_current_cell()
        {
            Cell cell = new Cell();
            cell.Tags = new string[] { "dark", "cavern" };
            GameMaster.CurrentCell = cell;
            Assert.AreEqual(2, GameMaster.CurrentTags.Length);
            Assert.Contains("dark", GameMaster.CurrentTags);
            Assert.Contains("cavern", GameMaster.CurrentTags);
        }

        [Test]
        public void Characters_can_only_be_registered_once()
        {
            GameMaster.Characters = new List<Character.Character>();
            Assert.AreEqual(0, GameMaster.Characters.Count);
            Character.Character character = new Character.Character();
            GameMaster.RegisterCharacter(character);
            GameMaster.RegisterCharacter(character);
            Assert.AreEqual(1, GameMaster.Characters.Count);
        }

        [Test]
        public void Only_registered_Characters_Can_be_removed()
        {
            GameMaster.Characters = new List<Character.Character>();
            Assert.AreEqual(0, GameMaster.Characters.Count);
            GameMaster.RegisterCharacter(characterA);
            Assert.AreEqual(1, GameMaster.Characters.Count);
            GameMaster.DeRegisterCharacter(characterB);
            Assert.AreEqual(1, GameMaster.Characters.Count);
            GameMaster.DeRegisterCharacter(characterA);
            Assert.AreEqual(0, GameMaster.Characters.Count);
        }
    }
}