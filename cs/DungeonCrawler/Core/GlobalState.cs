using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DungeonCrawler.Core
{
    public class GlobalState
    {
        public Dictionary<string, bool> Conditions;

        #region Singleton

        private static GlobalState _instance;

        public static GlobalState Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GlobalState();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public GlobalState()
        {
            Instance = this;
            Conditions = new Dictionary<string, bool>();
        }

        #endregion 

        #region Serialization

        public static GlobalState DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<GlobalState>(json);
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

        public static bool Get(string condition)
        {
            return Instance.Conditions[condition];
        }

        public static void Set(string condition, bool value)
        {
            Instance.Conditions[condition] = value;
        }
    }
}
