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
        public void SaveGameState()
        {
            Utilities.LoadRulebook();
            Character.Character hero = Utilities.Hero();
            GameMaster.RegisterCharacter(hero);
            GameMaster.CurrentLocation = Utilities.Location();

            GameState savedGame = GameMaster.SaveCurrentGame();

            Assert.AreEqual(1, savedGame.PlayerCharacters.Count);
            Assert.AreEqual(hero, savedGame.PlayerCharacters[0]);
            Console.WriteLine(GameState.SerializeToJson(savedGame));
            Assert.AreEqual(Utilities.JsonResource("SavedGame"), GameState.SerializeToJson(savedGame));
        }
    }
}