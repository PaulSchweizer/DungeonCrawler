using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Items
{
    public class ItemDatabase
    {
        public Dictionary<string, Item> Items;

        public static ItemDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ItemDatabase();
                    _instance.Items = new Dictionary<string, Item>();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static ItemDatabase _instance;

        public static Item Item(string itemName)
        {
            if (Instance.Items.ContainsKey(itemName))
            {
                return null;
            }
            else
            {
                return Instance.Items[itemName];
            }
        }

        public static void DeserializeFromJson(string json)
        {
            Instance = JsonConvert.DeserializeObject<ItemDatabase>(json);
        }
    }
}
