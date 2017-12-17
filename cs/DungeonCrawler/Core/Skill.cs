using Newtonsoft.Json;
using System.Collections.Generic;

namespace DungeonCrawler.Core
{
    public class Skill
    {
        public string Name;
        public string Description;
        public string[] OpposingSkills;
        public string[] Actions;

        public static Skill DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Skill>(json);
        }
    }
}