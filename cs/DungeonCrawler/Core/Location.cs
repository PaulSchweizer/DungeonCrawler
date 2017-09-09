
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DungeonCrawler.Core
{
    public class Location
    {
//        {
//	        "Name": "Forest",
//	        "Description": "A #dark #forest.",
//          "Tags": ["dark", "forest"]
//	        "Floorplan": [
//		        {
//			        "Position": [
//				        0,
//				        0
//			        ],
//			        "Type": "Wall",
//			        "Tags": [
//				        "dark"
//			        ]
//              }
//	        ]
//      }
        public string Name;
        public string Description;
        public string[] Tags;
        public Cell[] Floorplan;

        public static Location DeserializeFromJson(string json)
        {
            Location location = JsonConvert.DeserializeObject<Location>(json);
            foreach (Cell cell in location.Floorplan)
            {
                List<string> tags = new List<string>();
                tags.AddRange(cell.Tags);
                foreach (string tag in location.Tags)
                {
                    if (!tags.Contains(tag))
                    {
                        tags.Add(tag);
                    }
                }
                cell.Tags = tags.ToArray();
            }
            return location;
        }
    }
}