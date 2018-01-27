using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "JsonDatabase", menuName = "DungeonCrawler/JsonDatabase")]
public class JsonDatabase : ScriptableObject
{
    [Header("Items")]
    public TextAsset[] Armour;
    public TextAsset[] Items;
    public TextAsset[] Weapons;

    [Header("Locations")]
    public TextAsset[] CellBlueprints;
    public TextAsset[] Locations;

    [Header("Characters")]
    public TextAsset[] Monsters;
    public TextAsset[] PCs;
    public TextAsset[] Skills;

    [Header("Quests")]
    public TextAsset[] Quests;

    [Header("Global")]
    public TextAsset GlobalState;
    public TextAsset Rulebook;

    #region Data

    public string[] ArmoursData
    {
        get
        {
            string[] data = new string[Armour.Length];
            for (int i = 0; i < Armour.Length; i++)
            {
                data[i] = Armour[i].text;
            }
            return data;
        }
    }

    public string[] ItemsData
    {
        get
        {
            string[] data = new string[Items.Length];
            for (int i = 0; i < Items.Length; i++)
            {
                data[i] = Items[i].text;
            }
            return data;
        }
    }

    public string[] WeaponsData
    {
        get
        {
            string[] data = new string[Weapons.Length];
            for (int i = 0; i < Weapons.Length; i++)
            {
                data[i] = Weapons[i].text;
            }
            return data;
        }
    }

    public string[] CellBlueprintsData
    {
        get
        {
            string[] data = new string[CellBlueprints.Length];
            for (int i = 0; i < CellBlueprints.Length; i++)
            {
                data[i] = CellBlueprints[i].text;
            }
            return data;
        }
    }

    public string[] LocationsData
    {
        get
        {
            string[] data = new string[Locations.Length];
            for (int i = 0; i < Locations.Length; i++)
            {
                data[i] = Locations[i].text;
            }
            return data;
        }
    }

    public string[] MonstersData
    {
        get
        {
            string[] data = new string[Monsters.Length];
            for (int i = 0; i < Monsters.Length; i++)
            {
                data[i] = Monsters[i].text;
            }
            return data;
        }
    }

    public string[] PCsData
    {
        get
        {
            string[] data = new string[PCs.Length];
            for (int i = 0; i < PCs.Length; i++)
            {
                data[i] = PCs[i].text;
            }
            return data;
        }
    }

    public string[] SkillsData
    {
        get
        {
            string[] data = new string[Skills.Length];
            for (int i = 0; i < Skills.Length; i++)
            {
                data[i] = Skills[i].text;
            }
            return data;
        }
    }

    public string[] QuestsData
    {
        get
        {
            string[] data = new string[Quests.Length];
            for (int i = 0; i < Quests.Length; i++)
            {
                data[i] = Quests[i].text;
            }
            return data;
        }
    }

    public string GlobalStateData
    {
        get
        {
            return GlobalState.text;
        }
    }

    public string RulebookData
    {
        get
        {
            return Rulebook.text;
        }
    }

    #endregion
}
