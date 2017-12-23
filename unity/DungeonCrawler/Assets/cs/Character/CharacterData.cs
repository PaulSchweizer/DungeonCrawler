using DungeonCrawler.Character;
using DungeonCrawler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "DungeonCrawler/Character")]
public class CharacterData : ScriptableObject
{
    public TextAsset JsonFile;
    public Character Data;

    private void OnEnable()
    {
        Data = Character.DeserializeFromJson(JsonFile.text);
        if(Data.Type != "Player")
        {
            GameMaster.RegisterCharacter(Data);
        }
    }
}
