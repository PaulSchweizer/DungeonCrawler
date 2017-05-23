using NUnit.Framework;

namespace DungeonCrawler.NUnit.Tests.AspectTests
{
    [TestFixture]
    public class AspectTests
    {
        [Test]
        public void Cost_depends_on_aspect_fields()
        {
            Aspect.Aspect aspect = new Aspect.Aspect("#dark #knight", new string[]{ "Combat" }, 1);
            Assert.AreEqual(5, aspect.Cost);

            aspect = new Aspect.Aspect("#dark #knight", new string[] { "Combat" }, -1);
            Assert.AreEqual(-5, aspect.Cost);

            aspect = new Aspect.Aspect("#dark #knight", new string[] { "Combat" }, 0);
            Assert.AreEqual(3, aspect.Cost);
        }

        [Test]
        public void Extract_tags_from_text()
        {
            Aspect.Aspect aspect = new Aspect.Aspect("The #dark, #knight.", new string[] { "Combat" }, 1);
            foreach (string tag in new string[] { "dark", "knight" })
            {
                Assert.Contains(tag, aspect.Tags);
            }
        }

        [Test]
        public void Check_if_aspect_has_tags()
        {
            Aspect.Aspect aspect = new Aspect.Aspect("The #dark, #knight.", new string[] { "Combat" }, 1);
            Assert.IsTrue(aspect.HasAnyTag(new string[] { "Dark", "notDark" }));
            Assert.IsFalse(aspect.HasAnyTag(new string[] { "notDark" }));
        }
    }
}