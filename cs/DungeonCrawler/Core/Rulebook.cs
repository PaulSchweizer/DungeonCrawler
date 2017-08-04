using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DungeonCrawler.Core
{
    public class Rulebook
    {
        public Dictionary<string, Skill> Skills = new Dictionary<string, Skill>();
        public Dictionary<string, string[]> Tags = new Dictionary<string, string[]>();
        public List<Item> Items = new List<Item>();
        public List<Weapon> Weapons = new List<Weapon>();
        public List<Armour> Armours = new List<Armour>();

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

        public static Item Item(string name)
        {
            Item item = Instance.Items.Find(i => i.Name == name);
            if (item != null)
            {
                return item;
            }
            item = Instance.Weapons.Find(i => i.Name == name);
            if (item != null)
            {
                return item;
            }
            item = Instance.Armours.Find(i => i.Name == name);
            if (item != null)
            {
                return item;
            }
            return null;
        }

        //public static Item GetNewItem(string name)
        //{

        //}

        public static Weapon Weapon(string name)
        {
            return Instance.Weapons.Find(i => i.Name == name);
        }

        public static Armour Armour(string name)
        {
            return Instance.Armours.Find(i => i.Name == name);
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
