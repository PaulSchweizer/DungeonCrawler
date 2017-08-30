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

        private static string AttackTemplate = "\u2694 __{0}__ _attacks_ __{1}__ with _{2}_ {3} ({4}+D{5})";
        private static string DefendTemplate = "\u26C9 __{0}__ _defends_ against __{1}__ with _{2}_ {3} ({4}+D{5})";
        private static string ReceivePhysicalStressTemplate = "__{0}__ receives {1} _PhysicalStress_";
        private static string TakeConsequenceTemplate = "__{0}__ takes _Consequence_ \"{1}\" {2}";
        private static string GetsTakenOutTemplate = "\u271D __{0}__ is _taken out_";
        private static string GainsSpinTemplate = "__{0}__ gains {1} _spin_";

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

        public static void LogGainsSpin(Character.Character character, int spin)
        {
            Log.Add(string.Format(GainsSpinTemplate, character.Name, spin));
        }
    }
}
