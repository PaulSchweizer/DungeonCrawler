using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonCrawler.Core;
using UnityEngine.Analytics;
using System;
using DungeonCrawler.Character;

public class Tabletop : MonoBehaviour
{
    [Header("Data")]
    public PlayerParty Party;

    public static PlayerCharacter[] PlayerParty = new PlayerCharacter[] { };

    private void Awake ()
    {
        GameEventsLogger.OnEventLogged += LogAnalytics;
    }

    private void Start()
    {
        PlayerParty = GameObject.FindObjectsOfType<PlayerCharacter>();
        foreach(CharacterData character in Party.Characters)
        {
            character.Data.OnTakenOut += new TakenOutHandler(PlayerGotTakenOut);
        }
    }

    #region Debug

    #if UNITY_EDITOR

    public bool ShowLogs;

    private void Update()
    {
        if (ShowLogs)
        {
            string log = GameEventsLogger.Next;
            if (log != "")
            {
                Debug.Log(log);
            }
        }
    }

    #endif

    #endregion

    private void LogAnalytics(object sender, GameEventArgs e)
    {
        Analytics.CustomEvent(e.Name, new Dictionary<string, object> { { "details", e.Details} });
    }

    #region Events

    public void PlayerGotTakenOut(object sender, EventArgs e)
    {
        bool allTakenOut = true;
        foreach(CharacterData character in Party.Characters)
        {
            if (!character.Data.IsTakenOut)
            {
                allTakenOut = false;
                break;
            }
        }

        if(allTakenOut)
        {
            // Disable the Character controls
            InputController.Instance.enabled = false;
        }
    }

    #endregion
}
