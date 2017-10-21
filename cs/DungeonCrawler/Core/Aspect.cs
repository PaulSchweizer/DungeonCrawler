using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DungeonCrawler.Core
{
    public class Aspect
    {
        public string Name;
        public string[] Skills;
        public int Bonus;
        [JsonIgnore]
        public string[] Tags;

        public Aspect(string name, string[] skills, int bonus)
        {
            Name = name;
            Skills = skills;
            Bonus = bonus;
            Tags = ParseNameForTags();
        }

        [JsonIgnore]
        public int Cost
        {
            get
            {
                int cost = Tags.Length + Skills.Length;
                if (Bonus > 0)
                {
                    cost += 2;
                }
                else if (Bonus < 0)
                {
                    cost = -cost - 2;
                }
                return cost;
            }
        }

        public int Matches(string[] tags)
        {
            int matches = 0;

            if (Array.Exists(Tags, element => element == "any"))
            {
                matches += 1;
            }

            foreach (string tag in tags)
            {
                if (Array.Exists(Tags, element => element == tag.ToLower()))
                {
                    matches += 1;
                }
                else
                {
                    foreach (string synonym in Rulebook.SynonymsOf(tag))
                    {
                        if (Array.Exists(Tags, element => element == synonym.ToLower()))
                        {
                            matches += 1;
                            break;
                        }                    
                    }
                }
            }
            return matches;
        }

        private string[] ParseNameForTags()
        {
            var matches = Regex.Matches(Name, @"(?<=#)\w+");
            List<string> tags = new List<string>();
            foreach (Match m in matches)
            {
                tags.Add(m.Value.ToLower());
            }
            return tags.ToArray();
        }
    }
}