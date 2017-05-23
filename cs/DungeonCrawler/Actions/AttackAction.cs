using System;

namespace DungeonCrawler.Actions
{
    public class AttackAction
    {

//        Character.Attack(Character attacker,
//                 Skill skill,
//                 Weapon weapon,
//                 Stunt stunt,
//                 Character[] defenders,
//                 Situation situation){
//    int aspectBonus = AspectBonus(Attacker, defenders, situation);
//        int damage = Max(0, skill.Value
//                            + weapon.Bonus
//                            + stunt.Bonus
//                            + aspectBonus
//                            + DFate);
//    for (Character defender in defenders) {
//        defender.ReceiveDamage(damage, attacker);
//    }
//}

        public static int Attack(
            Character.Character attacker, Character.Character defender,
            string skill, string[] tags)
        {
            int attackValue = AttackValue(attacker, defender, skill, tags);
            int defendValue = DefendValue(attacker, defender, skill, tags);
            int shifts = attackValue - defendValue;

            if (shifts > 0)
            {
                defender.TakePhysicalDamage(shifts);
            }

            return shifts;
        }

        public static int AttackValue(
            Character.Character attacker, Character.Character defender,
            string skill, string[] tags)
        {
            string[] allTags = new string[tags.Length + defender.Tags.Length];
            Array.Copy(tags, allTags, tags.Length);
            Array.Copy(defender.Tags, 0, allTags, tags.Length, defender.Tags.Length);
            return attacker.SkillValue(skill, allTags);
        }

        public static int DefendValue(
            Character.Character attacker, Character.Character defender,
            string skill, string[] tags)
        {
            string[] allTags = new string[tags.Length + attacker.Tags.Length];
            Array.Copy(tags, allTags, tags.Length);
            Array.Copy(attacker.Tags, 0, allTags, tags.Length, attacker.Tags.Length);
            return defender.SkillValue(skill, allTags);
        }
    }
}
