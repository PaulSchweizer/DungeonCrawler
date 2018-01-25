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

        public static void InitializeGame(string rulebook, string[] armours, string[] items, string[] weapons, 
                                          string[] skills, string[] monsters, string[] locations, string[] cellBlueprints)
        {
            // Initialize Rulebook, including Aspects - Tags Mapping
            Rulebook.DeserializeFromJson(rulebook);

            // Items, Weapons and Armour
            foreach (string item in items)
            {
                Rulebook.Instance.Items.Add(Item.DeserializeFromJson(item));
            }
            foreach (string weapon in weapons)
            {
                Rulebook.Instance.Weapons.Add(Weapon.DeserializeFromJson(weapon));
            }
            foreach (string armour in armours)
            {
                Rulebook.Instance.Armours.Add(Armour.DeserializeFromJson(armour));
            }

            // Skills
            foreach (string skillData in skills)
            {
                Skill skill = Skill.DeserializeFromJson(skillData);
                Rulebook.Instance.Skills[skill.Name] = skill;
            }

            // Monsters - omitted for now
            //

            // Cell Blueprints
            foreach (string cellBlueprintData in cellBlueprints)
            {
                CellBlueprint cellBlueprint = CellBlueprint.DeserializeFromJson(cellBlueprintData);
                Rulebook.Instance.CellBlueprints[cellBlueprint.Type] = cellBlueprint;
            }

            // Locations
            foreach (string locationData in locations)
            {
                Location location = Location.DeserializeFromJson(locationData);
                Rulebook.Instance.Locations[location.Name] = location;
            }
        }

        #endregion

        #region Save and Load

        public static void SaveCurrentGame(string name)
        {
            GameState.Save(name);
        }

        public static void LoadGame(string[] pcs, string location, string globalState)
        {
            foreach (string pc in pcs)
            {
                RegisterCharacter(Character.Character.DeserializeFromJson(pc));
            }
            CurrentLocation = Rulebook.Instance.Locations[location];
            GlobalState.Instance = GlobalState.DeserializeFromJson(globalState);
        }

        #endregion
    }
}
