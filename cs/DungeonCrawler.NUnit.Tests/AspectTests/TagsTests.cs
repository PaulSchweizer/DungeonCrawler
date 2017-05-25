using NUnit.Framework;
using DungeonCrawler.Aspect;

namespace DungeonCrawler.NUnit.Tests.AspectTest
{
    [TestFixture]
    public class TagsTests
    {
        [Test]
        public void Serialize_deserialize_TagsTable()
        {
            string json = Utilities.JsonResource("TagsTable");
            TagsTable.DeserializeFromJson(json);
            string deserialized = TagsTable.SerializeToJson();
            // Assert.AreEqual(json, deserialized);
        }

        [Test]
        public void Tags_can_have_synonyms()
        {
            TagsTable.DeserializeFromJson(Utilities.JsonResource("TagsTable"));
            Assert.Contains("darkness", TagsTable.SynonymsOf("Dark"));
            Assert.IsEmpty(TagsTable.SynonymsOf("doesNotExist"));
        }

        [Test]
        public void Tags_can_have_opposing_tags()
        {
            TagsTable.DeserializeFromJson(Utilities.JsonResource("TagsTable"));
            Assert.Contains("dark", TagsTable.OppositesOf("Light"));
            Assert.IsEmpty(TagsTable.OppositesOf("doesNotExist"));
        }
    }
}
