using DungeonCrawler.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Dialogs
{

    public class Knot
    {
        public string Id;
        public string Text;
        public string Type;

        private string[] options;

        public string[] Options {
            get
            {
                return this.options;
            }
            set
            {
                this.options = value;
            }
        }
    }
      
    public class Option
    {
        public string Type;
        public string Id;
        public string Text;
        public string Destination;
        public bool Sticky;
        public Dictionary<string, int> Conditions;

        public bool MatchConditions()
        {
            if (Conditions == null)
            {
                return true;
            }
            foreach (KeyValuePair<string, int> entry in Conditions)
            {
                if (GameConditions.CheckCondition(entry.Key) != entry.Value)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class Dialog
    {
        public string CurrentKnot;
        public Knot[] Knots;
        public Option[] Options;

        public void Continue()
        {
            //Dialog dialog = GetDialogById(id);

            //Console.WriteLine(dialog.Text);
            //for (int i = 0; i < dialog.Options.Length; i++)
            //{
            //    Dialog option = GetDialogById(dialog.Options[i]);

            //    if (option.MatchConditions())
            //    {
            //        Console.WriteLine(string.Format("\t >> {0}", option.Text));
            //    }
            //}
        }
    }
}