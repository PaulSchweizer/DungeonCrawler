using NUnit.Framework;
using DungeonCrawler.Core;

namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class GameMasterTests
    {
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
            Assert.AreEqual(0, GameMaster.Characters.Count);
            Character.Character character = new Character.Character(); 
            GameMaster.RegisterCharacter(character);
            GameMaster.RegisterCharacter(character);
            Assert.AreEqual(1, GameMaster.Characters.Count);
        }
    }
}