using SlotSystem;
using UnityEngine;
using DungeonCrawler.Character;

public class InventoryUI : MonoBehaviour
{
    public SlotView Items;
    public SlotView Weapons;
    public SlotView Armour;

    private void Awake()
    {
        ChangeView("Items" );
    }

    public void ChangeView(string view)
    {
        Items.gameObject.SetActive(false);
        Weapons.gameObject.SetActive(false);
        Armour.gameObject.SetActive(false);
        if (view == "Items")
        {
            Items.gameObject.SetActive(true);
        }
        else if (view == "Weapons")
        {
            Weapons.gameObject.SetActive(true);
        }
        else if (view == "Armour")
        {
            Armour.gameObject.SetActive(true);
        }
    }

    public void InitFromInventory(Inventory inventory)
    {
        Items.InitFromInventoryItems(inventory);
        Weapons.InitFromInventoryWeapons(inventory);
        Armour.InitFromInventoryArmour(inventory);
    }
}
