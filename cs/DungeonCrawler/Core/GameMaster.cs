using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{
    public static class GameMaster
    {
        public static Cell CurrentCell;

        public static string[] CurrentTags
        {
            get
            {
                return CurrentCell.Tags;
            }
        }
    }
}
