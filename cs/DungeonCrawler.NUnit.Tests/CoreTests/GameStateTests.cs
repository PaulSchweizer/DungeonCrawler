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
            GameMaster.Characters = new List<Character.Character>();
            Utilities.LoadRulebook();
            Character.Character hero = Utilities.Hero();
            GameMaster.RegisterCharacter(hero);
            GameMaster.CurrentLocation = Utilities.Location();
            hero.Transform.Position = new Utility.Vector(100, 100);
            hero.Transform.Rotation = 66.6f;

             GameState savedGame = GameMaster.SaveCurrentGame();

            Assert.AreEqual(1, savedGame.PlayerCharacters.Count);
            Assert.AreEqual(hero, savedGame.PlayerCharacters[0]);
            Console.WriteLine(GameState.SerializeToJson(savedGame));
            Assert.AreEqual(Utilities.JsonResource("SavedGame").Replace("\n", "").Replace("\r", ""), 
                            GameState.SerializeToJson(savedGame).Replace("\n", "").Replace("\r", ""));
        }

        [Test]
        public void LoadGameState()
        {
            GameMaster.RootDataPath = Utilities.RootDataPath;
            string json = Utilities.JsonResource("SavedGame");
            GameMaster.LoadGame(json);

            Assert.AreEqual(1, GameMaster.Characters.Count);
            Assert.AreEqual(100.0, GameMaster.Characters[0].Transform.Position.X);
            Assert.AreEqual(100.0, GameMaster.Characters[0].Transform.Position.Y);
            Assert.AreEqual("Location", GameMaster.CurrentLocation.Name);
        }
    }
}