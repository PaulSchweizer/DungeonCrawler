using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DungeonCrawler.Core
{
    public class Rulebook
    {
        public Dictionary<string, Skill> Skills;
        public Dictionary<string, string[]> Tags = new Dictionary<string, string[]>();

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
    }
}
