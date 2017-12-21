using DungeonCrawler.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DungeonCrawler.Core
{
    public static class GameMaster
    {
        public static string RootDataPath;

        public static Location CurrentLocation;

        public static Cell CurrentCell;

        public static List<Character.Character> Characters = new List<Character.Character>();

        public static void RegisterCharacter(Character.Character character)
        {
            if (!Characters.Contains(character))
            {
                Characters.Add(character);
            }
        }

        public static void DeRegisterCharacter(Character.Character character)
        {
            if (Characters.Contains(character))
            {
                Characters.Remove(character);
            }
        }

        public static string[] CurrentTags
        {
            get
            {
                if (CurrentCell != null)
                {
                    return CurrentCell.Tags;
                }
                else
                {
                    return new string[] { };
                }
            }
        }

        public static List<Character.Character> CharactersOfType(string[] types)
        {
            List<Character.Character> characters = new List<Character.Character>();
            foreach (Character.Character character in Characters)
            {
                if (Array.Exists(types, element => element == character.Type))
                {
                    characters.Add(character);
                }
            }
            return characters;
        }

        public static List<Character.Character> CharactersOfType(string type)
        {
            List<Character.Character> characters = new List<Character.Character>();
            foreach (Character.Character character in Characters)
            {
                if (character.Type == type)
                {
                    characters.Add(character);
                }
            }
            return characters;
        }

        #region Initialize

        public static void InitializeGame()
        {
            // Initialize Rulebook, including Aspects - Tags Mapping
            Rulebook.DeserializeFromJson(File.ReadAllText(Path.Combine(RootDataPath, "Rulebook.json")));

            // Items, Weapons and Armour
            foreach (string json in Directory.GetFiles(Path.Combine(Path.Combine(RootDataPath, "Items"), "Items")))
            {
                if (json.EndsWith(".json"))
                {
                    Rulebook.Instance.Items.Add(Item.DeserializeFromJson(File.ReadAllText(json)));
                }
            }
            foreach (string json in Directory.GetFiles(Path.Combine(Path.Combine(RootDataPath, "Items"), "Weapons")))
            {
                if (json.EndsWith(".json"))
                {
                    Rulebook.Instance.Weapons.Add(Weapon.DeserializeFromJson(File.ReadAllText(json)));
                }
            }
            foreach (string json in Directory.GetFiles(Path.Combine(Path.Combine(RootDataPath, "Items"), "Armour")))
            {
                if (json.EndsWith(".json"))
                {
                    Rulebook.Instance.Armours.Add(Armour.DeserializeFromJson(File.ReadAllText(json)));
                }
            }

            // Skills
            foreach (string json in Directory.GetFiles(Path.Combine(RootDataPath, "Skills")))
            {
                if (json.EndsWith(".json"))
                {
                    Skill skill = Skill.DeserializeFromJson(File.ReadAllText(json));
                    Rulebook.Instance.Skills[skill.Name] = skill;
                }
            }
        }

        #endregion

        #region Save and Load

        public static void SaveCurrentGame(string name)
        {
            GameState.Save(name);
        }

        public static void LoadGame(string name)
        {
            GameState.Load(name);
        }

        #endregion
    }
}
