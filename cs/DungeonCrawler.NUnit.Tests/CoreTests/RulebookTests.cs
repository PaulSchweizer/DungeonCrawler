using NUnit.Framework;
using DungeonCrawler.Core;
using System;

namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class RulebookTests
    {
        [Test]
        public void DeserializeRulebook()
        {
            string json = Utilities.JsonResource("Rulebook");
            Rulebook.DeserializeFromJson(json);
            Console.WriteLine(Rulebook.SerializeToJson());
        }
    }
}
