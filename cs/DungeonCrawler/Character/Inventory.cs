using DungeonCrawler.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DungeonCrawler.Character
{

    #region Delegates

    public delegate void ItemAddedHandler(object sender, EventArgs e);
    public delegate void ItemRemovedHandler(object sender, EventArgs e);

    #endregion

    public class Inventory
    {
        public List<Item> Items;
        public List<Weapon> Weapons;
        public List<Armour> Armour;
        public Dictionary<string, int> Amounts;

        #region Events

        public event ItemAddedHandler OnItemAdded;
        public event ItemRemovedHandler OnItemRemoved;

        #endregion

        public Inventory()
        {
            Items = new List<Item>();
            Weapons = new List<Weapon>();
            Armour = new List<Armour>();
            Amounts = new Dictionary<string, int>();
        }

        public void AddItem(Item item, int amount = 1)
        {
            if (item is Weapon)
            {
                Weapons.Add(item as Weapon);
            }
            else if (item is Armour)
            {
                Armour.Add(item as Armour);
            }
            else if (item is Item)
            {
                // If not unique, reference the item from the Rulebook instead
                item = Rulebook.Item(item.Name);
                if (item != null)
                {
                    if (Amounts.ContainsKey(item.Name))
                    {
                        Amounts[item.Name] += amount;
                    }
                    else
                    {
                        Amounts[item.Name] = amount;
                        Items.Add(item);
                    }
                }
            }
            OnItemAdded?.Invoke(this, null);
        }

        public Item Item(string identifier)
        {
            Item item = Items.Find(i => i.Name == identifier);
            if (item != null)
            {
                return item;
            }
            item = Weapons.Find(i => i.Identifier == identifier);
            if (item != null)
            {
                return item;
            }
            item = Armour.Find(i => i.Identifier == identifier);
            if (item != null)
            {
                return item;
            }
            return null;
        }

        public void RemoveItem(Item item, int amount = 1)
        {
            if (!item.IsUnique)
            {
                if (Amounts.ContainsKey(item.Name))
                {
                    Amounts[item.Name] -= amount;
                    if (Amounts[item.Name] <= 0)
                    {
                        Amounts.Remove(item.Name);
                        if (item is Item)
                        {
                            item = Item(item.Name);
                            Items.Remove(item);
                        }
                    }
                }
            }
            else
            {
                if (item is Weapon)
                {
                    Weapons.Remove(item as Weapon);
                }
                else if (item is Armour)
                {
                    Armour.Remove(item as Armour);
                }
            }
            OnItemRemoved?.Invoke(this, null);
        }

        public int Amount(string identifier)
        {
            if (Amounts.TryGetValue(identifier, out int amount))
            {
                return amount;
            }
            else
            {
                for(int i = 0; i < Weapons.Count; i++)
                {
                    if (Weapons[i].Identifier == identifier)
                    {
                        return 1;
                    }
                }
                for (int i = 0; i < Armour.Count; i++)
                {
                    if (Armour[i].Identifier == identifier)
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }

        public static Inventory operator +(Inventory thisInventory, Inventory thatInventory)
        {
            // Add all items to this inventory
            foreach(Item item in thatInventory.Items)
            {
                thisInventory.AddItem(item, thatInventory.Amount(item.Name));
            }
            foreach (Weapon weapon in thatInventory.Weapons)
            {
                thisInventory.AddItem(weapon);
            }
            foreach (Armour armour in thatInventory.Armour)
            {
                thisInventory.AddItem(armour);
            }

            // Remove items from other inventory
            thatInventory.Items = new List<Item>();
            thatInventory.Weapons = new List<Weapon>();
            thatInventory.Armour = new List<Armour>();
            thatInventory.Amounts = new Dictionary<string, int>();

            return thisInventory;
        }

        #region Serialization

        public static Inventory DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Inventory>(json);
        }

        public static string SerializeToJson(Inventory inventory)
        {
            string json = JsonConvert.SerializeObject(inventory, Formatting.Indented,
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
