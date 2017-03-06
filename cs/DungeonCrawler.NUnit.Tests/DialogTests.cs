using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using NUnit.Framework;
using DungeonCrawler.Core;
using DungeonCrawler.Items;
using DungeonCrawler.Dialogs;


namespace DungeonCrawler.NUnit.Tests
{
    namespace DungeonCrawler.NUnit
    {
        [TestFixture]
        class DialogTests
        {
            [Test]
            public void DialogTest()
            {
                // Initialize the GameConditions
                var conditions = SpreadsheetHandler.GetGameConditions();
                GameConditions.Conditions = conditions;

                // Get the dialogs
                var dialogs = SpreadsheetHandler.GetDialogs();
                   
                for (int i = 0; i < dialogs.Length; i++)
                {
                    Console.WriteLine(dialogs[i].Knots.Length);
                    Console.WriteLine(dialogs[i].Options.Length);
                }

                //// Create the Manager for Dialogs
                //DialogManager manager = new DialogManager(dialogs);

                //// Go through the story
                //manager.StartDialog("StartAgolmar");
            }
        }
    }
}
