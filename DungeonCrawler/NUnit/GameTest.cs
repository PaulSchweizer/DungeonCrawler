using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DungeonCrawler.Core;
using DungeonCrawler.Items;
using System.Data;

namespace DungeonCrawler.NUnit
{
    [TestFixture]
    class GameTest
    {
        [Test]
        public void InitializeNewGame()
        {
            var game = new Game();
            game.InitializeNewGame();
        }
    }
}
