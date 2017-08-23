using DungeonCrawler.Character;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{
    public static class GameEventsLogger
    {

        private static List<string> Log = new List<string>();

        private static int index = 0;

        private static string AttackTemplate = "\u2694 \"{0}\" attacks \"{1}\" with \"{2}\" {3} ({4}+D{5})";
        private static string DefendTemplate = "\u26C9 \"{0}\" defends against \"{1}\" with \"{2}\" {3} ({4}+D{5})";
        private static string ReceivePhysicalStressTemplate = "* \"{0}\" receives {1} PhysicalStress";
        private static string TakeConsequenceTemplate = "\"{0}\" takes Consequence {1} {2}";
        private static string GetsTakenOutTemplate = "\u271D \"{0}\" is taken out.";

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
            string skill, int totalDefendValue, int defendValue, int diceValue)
        {
            Log.Add(string.Format(DefendTemplate, defender.Name, attacker.Name,
                                  skill, totalDefendValue, defendValue, diceValue));
        }

        public static void LogReceivePhysicalStress(Character.Character character, int damage)
        {
            Log.Add(string.Format(ReceivePhysicalStressTemplate, character.Name, damage));
        }

        public static void LogTakeConsequence(Character.Character character, Consequence consequence)
        {
            Log.Add(string.Format(TakeConsequenceTemplate, character.Name, consequence.Name, consequence.Capacity));
        }

        public static void LogGetsTakenOut(Character.Character character)
        {
            Log.Add(string.Format(GetsTakenOutTemplate, character.Name));
        }
    }
}
