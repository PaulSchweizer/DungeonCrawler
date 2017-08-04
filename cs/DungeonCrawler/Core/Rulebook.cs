using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DungeonCrawler.Core
{
    public class Rulebook
    {
        public Dictionary<string, Skill> Skills = new Dictionary<string, Skill>();
        public Dictionary<string, string[]> Tags = new Dictionary<string, string[]>();
        public Dictionary<string, Item> Items = new Dictionary<string, Item>();
        public Dictionary<string, Weapon> Weapons = new Dictionary<string, Weapon>();
        public Dictionary<string, Armour> Armours = new Dictionary<string, Armour>();

        #region Singleton

        private static Rulebook _instance;

        public static Rulebook Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Rulebook();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        #endregion 

        #region Tags

        public static string[] SynonymsOf(string tag)
        {
            tag = tag.ToLower();
            if (Instance.Tags.ContainsKey(tag))
            {
                return Instance.Tags[tag];
            }
            else
            {
                foreach(KeyValuePair<string, string[]> entry in Instance.Tags)
                {
                    foreach(string value in entry.Value)
                    {
                        if (value == tag)
                        {
                            return new string[] {entry.Key};
                        }
                    }
                }
            }
            return new string[] { };
        }

        #endregion

        #region Items

        public static Item Item(string itemName)
        {
            if (Instance.Items.ContainsKey(itemName))
            {
                return Instance.Items[itemName];
            }
            else if (Instance.Weapons.ContainsKey(itemName))
            {
                return Instance.Weapons[itemName];
            }
            else if (Instance.Armours.ContainsKey(itemName))
            {
                return Instance.Armours[itemName];
            }
            else
            {
                return null;
            }
        }

        public static Weapon Weapon(string itemName)
        {
            if (!Instance.Weapons.ContainsKey(itemName))
            {
                return null;
            }
            else
            {
                return Instance.Weapons[itemName];
            }
        }

        public static Armour Armour(string itemName)
        {
            if (!Instance.Armours.ContainsKey(itemName))
            {
                return null;
            }
            else
            {
                return Instance.Armours[itemName];
            }
        }

        #endregion 

        #region Serialization

        public static void DeserializeFromJson(string json)
        {
            Instance = JsonConvert.DeserializeObject<Rulebook>(json);
        }

        public static string SerializeToJson()
        {
            string json = JsonConvert.SerializeObject(Instance, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.Indentation = 4;
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }

        #endregion
    }
}
