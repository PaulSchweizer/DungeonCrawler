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
        public string Destination;
        public Dictionary<string, string> Items;
        public Dictionary<string, string> Enemies;

        public override string ToString()
        {
            return string.Format("{0}/{1} | {2} | {3}", Position[0], Position[1], Type, Color);
        }

        [OnDeserialized]
        private void OnDeserializedMethod(StreamingContext context)
        {
            //foreach (KeyValuePair<string, string> entry in Items)
            //{
            //    Console.WriteLine(entry.Key);
            //    Console.WriteLine(entry.Value);
            //}
            //foreach (KeyValuePair<string, string> entry in Enemies)
            //{
            //    Console.WriteLine(entry.Key);
            //    Console.WriteLine(entry.Value);
            //}
        }
    }
}
