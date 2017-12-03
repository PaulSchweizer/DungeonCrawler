using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SlotSystem;
using DungeonCrawler.Character;

public class GameOverUI : MonoBehaviour
{
    [Header("Data")]
    public PlayerParty Party;

    protected void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Start()
    {
        foreach (CharacterData character in Party.Characters)
        {
            character.Data.OnTakenOut += new TakenOutHandler(PlayerGotTakenOut);
        }
    }

    #region Actions

    #endregion

    #region Events

    public void PlayerGotTakenOut(object sender, EventArgs e)
    {
        bool allTakenOut = true;
        foreach (CharacterData character in Party.Characters)
        {
            if (!character.Data.IsTakenOut)
            {
                allTakenOut = false;
                break;
            }
        }

        if (allTakenOut)
        {
            gameObject.SetActive(true);
        }
    }

    #endregion
}
