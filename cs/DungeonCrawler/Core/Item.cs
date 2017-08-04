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

        public static Item DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Item>(json);
        }
    }

    public class Weapon : Item
    {
        public string[] Skills;
        public int Damage;

        new public static Weapon DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Weapon>(json);
        }
    }

    public class Armour : Item
    {
        public int Protection;
        public Character.Consequence[] Consequences;

        new public static Armour DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Armour>(json);
        }
    }
}
