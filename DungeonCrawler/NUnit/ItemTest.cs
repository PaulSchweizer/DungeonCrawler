using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using DungeonCrawler.Items;
using DungeonCrawler.Characters;

namespace DungeonCrawler.NUnit
{
    [TestFixture]
    class ItemTest
    {
        [Test]
        public void LoadItems()
        {
            var loader = new ItemLoader();

            var items = loader.LoadItemsFromUrl();

            foreach(Item item in items)
            {
                Console.WriteLine(item);
            }
        }

        [Test]
        public void LoadCharacters()
        {
            var loader = new CharacterLoader();

            var characters = loader.LoadCharactersFromUrl();

            foreach (Character character in characters)
            {
                Console.WriteLine(character);
                foreach (KeyValuePair<string, int> entry in character.Inventory)
                {
                    Console.WriteLine(entry.Key);
                    Console.WriteLine(entry.Value);
                }
            }
        }
    }
}
