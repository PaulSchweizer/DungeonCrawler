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
        public Aspect.Aspect Effect;

        public Consequence(string type, int capacity, bool isTaken=false, Aspect.Aspect effect=null)
        {
            Type = type;
            Capacity = capacity;
            IsTaken = isTaken;
            Effect = effect;
        }

        public void Take()
        {
            IsTaken = true;
            Effect = new Aspect.Aspect("Consequence Standin Aspect that affects #any skill.", new string[] { "Combat" }, -1);
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
        public List<Aspect.Aspect> Aspects;
        public bool IsTakenOut;

        public List<Aspect.Aspect> AllAspects
        {
            get
            {
                List<Aspect.Aspect> aspects = new List<Aspect.Aspect>();
                if (Aspects != null)
                {
                    foreach(Aspect.Aspect aspect in Aspects)
                    {
                        aspects.Add(aspect);
                    }
                }
                foreach (Consequence consequence in Consequences)
                {
                    if (consequence.IsTaken)
                    {
                        aspects.Add(consequence.Effect);
                    }
                }
                return aspects;
            }
        }

        public Aspect.Aspect[] AspectsAffectingSkill(string skill)
        {
            List<Aspect.Aspect> aspects = new List<Aspect.Aspect>();
            foreach (Aspect.Aspect aspect in AllAspects)
            {
                if (Array.Exists(aspect.Skills, element => element == skill))
                {
                    aspects.Add(aspect);
                }
            }
            return aspects.ToArray();
        }

        //[JsonIgnore]
        //public string[] Tags
        //{
        //    get {
        //        List<string> tags = new List<string>();
        //        foreach (Aspect.Aspect aspect in Aspects)
        //        {
        //            foreach(string tag in aspect.Tags)
        //            {
        //                tags.Add(tag);
        //            }
        //        }
        //        return tags.ToArray();
        //    }
        //}

        public int SkillValue(string skill, string[] tags)
        {
            int skillValue = Skills[skill];
            foreach (Aspect.Aspect aspect in AspectsAffectingSkill(skill))
            {
                if (aspect.Matches(tags) > 0)
                {
                    skillValue += aspect.Bonus;
                }
            }
            return skillValue;
        }

        public void TakePhysicalDamage(int shifts)
        {
            if (PhysicalStress.Value + shifts > PhysicalStress.MaxValue)
            {
                TakeConsequence(shifts);
            }
            else
            {
                PhysicalStress.Value += shifts;
            }
        }

        public void TakeConsequence(int shifts) {
            // Try to see if the damage can be absorbed by a consequence
            foreach (Consequence consequence in Consequences) {
                if (shifts <= consequence.Capacity && !consequence.IsTaken)
                {
                    consequence.Take();
                    return;
                }
            }

            IsTakenOut = true;
        }

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