using NUnit.Framework;

namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    public class CharacterAspects
    {
        Character.Character hero;
        Character.Character rat;

        [SetUp]
        public void SetUp()
        {
            hero = Character.Character.DeserializeFromJson(Utilities.JsonResource("Hero"));
            rat = Character.Character.DeserializeFromJson(Utilities.JsonResource("Rat"));
        }

        [Test]
        public void All_aspects_affecting_Skill()
        {
            Assert.IsNotEmpty(hero.AspectsAffectingSkill("Combat"));
            Assert.IsEmpty(hero.AspectsAffectingSkill("DoesNotExist"));
        }

        [Test]
        public void Aspects_are_used_to_tag_a_Character()
        {
            Assert.Contains("rat", rat.Tags);
        }

        [Test]
        public void Aspects_change_skill_values()
        {
            int combatValue = hero.SkillValue("Combat", new string[] { "Rat" });
            Assert.AreEqual(3, combatValue);
        }
    }
}
