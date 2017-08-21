using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{
    public static class Dice
    {
        public static Random Die = new Random();

        public static Dictionary<int, int> Distribution =
            new Dictionary<int, int>() { { 1, -4 }, { 5, -3 }, { 15, -2 }, { 31, -1 }, { 50, 0 },
                                         { 66, 1 }, { 76, 2 }, { 80, 3 }, { 81, 4 }};

        public static int Roll()
        {
            int die = Die.Next(0, 82);
            foreach (int key in Distribution.Keys)
            {
                if (die <= key)
                {
                    return Distribution[key];
                }
            }
            return 0;
        }
    }
}
