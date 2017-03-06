using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DungeonCrawler.Core;
using DungeonCrawler.Items;
using System.Data;
using DungeonCrawler.Locations;

namespace DungeonCrawler.NUnit
{
    [TestFixture]
    class SpreadsheetHandlerTest
    {
        [Test]
        public void GetRegisteredGames()
        {
            var data = SpreadsheetHandler.GetRegisteredGames();
            foreach (GameData game in data)
            {
                Console.WriteLine(game);
            }
        }

        [Test]
        public void GetItems()
        {
            var items = SpreadsheetHandler.GetItems();
            foreach(Item item in items)
            {
               Console.WriteLine(item);            
            }
        }

        [Test]
        public void GetLocations()
        {
            var locations = SpreadsheetHandler.GetLocations();
            foreach(Location location in locations)
            {
                Console.WriteLine(location);
            }
        }
    }
}
