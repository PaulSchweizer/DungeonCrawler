using DungeonCrawler.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{
    public static class GameMaster
    {
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

        public static Character.Character[] CharactersOfType(string[] types)
        {
            List<Character.Character> characters = new List<Character.Character>();
            foreach (Character.Character character in Characters)
            {
                if (Array.Exists(types, element => element == character.Type))
                {
                    characters.Add(character);
                }
            }
            return characters.ToArray();
        }
    }
}
