using DungeonCrawler.Core;
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
        public string Type;
        public int Capacity;
        public bool IsTaken;
        public Aspect Effect;

        public Consequence(string type, int capacity, bool isTaken = false, Aspect effect = null)
        {
            Type = type;
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

    public class Inventory
    {
        public Dictionary<string, Dictionary<string, int>> Items;
        
        public Inventory()
        {
            Items = new Dictionary<string, Dictionary<string, int>>();
        }

        public void AddItem(string name, int amount, int quality)
        {
            if (Items.ContainsKey(name))
            {
                Items[name]["Amount"] += amount;
                Items[name]["Quality"] = quality < Items[name]["Quality"] ? quality: Items[name]["Quality"];
            }
            else
            {
                Items[name] = new Dictionary<string, int>();
                Items[name]["Amount"] = amount;
                Items[name]["Quality"] = quality;
            }
        }

        public void RemoveItem(string name, int amount)
        {
            if (Items.ContainsKey(name))
            {
                Items[name]["Amount"] -= amount;
                if (Items[name]["Amount"] <= 0)
                {
                    Items.Remove(name);
                }
            }
            else
            {
                throw new Exception(string.Format("Item {0} not in the Inventory.", name));
            }
        }
    }

    public class Character
    {
        public int Id;
        public string Name;
        public Attribute PhysicalStress;
        public List<Consequence> Consequences;
        public Dictionary<string, int> Skills;
        public string[] Tags;
        public List<Aspect> Aspects;
        public Dictionary<string, string> Equipment;
        public Inventory Inventory;
        public bool IsTakenOut;

        #region Actions

        #endregion

        #region Aspects and Skills

        public int SkillValue(string skill, string[] tags)
        {
            int skillValue = Skills[skill];
            foreach (Aspect aspect in AspectsAffectingSkill(skill))
            {
                if (aspect.Matches(tags) > 0)
                {
                    skillValue += aspect.Bonus;
                }
            }
            return skillValue;
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
                if (Aspects != null)
                {
                    foreach (Aspect aspect in Aspects)
                    {
                        aspects.Add(aspect);
                    }
                }
                return aspects;
            }
        }

        #endregion

        #region Damage and Consequences

        #endregion

        #region Equipment

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