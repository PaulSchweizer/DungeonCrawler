using DungeonCrawler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public Dictionary<string, int> Items;

    public bool RemoveItemsOnCollect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectLoot();
        }
    }

    private void CollectLoot()
    {
        List<string> keys = new List<string>(Items.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            PlayerCharacter.PlayerCharacters[0].Character.Data.Inventory.AddItem(Rulebook.Item(keys[i]), Items[keys[i]]);
            if (RemoveItemsOnCollect)
            {
                Items.Remove(keys[i]);
            }
        }
        gameObject.SetActive(false);
    }
}
