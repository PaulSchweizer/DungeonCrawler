using NUnit.Framework;
using DungeonCrawler.Core;

namespace DungeonCrawler.NUnit.Tests.CoreTests
{
    [TestFixture]
    public class AspectTests
    {
        [Test]
        public void Cost_depends_on_aspect_fields()
        {
            Aspect aspect = new Aspect("#dark #knight", new string[] { "MeleeWeapons" }, 1);
            Assert.AreEqual(5, aspect.Cost);

            aspect = new Aspect("#dark #knight", new string[] { "MeleeWeapons" }, -1);
            Assert.AreEqual(-5, aspect.Cost);

            aspect = new Aspect("#dark #knight", new string[] { "MeleeWeapons" }, 0);
            Assert.AreEqual(3, aspect.Cost);
        }

        [Test]
        public void Extract_tags_from_text()
        {
            Aspect aspect = new Aspect("The #dark, #knight.", new string[] { }, 1);
            foreach (string tag in new string[] { "dark", "knight" })
            {
                Assert.Contains(tag, aspect.Tags);
            }
        }

        [Test]
        public void Check_if_aspect_has_tags()
        {
            Aspect aspect = new Aspect("The #dark, #knight.", new string[] { }, 1);
            Assert.AreEqual(1, aspect.Matches(new string[] { "Dark", "notDark" }));
            Assert.AreEqual(0, aspect.Matches(new string[] { "notDark" }));
        }

        [Test]
        public void Aspect_with_tag_any_is_always_triggered()
        {
            Aspect aspect = new Aspect("This is triggered by #Any tag.", new string[] { }, 1);
            Assert.AreEqual(1, aspect.Matches(new string[] { }));
            aspect = new Aspect("This is triggered by #Any tag and #dark.", new string[] { }, 1);
            Assert.AreEqual(2, aspect.Matches(new string[] { "dark" }));
        }

        [Test]
        public void Tags_are_checked_for_synonyms_before_matched()
        {
            Rulebook.Instance.Tags["rat"] = new string[] { "rats" };
            Aspect aspect = new Aspect("The synonym for #rats is rat.", new string[] { }, 1);
            Assert.AreEqual(1, aspect.Matches(new string[] { "rat" }));

            aspect = new Aspect("Matching rats with it's synonym #rat.", new string[] { }, 1);
            Assert.AreEqual(1, aspect.Matches(new string[] { "rats" }));
        }
    }
}