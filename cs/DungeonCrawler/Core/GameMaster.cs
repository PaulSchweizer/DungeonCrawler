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

        public static Character.Character[] CharactersOnGridPoint(GridPoint point, string[] types = null)
        {
            return CharactersOnGridPoint(new int[] { point.X, point.Y }, types);
        }

        public static Character.Character[] CharactersOnGridPoint(int[] point, string[] types = null)
        {
            List<Character.Character> characters = new List<Character.Character>();
            foreach (Character.Character character in Characters)
            {
                if (character.Transform.Position.X == point[0] && character.Transform.Position.Y == point[1])
                {
                    if (types != null)
                    {
                        if (!Array.Exists(types, element => element == character.Type))
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
