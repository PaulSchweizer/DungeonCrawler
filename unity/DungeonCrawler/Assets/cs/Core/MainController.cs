using DungeonCrawler.Core;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainController : MonoBehaviour
{

	void Awake ()
    {
        GameMaster.RootDataPath = Path.Combine(Path.Combine(Path.Combine(Application.dataPath, "json"), "TestData"), "GameData");
        GameMaster.InitializeGame();
    }
}
