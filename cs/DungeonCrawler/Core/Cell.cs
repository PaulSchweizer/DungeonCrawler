using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{
    //{
    //    "Type": "Path",
    //    "Tags": ["dark"],
    //    "Position": [1, 2],
    //    "Enemies": {},
    //    "Items": {},
    //    "IsVisible": false,
    //}
    public class Cell
    {
        public string Type;
        public string[] Tags;
        public int[] Position;
    }
}