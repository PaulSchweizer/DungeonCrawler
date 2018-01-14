using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DungeonCrawler.Core
{
    public class GameState
    {
        public string Location;
        public List<string> PlayerCharacters;
        public string GlobalState;

        public GameState()
        {
            PlayerCharacters = new List<string>();
        }

        #region Save and Load

        public static void Save(string name)
        {
            string rootPath = Path.Combine(GameMaster.RootDataPath, name);
            Directory.CreateDirectory(rootPath);
            Directory.CreateDirectory(Path.Combine(rootPath, "PCs"));

            GameState gameState = new GameState();
            gameState.Location = GameMaster.CurrentLocation.Name;
            foreach (Character.Character character in GameMaster.CharactersOfType(new string[] { "Player" }))
            {
                gameState.PlayerCharacters.Add(Path.Combine("PCs", character.Name));
                File.WriteAllText(Path.Combine(rootPath, Path.Combine("PCs", string.Format("{0}.json", character.Name))),
                    Character.Character.SerializeToJson(character));
            }
            File.WriteAllText(Path.Combine(rootPath, "GameState.json"), SerializeToJson(gameState));
            File.WriteAllText(Path.Combine(rootPath, "GlobalState.json"), Core.GlobalState.SerializeToJson());
        }

        #endregion

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
