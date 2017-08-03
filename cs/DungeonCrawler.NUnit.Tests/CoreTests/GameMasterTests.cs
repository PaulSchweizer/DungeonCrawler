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
    }
}