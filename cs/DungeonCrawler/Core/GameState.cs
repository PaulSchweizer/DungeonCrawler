using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DungeonCrawler.Core
{
    public class GameState
    {
        public string CurrentLocation;
        public List<Character.Character> PlayerCharacters;

        #region Serialization

        public static GameState DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<GameState>(json);
        }

        public static string SerializeToJson(GameState gameState)
        {
            string json = JsonConvert.SerializeObject(gameState, Formatting.Indented,
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
