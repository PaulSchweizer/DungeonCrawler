using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{
    public static class Dice
    {
        public static Random Die = new Random();

        public static int Roll()
        {
            int result = 0;
            for (int i = 0; i < 4; i++)
            {
                result += Die.Next(-1, 2);
            }
            return result;
        }
    }
}
