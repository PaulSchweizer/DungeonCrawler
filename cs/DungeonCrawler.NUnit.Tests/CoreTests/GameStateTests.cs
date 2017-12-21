using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using DungeonCrawler.Core;

namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class GameStateTests
    {

        [Test]
        public void Save_and_load_GameState()
        {
            GameMaster.Characters = new List<Character.Character>();
            Utilities.LoadRulebook();
            Character.Character hero = Utilities.Hero();
            GameMaster.RegisterCharacter(hero);
            GameMaster.CurrentLocation = Utilities.Location();
            hero.Transform.Position = new Utility.Vector(100, 100);
            hero.Transform.Rotation = 66.6f;

            // Save
            GameMaster.SaveCurrentGame("Test");

            // Reset the Game
            GameMaster.DeRegisterCharacter(hero);
            GameMaster.CurrentLocation = null;

            // Load
            GameMaster.LoadGame("Test");

            Assert.AreEqual(1, GameMaster.CharactersOfType("Player").Count);
            Assert.AreEqual(hero.Name, GameMaster.CharactersOfType("Player")[0].Name);
            Assert.AreEqual(Utilities.Location().Name, GameMaster.CurrentLocation.Name);
        }
    }
}