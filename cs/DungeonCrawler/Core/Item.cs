using DungeonCrawler.Core;
using Newtonsoft.Json;
using System;
using System.IO;

namespace DungeonCrawler.Core
{
    public class Item
    {
        public string Id;
        public string Name;
        public Aspect[] Aspects;
        public string[] Tags;
        public string EquipmentSlot;
        public bool IsUnique;

        [JsonIgnore]
        public string Identifier
        {
            get
            {
                return string.Format("{0}-{1}", Name, Id);
            }
        }

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

        public Item ()
        {
            Id = Guid.NewGuid().ToString("N");
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
        public Character.AttackShapeMarker[] AttackShape;
        public float Speed;

        public override int Cost
        {
            get
            {
                int cost = base.Cost;
                foreach(Character.AttackShapeMarker shape in AttackShape)
                {
                    cost += (int)shape.Area();
                }
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
                foreach(Character.Consequence consequence in Consequences)
                {
                    cost += consequence.Capacity;
                }
                return cost;
            }
        }

        new public static Armour DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Armour>(json);
        }
    }
}
