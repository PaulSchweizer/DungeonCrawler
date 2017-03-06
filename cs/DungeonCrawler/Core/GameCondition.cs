using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{
    public static class GameConditions
    {
        public static GameCondition[] Conditions;

        public static int CheckCondition (string condition)
        {
            int value = 0;
            for (int i = 0; i < Conditions.Length; i++)
            {
                if (Conditions[i].Name == condition)
                {
                    value = Conditions[i].Value;
                    break;
                }
            }
            return value;
        }
    }

    public class GameCondition
    {
        public int Id;
        public string Name;
        public int Value;
    }
}
