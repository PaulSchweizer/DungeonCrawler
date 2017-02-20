using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{
    public class GameData
    {
        public int Id;
        public string Name;
        public string Description;
        public string Url;

        public override string ToString()
        {
            return string.Format("{0} | {1} | {2} | {3}", Id, Name, Description, Url);
        }
    }
}
