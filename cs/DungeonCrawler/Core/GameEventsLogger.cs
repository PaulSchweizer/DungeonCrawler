using DungeonCrawler.Character;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{

    public class GameEventArgs : EventArgs
    {
        public string Name { get; set; }
        public string Details { get; set; }

        public GameEventArgs(string name, string details)
        {
            Name = name;
            Details = details;
        }
    }

    public delegate void EventLoggedHandler(object sender, GameEventArgs e);

    public static class GameEventsLogger
    {

        public enum LogFormat { terminal = 0, markup = 1 };

        public static LogFormat Format = LogFormat.terminal;

        public static event EventLoggedHandler OnEventLogged;

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
        private static string HealingSuccessTemplate = "__{0}__ _heals_ __{1}__ of _{2}_ with {3} ({4}+D{5})";
        private static string HealingFailTemplate = "__{0}__ fails at _healing_ __{1}__ of _{2}_ with {3} ({4}+D{5})";

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

        private static void AddLog(string log, string name = "Log", bool emitEvent = true)
        {
            if (Format == LogFormat.terminal)
            {
                log = log.Replace("_", "").Replace("#", "");
            }
            Log.Add(log);
            if (emitEvent)
            {
                OnEventLogged?.Invoke(name, new GameEventArgs(name, log));
            }
        }

        public static void Reset()
        {
            Log = new List<string>();
            index = 0;
        }

        public static void LogSeparator(string title = "")
        {
            AddLog(string.Format(SpearatorTemplate, title), emitEvent: false);
        }

        public static void LogAttack(Character.Character attacker, Character.Character defender, 
            string skill, int totalValue, int skillValue, int diceValue)
        {
            AddLog(string.Format(AttackTemplate, attacker.Name, defender.Name,
                                 skill, totalValue, skillValue, diceValue), "Attack");
        }

        public static void LogDefend(Character.Character attacker, Character.Character defender,
            string skill, int totalDefendValue, int defendValue, int diceValue)
        {
            AddLog(string.Format(DefendTemplate, defender.Name, attacker.Name,
                                  skill, totalDefendValue, defendValue, diceValue), "Defend");
        }

        public static void LogReceivePhysicalStress(Character.Character character, int damage)
        {
            AddLog(string.Format(ReceivePhysicalStressTemplate, character.Name, damage), "ReceivePhysicalStress");
        }

        public static void LogTakeConsequence(Character.Character character, Consequence consequence)
        {
            AddLog(string.Format(TakeConsequenceTemplate, character.Name, consequence.Name, consequence.Capacity), "TakePhysicalStress");
        }

        public static void LogGetsTakenOut(Character.Character character)
        {
            AddLog(string.Format(GetsTakenOutTemplate, character.Name), "TakenOut");
        }

        public static void LogGainsSpin(Character.Character character, int spin)
        {
            AddLog(string.Format(GainsSpinTemplate, character.Name, spin), "GainsSpin");
        }

        public static void LogUsesStunt(Character.Character character, Stunt stunt)
        {
            AddLog(string.Format(UsesStuntTemplate, character.Name, stunt.Name, stunt.Bonus), "UsesStunt");
        }

        public static void LogUsesSpin(Character.Character character, int spin)
        {
            AddLog(string.Format(UsesSpinTemplate, character.Name, spin), "UsesSpin");
        }

        public static void LogReceivesXP(Character.Character character, int xp)
        {
            AddLog(string.Format(ReceivesXPTemplate, character.Name, xp), "ReceivesXP");
        }

        public static void LogReachesNextLevel(Character.Character character)
        {
            AddLog(string.Format(ReachesNextLevelTemplate, character.Name, character.Level), "ReachesNextLevel");
        }

        public static void LogHealing(Character.Character character, Character.Character patient,
            Consequence consequence, int totalValue, int skillValue, int diceValue, bool success)
        {
            if (success)
            {
                AddLog(string.Format(HealingSuccessTemplate, character.Name, patient.Name,
                          consequence.Name, totalValue, skillValue, diceValue), "HealingSuccess");
            }
            else
            {
                AddLog(string.Format(HealingFailTemplate, character.Name, patient.Name,
                          consequence.Name, totalValue, skillValue, diceValue), "HealingFailure");
            }
        }
    }
}
