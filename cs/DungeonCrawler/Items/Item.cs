using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Items
{
    public class Item
    {
        public string Name;
        public string Type;
        public string Description;
        public string[] Skills;
        public int Bonus;
        public Aspect.Aspect[] Aspects;
        public string[] Tags;
        public string EquipmentSlot;
        public Character.Consequence[] Consequences;

        public static Item DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Item>(json);
        }
    }
}
