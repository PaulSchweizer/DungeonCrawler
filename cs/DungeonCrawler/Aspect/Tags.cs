using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DungeonCrawler.Aspect
{
    public class TagsTable
    {
        public string[] Tags;
        public Dictionary<string, string[]> Synonyms;
        public Dictionary<string, string[]> Opposites;
        public static TagsTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TagsTable();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static TagsTable _instance;

        public static string[] SynonymsOf(string tag)
        {
            tag = tag.ToLower();
            if (Instance.Synonyms.ContainsKey(tag))
            {
                return Instance.Synonyms[tag];
            }
            return new string[] { };
        }

        public static string[] OppositesOf(string tag)
        {
            tag = tag.ToLower();
            if (Instance.Opposites.ContainsKey(tag))
            {
                return Instance.Opposites[tag];
            }
            return new string[] { };
        }

        public static void DeserializeFromJson(string json)
        {
            Instance = JsonConvert.DeserializeObject<TagsTable>(json);
        }

        public static string SerializeToJson()
        {
            string json = JsonConvert.SerializeObject(Instance, Formatting.Indented);

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
