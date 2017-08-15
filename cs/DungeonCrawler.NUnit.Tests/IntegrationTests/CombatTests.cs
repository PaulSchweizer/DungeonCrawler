using NUnit.Framework;
using DungeonCrawler.Core;

namespace DungeonCrawler.NUnit.Tests.IntegrationTests
{
    [TestFixture]
    public class CombatTests
    {
        Character.Character hero;
        Character.Character rat;
        Weapon weapon;
        Armour armour;

        [SetUp]
        public void SetUp()
        {
            hero = Utilities.Hero();
            rat = Utilities.Rat();
            weapon = Utilities.Weapon();
            armour = Utilities.Armour();
            Utilities.LoadRulebook();
        }

        //[Test]
        //public void Hero_suffers_damage_from_rat_attack()
        //{
        //    rat.Attack(hero);
        //}
    }
}
