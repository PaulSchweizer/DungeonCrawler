using DungeonCrawler.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Character
{

    public class Inventory
    {
        public List<Item> Items;
        public List<Weapon> Weapons;
        public List<Armour> Armours;
        public Dictionary<string, int> Amounts;

        public Inventory()
        {
            Items = new List<Item>();
            Weapons = new List<Weapon>();
            Armours = new List<Armour>();
            Amounts = new Dictionary<string, int>();
        }

        public void AddItem(Item item, int amount = 1)
        {
            // If not unique, reference the item from the Rulebook instead
            if (!item.IsUnique)
            {
                item = Rulebook.Item(item.Name);
            }

            if (item is Weapon)
            {
                Weapons.Add(item as Weapon);
            }
            else if (item is Armour)
            {
                Armours.Add(item as Armour);
            }
            else if (item is Item)
            {
                Items.Add(item);
            }
            else
            {
                return;
            }

            if (Amounts.ContainsKey(item.Name))
            {
                Amounts[item.Name] += amount;
            }
            else
            {
                Amounts[item.Name] = amount;
            }
        }

        public Item Item(string name)
        {
            Item item = Items.Find(i => i.Name == name);
            if (item != null)
            {
                return item;
            }
            item = Weapons.Find(i => i.Name == name);
            if (item != null)
            {
                return item;
            }
            item = Armours.Find(i => i.Name == name);
            if (item != null)
            {
                return item;
            }
            return null;
        }

        public void RemoveItem(Item item, int amount = 1)
        {
            if (Amounts.ContainsKey(item.Name))
            {
                Amounts[item.Name] -= amount;

                // Weapons and Armour Items are unique items, 
                // so they are being removed from the list
                if (item is Weapon)
                {
                    Weapons.Remove(item as Weapon);
                }
                else if (item is Armour)
                {
                    Armours.Remove(item as Armour);
                }

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

        public static Inventory operator +(Inventory thisInventory, Inventory thatInventory)
        {
            // Add all items to this inventory
            foreach(Item item in thatInventory.Items)
            {
                thisInventory.AddItem(item, thatInventory.Amounts[item.Name]);
            }
            foreach (Weapon weapon in thatInventory.Weapons)
            {
                thisInventory.AddItem(weapon, thatInventory.Amounts[weapon.Name]);
            }
            foreach (Armour armour in thatInventory.Armours)
            {
                thisInventory.AddItem(armour, thatInventory.Amounts[armour.Name]);
            }

            // Remove items from other inventory
            thatInventory.Items = new List<Item>();
            thatInventory.Weapons = new List<Weapon>();
            thatInventory.Armours = new List<Armour>();
            thatInventory.Amounts = new Dictionary<string, int>();

            return thisInventory;
        }
    }
}
