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

        public static Character.Character[] CharactersOnCell(Cell cell)
        {
            List<Character.Character> characters = new List<Character.Character>();
            foreach (Character.Character character in Characters)
            {
                if (character.CurrentCell == cell)
                {
                    characters.Add(character);
                }
            }
            return characters.ToArray();
        }

        public static Character.Character[] CharactersOnGridPoint(GridPoint point, string[] types = null, Character.Character[] excludes = null)
        {
            return CharactersOnGridPoint(new int[] { point.X, point.Y }, types, excludes);
        }

        public static Character.Character[] CharactersOnGridPoint(int[] point, string[] types = null, Character.Character[] excludes = null)
        {
            return CharactersOnGridPoint(point[0], point[1], types, excludes);
        }

        public static Character.Character[] CharactersOnGridPoint(int x, int y, string[] types = null, Character.Character[] excludes = null)
        {
            List<Character.Character> characters = new List<Character.Character>();
            foreach (Character.Character character in Characters)
            {
                if (character.Transform.Position.X == x && character.Transform.Position.Y == y)
                {
                    if (types != null)
                    {
                        if (!Array.Exists(types, element => element == character.Type))
                        {
                            continue;
                        }
                    }
                    if (excludes != null)
                    {
                        if (Array.Exists(excludes, element => element == character))
                        {
                            continue;
                        }
                    }
                    characters.Add(character);
                }
            }
            return characters.ToArray();
        }

    }
}
