using DungeonCrawler.Core;
using NUnit.Framework;
using System;

namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class GlobalGameStateTests
    {
        [Test]
        public void Test_GlobalGameState_is_serializable()
        {
            GlobalState.DeserializeFromJson(Utilities.JsonResource("GameState.GlobalState"));
            Console.WriteLine(GlobalState.SerializeToJson());
            Assert.IsTrue(GlobalState.Get("TrueCondition"));
            Assert.IsFalse(GlobalState.Get("FalseCondition"));
        }
    }
}
