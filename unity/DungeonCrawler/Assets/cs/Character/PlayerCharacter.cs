using DungeonCrawler.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerCharacter : BaseCharacter
{
    public static List<PlayerCharacter> PlayerCharacters = new List<PlayerCharacter>();

    public override void Awake()
    {
        base.Awake();
        PlayerCharacters.Add(this);
    }
}
