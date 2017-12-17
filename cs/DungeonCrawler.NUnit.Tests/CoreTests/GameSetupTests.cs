using DungeonCrawler.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    public class GameSetupTests
    {
        [Test]
        public void Initialize_game_from_json_files()
        {
            GameMaster.RootDataPath = Utilities.RootDataPath;
            GameMaster.InitializeGame();

            Assert.Contains("rats", Rulebook.Instance.Tags["rat"]);

            Assert.AreEqual(1, Rulebook.Instance.Items.Count);
            Assert.AreEqual(1, Rulebook.Instance.Weapons.Count);
            Assert.AreEqual(1, Rulebook.Instance.Armours.Count);

            Assert.AreEqual("Athletics", Rulebook.Instance.Skills["Athletics"].Name);
            Assert.AreEqual("Craftsmanship", Rulebook.Instance.Skills["Craftsmanship"].Name);
            Assert.AreEqual("Healing", Rulebook.Instance.Skills["Healing"].Name);
            Assert.AreEqual("MeleeWeapons", Rulebook.Instance.Skills["MeleeWeapons"].Name);
            Assert.AreEqual("RangedWeapons", Rulebook.Instance.Skills["RangedWeapons"].Name);

            Console.WriteLine(Rulebook.SerializeToJson());
        }
    }
}
