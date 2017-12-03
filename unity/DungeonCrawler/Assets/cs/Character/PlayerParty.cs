using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParty", menuName = "DungeonCrawler/PlayerParty")]
public class PlayerParty : ScriptableObject
{
    public List<CharacterData> Characters;
}

