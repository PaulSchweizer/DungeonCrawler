using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "JsonSavedGame", menuName = "DungeonCrawler/JsonSavedGame")]
public class JsonSavedGame : ScriptableObject
{
    [Header("PCs")]
    public TextAsset[] PCs;

    [Header("SavedState")]
    public TextAsset GameState;
    public TextAsset GlobalState;
}
