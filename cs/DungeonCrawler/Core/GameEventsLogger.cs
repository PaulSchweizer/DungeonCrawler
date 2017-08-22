using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{
    public static class GameEventsLogger
    {

        private static List<string> Log = new List<string>();

        private static int index = 0;

        private static string AttackTemplate = "'{0}' attacks '{1}' with '{2}' {3} ({4} + D{5})";
        private static string DefendTemplate = "'{0}' defends from '{1}' with '{2}' {3} ({4} - {5} + D{6})";
        
        public static string Next
        {
            get
            {
                string next = "";
                if (index == Log.Count)
                {
                    return next;
                }
                for (int i = index; i < Log.Count; i++)
                {
                    next += Log[i];
                    if (i < Log.Count - 1)
                    {
                        next += "\n";
                    }
                    index = i+1;
                }
                return next;
            }
        }

        public static void Reset()
        {
            Log = new List<string>();
            index = 0;
        }

        public static void LogAttack(Character.Character attacker, Character.Character defender, 
            string skill, int totalValue, int skillValue, int diceValue)
        {
            Log.Add(string.Format(AttackTemplate, attacker.Name, defender.Name, 
                                  skill, totalValue, skillValue, diceValue));
        }

        public static void LogDefend(Character.Character attacker, Character.Character defender,
            string skill, int shifts, int attackValue, int defendValue, int diceValue)
        {
            Log.Add(string.Format(DefendTemplate, defender.Name, attacker.Name,
                                  skill, shifts, attackValue, defendValue, diceValue));
        }
    }
}
