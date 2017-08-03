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
            hero = Utilities.Hero();
            rat = Utilities.Rat();
            Utilities.LoadRulebook();
        }

        [Test]
        public void All_aspects_affecting_Skill()
        {
            Assert.IsNotEmpty(hero.AspectsAffectingSkill("MeleeWeapons"));
            Assert.IsEmpty(hero.AspectsAffectingSkill("DoesNotExist"));
        }

        [Test]
        public void Aspects_change_skill_values()
        {
            int combatValue = hero.SkillValue("MeleeWeapons", new string[] { "Rat" });
            Assert.AreEqual(3, combatValue);
        }
    }
}
