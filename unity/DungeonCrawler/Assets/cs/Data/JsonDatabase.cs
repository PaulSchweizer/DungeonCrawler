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
    public TextAsset[] Locations;

    [Header("Characters")]
    public TextAsset[] Monsters;
    public TextAsset[] PCs;
    public TextAsset[] Skills;

    [Header("Global")]
    public TextAsset GameMaster;
    public TextAsset GlobalState;
    public TextAsset Rulebook;

}
