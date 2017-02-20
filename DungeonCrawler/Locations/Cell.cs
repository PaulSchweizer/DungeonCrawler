using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DungeonCrawler.Locations
{
    public class Cell
    {
        public int[] Position;
        public string Type;
        public string Color;
        public Dictionary<string, object> Contents;

        public override string ToString()
        {
            return string.Format("{0}/{1} | {2} | {3}", Position[0], Position[1], Type, Color);
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (KeyValuePair<string, object> entry in Contents)
            {
                Console.WriteLine(entry.Key);
                Console.WriteLine(entry.Value);
            }
        }
    }
}
