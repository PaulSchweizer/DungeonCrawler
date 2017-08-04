using DungeonCrawler.Core;
using NUnit.Framework;

namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    public class CharacterSkillsTests
    {
        Character.Character hero;

        [SetUp]
        public void SetUp()
        {
            hero = Utilities.Hero();
            Utilities.LoadRulebook();
        }

        [Test]
        public void Skill_modifiers_as_a_list()
        {
            // No modifiers because no tag is given
            int[] modifiers = hero.SkillValueModifiers(skill: "MeleeWeapons", tags: new string[] { });
            Assert.AreEqual(new int[] { }, modifiers);

            // Add a tag that affects the skill
            modifiers = hero.SkillValueModifiers(skill: "MeleeWeapons", tags: new string[] { "rat" });
            Assert.AreEqual(new int[] { 1 }, modifiers);

            // Add some Equipment
            hero.Equip(Rulebook.Weapon("Weapon").Name, "RightHand");
            hero.Equip(Rulebook.Armour("Armour").Name, "Torso");
            modifiers = hero.SkillValueModifiers(skill: "MeleeWeapons", tags: new string[] { "rat" });
            Assert.AreEqual(new int[] { 1, 1, 1 }, modifiers);
        }
    }
}
