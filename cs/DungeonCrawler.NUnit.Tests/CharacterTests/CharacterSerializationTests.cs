using NUnit.Framework;
using System;

namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    class CharacterSerializationTests
    {
        [Test]
        public void Serialize_deserialize_Character()
        {
            string json = Utilities.JsonResource("Hero");

            Character.Character deserializedChar = Character.Character.DeserializeFromJson(json);
            string deserialized = Character.Character.SerializeToJson(deserializedChar);

            Console.WriteLine(json);
            Console.WriteLine(deserialized);

            Assert.AreEqual(json, deserialized);
        }
    }
}
