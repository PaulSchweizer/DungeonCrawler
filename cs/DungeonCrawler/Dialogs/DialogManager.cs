using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Dialogs
{
    public class DialogManager
    {

        public Dialog[] Dialogs;

        public DialogManager (Dialog[] dialogs)
        {
            Dialogs = dialogs;
        }

        //public void StartDialog(string id)
        //{
        //    Dialog dialog = GetDialogById(id);

        //    Console.WriteLine(dialog.Text);
        //    for (int i = 0; i < dialog.Options.Length; i++)
        //    {
        //        Dialog option = GetDialogById(dialog.Options[i]);

        //        if (option.MatchConditions())
        //        {
        //            Console.WriteLine(string.Format("\t >> {0}", option.Text));
        //        }
        //    }
        //}

        //public Dialog GetDialogById(string id)
        //{
        //    Dialog dialog = null;
        //    for (int i = 0; i < Dialogs.Length; i++)
        //    {
        //        if (Dialogs[i].Id == id)
        //        {
        //            dialog = Dialogs[i];
        //            break;
        //        }
        //    }
        //    return dialog;
        //}
    }
}
