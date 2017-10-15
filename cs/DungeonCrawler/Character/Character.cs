using DungeonCrawler.Core;
using DungeonCrawler.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DungeonCrawler.Character
{
    public class Attribute
    {
        public int Value;
        public int MaxValue;
        public int MinValue;

        public Attribute(int value, int maxValue, int minValue)
        {
            Value = value;
            MaxValue = maxValue;
            MinValue = minValue;
        }
    }

    public class Consequence
    {
        public string Name;
        public int Capacity;
        public bool IsTaken;
        public Aspect Effect;

        public Consequence(string name, int capacity, bool isTaken = false, Aspect effect = null)
        {
            Name = name;
            Capacity = capacity;
            IsTaken = isTaken;
            Effect = effect;
        }

        public void Take()
        {
            IsTaken = true;
            Effect = new Aspect("Consequence Standin Aspect that affects #any skill.", new string[] { "MeleeCombat" }, -1);
        }
    }

    public struct AttackMarker
    {
        public int[][] Shape;
        public float Countdown;
        public string Skill;

        public AttackMarker(int[][] shape, float countdown, string skill)
        {
            Shape = shape;
            Countdown = countdown;
            Skill = skill;
        }

        public void Schedule(int[][] shape, float countdown, string skill)
        {
            Shape = shape;
            Countdown = countdown;
            Skill = skill;
        }
    }

    public class Character
    {
        public int Id;
        public string Type;
        public string Name;
        public int XP;
        public int SkillPoints;
        public Attribute PhysicalStress;
        public List<Consequence> Consequences;
        public Dictionary<string, int> Skills;
        public string[] Tags;
        public List<Aspect> Aspects;
        public List<Stunt> Stunts;
        public Dictionary<string, string> Equipment;
        public Inventory Inventory;
        public bool IsTakenOut;
        public string[] Enemies;

        [JsonIgnore]
        public AttackMarker ScheduledAttack = new AttackMarker();

        [JsonIgnore]
        public Transform Transform = new Transform(0, 0, 0);

        [JsonIgnore]
        public Cell CurrentCell;

        [JsonIgnore]
        public int Spin;

        [JsonIgnore]
        public int Cost
        {
            get
            {
                int cost = 0;
                foreach (int skillValue in Skills.Values)
                {
                    cost += skillValue;
                }
                foreach (Consequence consequence in AllConsequences)
                {
                    cost += consequence.Capacity;
                }
                cost += Protection;
                cost += Damage;
                cost += PhysicalStress.MaxValue;
                return cost;
            }
        }

        [JsonIgnore]
        public int[][] AttackShape
        {
            get
            {
                List<int[]> attackShape = new List<int[]>();
                foreach (Weapon weapon in Weapons)
                {
                    foreach (int[] shape in weapon.AttackShape)
                    {
                        if (!attackShape.Contains(shape))
                        {
                            attackShape.Add(shape);
                        }
                    }
                }
                if (attackShape.Count == 0)
                {
                    attackShape.Add(new int[] { 1, 0 });
                }
                return attackShape.ToArray();
            }
        }

        #region Transform

        public void MoveTo(int x, int y)
        {
            Cell targetCell = GameMaster.CurrentLocation.CellAt(x, y);
            if (targetCell != null)
            {
                Transform.Position.X = x;
                Transform.Position.Y = y;
                CurrentCell = targetCell;
            }
        }

        #endregion

        #region Aspects and Skills

        public int[] SkillValueModifiers(string skill, string[] tags)
        {
            List<int> modifiers = new List<int>();
            foreach (Aspect aspect in AspectsAffectingSkill(skill))
            {
                if (aspect.Matches(tags) > 0)
                {
                    modifiers.Add(aspect.Bonus);
                }
            }
            return modifiers.ToArray();
        }

        public int SkillValue(string skill, string[] tags)
        {
            int value = 0;
            if (Skills.ContainsKey(skill))
            {
                value = Skills[skill];
            }
            int[] modifiers = SkillValueModifiers(skill, tags);
            foreach (var item in modifiers)
            {
                value += item;
            }
            return value;
        }

        public Aspect[] AspectsAffectingSkill(string skill)
        {
            List<Aspect> aspects = new List<Aspect>();
            foreach (Aspect aspect in AllAspects)
            {
                if (Array.Exists(aspect.Skills, element => element == skill))
                {
                    aspects.Add(aspect);
                }
            }
            return aspects.ToArray();
        }

        [JsonIgnore]
        public List<Aspect> AllAspects
        {
            get
            {
                List<Aspect> aspects = new List<Aspect>();

                // Basic Aspects
                if (Aspects != null)
                {
                    foreach (Aspect aspect in Aspects)
                    {
                        aspects.Add(aspect);
                    }
                }

                // Aspects of all taken Consequences
                foreach (Consequence consequence in AllConsequences)
                {
                    if (consequence.IsTaken)
                    {
                        aspects.Add(consequence.Effect);
                    }
                }

                // Aspects from the equipped Items
                foreach (string itemName in Equipment.Values)
                {
                    if (itemName != null)
                    {
                        foreach (Aspect aspect in Rulebook.Item(itemName).Aspects)
                        {
                            aspects.Add(aspect);
                        }
                    }
                }

                return aspects;
            }
        }

        #endregion

        #region XP & Level

        [JsonIgnore]
        public int Level
        {
            get
            {
                return (int)Math.Sqrt(XP / 100);
            }
        }

        public void ReceiveXP(int xp)
        {
            GameEventsLogger.LogReceivesXP(this, xp);
            int previousLevel = Level;
            XP += xp;
            if (previousLevel < Level)
            {
                NextLevelReached();
            }
        }

        public void NextLevelReached()
        {
            GameEventsLogger.LogReachesNextLevel(this);
            SkillPoints += 1;
        }

        public void LevelUpSkill(string skill)
        {
            // Character doesn't have the skill yet
            if (!Skills.ContainsKey(skill))
            {
                if (SkillPoints > 0)
                {
                    Skills[skill] = 0;
                    SkillPoints -= 1;
                    return;
                }
            }

            int cost = Skills[skill] + 1;
            if (SkillPoints >= cost)
            {
                Skills[skill] += 1;
                SkillPoints -= cost;
            }
        }

        #endregion

        #region Equipment

        public bool Equip(string itemName, string slot)
        {
            if (Inventory.Item(itemName) == null)
            {
                return false;
            }
            Item item = Inventory.Item(itemName);
            if (slot == item.EquipmentSlot && Equipment.ContainsKey(slot))
            {
                if (Equipment[slot] != null)
                {
                    UnEquip(slot);
                }
                Equipment[slot] = itemName;
                return true;
            }
            return false;
        }

        public void UnEquip(string itemName)
        {
            foreach (KeyValuePair<string, string> entry in Equipment)
            {
                if (entry.Value == itemName)
                {
                    Equipment[entry.Key] = null;
                    return;
                }
            }
        }

        [JsonIgnore]
        public Weapon[] Weapons
        {
            get
            {
                List<Weapon> weapons = new List<Weapon>();
                foreach (string itemName in Equipment.Values)
                {
                    Item item = Inventory.Item(itemName);
                    if (item is Weapon)
                    {
                        weapons.Add(item as Weapon);
                    }
                }
                return weapons.ToArray();
            }
        }

        #endregion

        #region Damage and Consequences

        [JsonIgnore]
        public List<Consequence> AllConsequences
        {
            get
            {
                List<Consequence> consequences = new List<Consequence>();

                // 1. All the Consequences that equipped armour provides
                foreach (string itemName in Equipment.Values)
                {
                    if (itemName != null)
                    {
                        Item item = Rulebook.Item(itemName);
                        if (item is Armour)
                        {
                            Armour armour = (Armour)item;
                            foreach (Consequence consequence in armour.Consequences)
                            {
                                consequences.Add(consequence);
                            }
                        }
                    }
                }
                consequences.Sort((x, y) => x.Capacity.CompareTo(y.Capacity));

                // 1. Default Consequences
                if (consequences != null)
                {
                    foreach (Consequence consequence in Consequences)
                    {
                        consequences.Add(consequence);
                    }
                }

                return consequences;
            }
        }

        [JsonIgnore]
        public int Protection
        {
            get
            {
                int protection = 0;
                foreach (string itemName in Equipment.Values)
                {
                    Item item = Inventory.Item(itemName);
                    if (item is Armour)
                    {
                        Armour armour = item as Armour;
                        protection += armour.Protection;
                    }
                }
                return protection;
            }
        }

        [JsonIgnore]
        public int Damage
        {
            get
            {
                int damage = 0;
                foreach (Weapon weapon in Weapons)
                {
                    damage += weapon.Damage;
                }
                return damage;
            }
        }

        [JsonIgnore]
        public float AttackSpeed
        {
            get
            {
                float speed = 0;
                foreach (Weapon weapon in Weapons)
                {
                    if (weapon.Speed > speed)
                    {
                        speed = weapon.Speed;
                    }
                }
                return speed + 1;
            }
        }

        public void ReceiveDamage(int damage)
        {
            // Subtract Protection by Armour
            damage = Math.Max(damage - Protection, 0);

            if (PhysicalStress.Value + damage > PhysicalStress.MaxValue)
            {
                TakeConsequence(damage);
            }
            else
            {
                GameEventsLogger.LogReceivePhysicalStress(this, damage);
                PhysicalStress.Value += damage;
            }
        }

        public void TakeConsequence(int damage)
        {
            foreach (Consequence consequence in AllConsequences)
            {
                if (damage <= consequence.Capacity && !consequence.IsTaken)
                {
                    consequence.Take();
                    GameEventsLogger.LogTakeConsequence(this, consequence);
                    return;
                }
            }
            GetsTakenOut();
        }

        public void GetsTakenOut()
        {
            IsTakenOut = true;
            GameEventsLogger.LogGetsTakenOut(this);
        }

        #endregion

        #region Combat

        public Character[] EnemiesInReach()
        {
            List<Character> enemies = new List<Character>();
            for (int i = 0; i < AttackShape.Length; i++)
            {
                GridPoint point = new GridPoint(Transform.Map(AttackShape[i]));
                foreach (Character enemy in GameMaster.CharactersOnGridPoint(point, Enemies))
                {
                    enemies.Add(enemy);
                }
            }
            return enemies.ToArray();
        }

        public void ScheduleAttack(string attackSkill = "MeleeWeapons")
        {
            int[][] attackShape = new int[AttackShape.Length][];
            for(int i = 0; i < attackShape.Length; i++)
            {
                attackShape[i] = Transform.Map(AttackShape[i][0], AttackShape[i][1]);
            }
            float countdown = 1 / AttackSpeed;
            ScheduledAttack.Schedule(attackShape, countdown, attackSkill);
        }




        public void Attack(Character defender, string attackSkill = "MeleeWeapons", Stunt stunt = null)
        {
            GameEventsLogger.LogSeparator("Attack");
            List<string> tags = new List<string>(); 
            for(int i = 0; i < defender.Tags.Length; i++)
            {
                if (!tags.Contains(defender.Tags[i]))
                {
                    tags.Add(defender.Tags[i]);
                }
            }
            for (int i = 0; i < GameMaster.CurrentTags.Length; i++)
            {
                if (!tags.Contains(GameMaster.CurrentTags[i]))
                {
                    tags.Add(GameMaster.CurrentTags[i]);
                }
            }
            int skillValue = SkillValue(attackSkill, tags.ToArray());
            int diceValue = Dice.Roll();
            int totalValue = skillValue + diceValue;
            if (stunt != null)
            {
                totalValue += stunt.Bonus;
                GameEventsLogger.LogUsesStunt(this, stunt);
            }
            if (Spin > 0)
            {
                totalValue += Spin;
                GameEventsLogger.LogUsesSpin(this, Spin);
                Spin = 0;
            }
            GameEventsLogger.LogAttack(this, defender, attackSkill, totalValue, skillValue, diceValue);
            int shifts = defender.Defend(this, attackSkill, totalValue);
            if (shifts > 0)
            {
                defender.ReceiveDamage(shifts + Damage);
                if (defender.IsTakenOut)
                {
                    ReceiveXP(defender.Cost);
                }
            }
        }

        public int Defend(Character attacker, string attackSkill, int attackValue)
        {
            List<string> tags = new List<string>();
            for (int i = 0; i < attacker.Tags.Length; i++)
            {
                if (!tags.Contains(attacker.Tags[i]))
                {
                    tags.Add(attacker.Tags[i]);
                }
            }
            for (int i = 0; i < GameMaster.CurrentTags.Length; i++)
            {
                if (!tags.Contains(GameMaster.CurrentTags[i]))
                {
                    tags.Add(GameMaster.CurrentTags[i]);
                }
            }

            // Get the best defend skill
            int defendValue = 0;
            string defendSkill = null;
            foreach (string skill in Rulebook.Instance.Skills[attackSkill].OpposingSkills)
            {
                if (Skills.ContainsKey(skill))
                {
                    int skillValue = SkillValue(skill, tags.ToArray());
                    if (skillValue > defendValue)
                    {
                        defendValue = skillValue;
                        defendSkill = skill;
                    }
                }
            }
            int diceValue = Dice.Roll();
            int totalDefendValue = defendValue + diceValue;
            int shifts = attackValue - totalDefendValue;
            GameEventsLogger.LogDefend(attacker, this, defendSkill, totalDefendValue, defendValue, diceValue);
            if (shifts < -1)
            {
                int spin = shifts / -2;
                Spin += spin;
                GameEventsLogger.LogGainsSpin(this, spin);
            }

            return shifts;
        }

        #endregion
        
        #region Actions

        public void Heal(Character patient, Consequence consequence)
        {
            int skillValue = SkillValue("Healing", GameMaster.CurrentTags);
            int diceValue = Dice.Roll();
            int totalValue = skillValue + diceValue;
            bool success = false;
            if (totalValue >= consequence.Capacity)
            {
                consequence.IsTaken = false;
                success = true;
            }
            GameEventsLogger.LogHealing(this, patient, consequence, totalValue, skillValue, diceValue, success);
        }

        #endregion

        #region Serialization

        public static Character DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Character>(json);
        }

        public static string SerializeToJson(Character character)
        {
            string json = JsonConvert.SerializeObject(character, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.Indentation = 4;
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString(); 
            }
        }

        #endregion
    }
}