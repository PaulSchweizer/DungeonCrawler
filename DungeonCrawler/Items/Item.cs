using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Items
{
    public class Item
    {
        public int Id;

        public string Name;

        public string Description;

        public override string ToString()
        {
            return string.Format("{0} - {1} {2}", Id, Name, Description);
        }
    }
}
