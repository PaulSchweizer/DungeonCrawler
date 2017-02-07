using System;
using System.Collections.Generic;
using System.Text;
using DungeonCrawler.Items;

namespace DungeonCrawler.Characters
{
    public class Character
    {

        public int Id;

        public string Name;

        public string Type;

        public Dictionary<string, int> Inventory;

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Id, Name, Type);
        }

    }

}
