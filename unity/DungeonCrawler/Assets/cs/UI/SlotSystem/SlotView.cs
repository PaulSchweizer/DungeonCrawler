using UnityEngine;
using System.Collections.Generic;
using DungeonCrawler.Character;
using DungeonCrawler.Core;

namespace SlotSystem
{
    public class SlotView : MonoBehaviour, ISlotChanged
    {
        public int NumberOfSlots;
        public bool InfiniteSlots;
        public Slot SlotPrefab;
        public Transform SlotParent;

        public SlottableItem SlottableItemPrefab;

        public Dictionary<string, SlottableItem> _items = new Dictionary<string, SlottableItem>() { };

        private readonly List<Slot> _slots = new List<Slot>();

        private void Awake()
        {
            for (int i = 0; i < NumberOfSlots; i++)
            {
                AddSlot();
            }
        }

        public void InitFromInventoryItems(Inventory inventory)
        {
            NumberOfSlots = inventory.Items.Count;
            ResetSlots();
            _items.Clear();
            foreach (Item item in inventory.Items)
            {
                AddItem(item, inventory.Amount(item.Name));
            }
        }

        public void InitFromInventoryWeapons(Inventory inventory)
        {
            NumberOfSlots = inventory.Weapons.Count;
            ResetSlots();
            _items.Clear();
            foreach(Weapon item in inventory.Weapons)
            {
                AddItem(item, inventory.Amount(item.Identifier));
            }
        }

        public void InitFromInventoryArmour(Inventory inventory)
        {
            NumberOfSlots = inventory.Armour.Count;
            ResetSlots();
            _items.Clear();
            foreach (Armour item in inventory.Armour)
            {
                AddItem(item, inventory.Amount(item.Identifier));
            }
        }

        public void RemoveItem(string name, int amount)
        {
            SlottableItem viewItem;
            if (_items.TryGetValue(name, out viewItem))
            {
                viewItem.Amount -= amount;
                if (viewItem.Amount <= 0)
                {
                    _items.Remove(name);
                    viewItem.Slot.Clear();
                }
                else
                {
                    viewItem.UpdateDisplay();
                }
            }
        }

        public void ResetSlots()
        {
            foreach (Slot slot in _slots)
            {
                slot.gameObject.SetActive(false);
                if (slot.Item != null)
                {
                    slot.Item.gameObject.SetActive(false);
                    slot.Item.Amount = 0;
                    slot.Item.Item = null;
                    _items.Remove(slot.Item.Name.text);
                }
            }
            for (int i = 0; i < NumberOfSlots; i++)
            {
                if (i < _slots.Count)
                {
                    _slots[i].gameObject.SetActive(true);
                }
                else
                {
                    AddSlot();
                }
            }
        }

        protected Slot AddSlot()
        {
            Slot slot = Instantiate(SlotPrefab, SlotParent);
            slot.name = string.Format("Slot{0}", _slots.Count);
            _slots.Add(slot);
            slot.transform.localScale = new Vector3(1, 1, 1);
            slot.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            return slot;
        }

        public Slot NextAvailableSlot()
        {
            if (_items.Count > NumberOfSlots)
            {
                return InfiniteSlots ? AddSlot() : null;
            }
            foreach (Slot slot in _slots)
            {
                if (slot.Item == null)
                {
                    return slot;
                }
            }
            return InfiniteSlots ? AddSlot() : null;
        }

        public void AddItem(SlottableItem item)
        {
            Slot slot = NextAvailableSlot();
            if (slot != null)
            {
                slot.Drop(item);
            }
        }

        public void AddItem(Item item, int amount)
        {
            SlottableItem viewItem;
            if (_items.TryGetValue(item.Name, out viewItem))
            {
                viewItem.Amount += amount;
                viewItem.UpdateDisplay();
            }
            else
            {
                foreach (Slot slot in _slots)
                {
                    if (slot.Item != null)
                    {
                        if (slot.Item.Item == null)
                        {
                            slot.Item.Init(item, amount);
                            return;
                        }
                    }
                }
                viewItem = Instantiate(SlottableItemPrefab);
                viewItem.Init(item, amount);
                _items[item.Name] = viewItem;
                AddItem(viewItem);
            }
        }

        //public void ClearView()
        //{
        //    foreach (Slot slot in _slots)
        //    {
        //        if (!slot.IsFree)
        //        {
        //            //GameObject.Destroy(slot.Item.gameObject);
        //            slot.Item.gameObject.SetActive(false);
        //            slot.Clear();
        //        }
        //    }
        //}

        public void SlotChanged(Slot slot, SlottableItem currentItem, SlottableItem previousItem)
        {

        }
    }
}
