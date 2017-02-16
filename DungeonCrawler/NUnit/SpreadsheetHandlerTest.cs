using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DungeonCrawler.Core;

namespace DungeonCrawler.NUnit
{
    [TestFixture]
    class SpreadsheetHandlerTest
    {
        [Test]
        public void LoadRegisteredGames()
        {
            var data = SpreadsheetHandler.GetRegisteredGames();
            Console.WriteLine(data.Tables["Table"].Rows[0]["Name"]);
        }
    }
}
