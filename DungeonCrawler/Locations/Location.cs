using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DungeonCrawler.Locations
{
    public class Location
    {
        public int Id;
        public string Name;
        public string Type;
        public string Description;
        public string[] Connections;
        public int[] Position;
        public Cell[] Floorplan;

        public override string ToString()
        {
            string cells = "";
            foreach (Cell cell in Floorplan)
            {
                cells += string.Format("{0}\n", cell.ToString());
            }

            return string.Format("{0} | {1} | {2} | {3}\n{4}", Id, Name, Type, Description, cells);
        }
    }
}