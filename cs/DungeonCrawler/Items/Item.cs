using Newtonsoft.Json;

namespace DungeonCrawler.Items
{
    public class Item
    {
        public string Name;
        public string Type;
        public string Description;
        public Aspect.Aspect[] Aspects;
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
    }

    public class Armour : Item
    {
        public int Protection;
        public Character.Consequence[] Consequences;
    }
}
