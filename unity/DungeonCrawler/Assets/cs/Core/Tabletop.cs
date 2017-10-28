using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonCrawler.Core;
using UnityEngine.Analytics;
using System;

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
        PlayerParty = GameObject.FindObjectsOfType<PlayerCharacter>(); ; 
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
}
