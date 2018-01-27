using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SlotSystem;
using DungeonCrawler.Character;

public class GameOverUI : MonoBehaviour
{

    public static GameOverUI Instance;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        foreach (PlayerCharacter player in PlayerCharacter.PlayerCharacters)
        {
            player.Character.Data.OnTakenOut += new TakenOutHandler(PlayerGotTakenOut);
        }
    }

    #region Actions

    #endregion

    #region Events

    public void PlayerGotTakenOut(object sender, EventArgs e)
    {
        bool allTakenOut = true;
        foreach (PlayerCharacter player in PlayerCharacter.PlayerCharacters)
        {
            if (!player.Character.Data.IsTakenOut)
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
