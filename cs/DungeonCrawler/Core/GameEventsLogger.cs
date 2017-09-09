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

        private static string SpearatorTemplate = "#### {0}";
        private static string AttackTemplate = "\u2694 __{0}__ _attacks_ __{1}__ with _{2}_ {3} ({4}+D{5})";
        private static string DefendTemplate = "\u26C9 __{0}__ _defends_ against __{1}__ with _{2}_ {3} ({4}+D{5})";
        private static string ReceivePhysicalStressTemplate = "\u2661 __{0}__ receives {1} _PhysicalStress_";
        private static string TakeConsequenceTemplate = "\u2661 __{0}__ takes _Consequence_ \"{1}\" {2}";
        private static string GetsTakenOutTemplate = "\u271D __{0}__ is _taken out_";
        private static string GainsSpinTemplate = "\\+ __{0}__ gains {1} _spin_";
        private static string UsesStuntTemplate = "\u272A __{0}__ uses __Stunt__ _{1}_ ({2})";
        private static string UsesSpinTemplate = "\u2A39 __{0}__ uses {1} __Spin__";
        private static string ReceivesXPTemplate = "\u2795 __{0}__ receives {1} __XP__";
        private static string ReachesNextLevelTemplate = "\u2191 __{0}__ reaches __Level__ {1}";

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

        public static void LogSeparator(string title = "")
        {
            Log.Add(string.Format(SpearatorTemplate, title));
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

        public static void LogUsesStunt(Character.Character character, Stunt stunt)
        {
            Log.Add(string.Format(UsesStuntTemplate, character.Name, stunt.Name, stunt.Bonus));
        }

        public static void LogUsesSpin(Character.Character character, int spin)
        {
            Log.Add(string.Format(UsesSpinTemplate, character.Name, spin));
        }

        public static void LogReceivesXP(Character.Character character, int xp)
        {
            Log.Add(string.Format(ReceivesXPTemplate, character.Name, xp));
        }

        public static void LogReachesNextLevel(Character.Character character)
        {
            Log.Add(string.Format(ReachesNextLevelTemplate, character.Name, character.Level));
        }
    }
}
