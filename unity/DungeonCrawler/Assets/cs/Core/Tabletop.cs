using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonCrawler.Core;
using UnityEngine.Analytics;
using System;
using DungeonCrawler.Character;

public class Tabletop : MonoBehaviour
{
    public TextAsset RulebookJsonFile;

    public static PlayerCharacter[] PlayerParty = new PlayerCharacter[] { };

    private void Awake ()
    {
        Rulebook.DeserializeFromJson(RulebookJsonFile.text);
        GameEventsLogger.OnEventLogged += LogAnalytics;
    }

    private void Start()
    {
        PlayerParty = GameObject.FindObjectsOfType<PlayerCharacter>();
        foreach(PlayerCharacter character in PlayerParty)
        {
            character.CharacterData.OnTakenOut += new TakenOutHandler(PlayerGotTakenOut);
        }
        PlayerUI.Instance.Initialize();
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
        foreach(PlayerCharacter character in PlayerParty)
        {
            if (!character.CharacterData.IsTakenOut)
            {
                allTakenOut = false;
                break;
            }
        }

        if(allTakenOut)
        {
            // Disable the Character controls
            InputController.Instance.enabled = false;

            // Show GameOverDialog
            PlayerUI.Instance.gameObject.SetActive(false);
            GameOverUI.Instance.gameObject.SetActive(true);
        }
    }

    #endregion
}
