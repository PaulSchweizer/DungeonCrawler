using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{
    public static class GameMaster
    {
        public static Location CurrentLocation;
        public static Cell CurrentCell;

        public static List<Character.Character> Characters;

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
    }
}
