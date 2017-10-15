using DungeonCrawler.Core;
using Newtonsoft.Json;

namespace DungeonCrawler.Core
{
    public class Item
    {
        public string Name;
        public Aspect[] Aspects;
        public string[] Tags;
        public string EquipmentSlot;
        public bool IsUnique;

        [JsonIgnore]
        public virtual int Cost
        {
            get
            {
                int cost = 0;
                foreach(Aspect aspect in Aspects)
                {
                    cost += aspect.Cost;
                }
                cost += Tags.Length;
                return cost;
            }
        }

        public static Item DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Item>(json);
        }
    }

    public class Weapon : Item
    {
        public string[] Skills;
        public int Damage;
        public int[][] AttackShape;
        public float Speed;

        public override int Cost
        {
            get
            {
                int cost = base.Cost;
                cost += Damage * AttackShape.Length;
                return cost;
            }
        }

        new public static Weapon DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Weapon>(json);
        }
    }

    public class Armour : Item
    {
        public int Protection;
        public Character.Consequence[] Consequences;

        public override int Cost
        {
            get
            {
                int cost = base.Cost;
                cost += Protection;
                return cost;
            }
        }

        new public static Armour DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Armour>(json);
        }
    }
}
