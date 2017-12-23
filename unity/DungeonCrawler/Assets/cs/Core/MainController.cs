using DungeonCrawler.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    [Header("Game Data")]
    public string GameDataPath = "json/TestData/GameData";
    public string SavedGamesPath = "SavedGames";
    public string DefaultGamePath = "json/TestData/GameData";
    public string DefaultGame = "TestSave";

    [Header("Fading Screen")]
    public Canvas FadingCanvas;
    public Image FadingScreen;
    public float FadingSpeed = 3;
    public Text LoadingText;

    [Header("Prefabs")]
    public GameObject TabletopPrefab;

    [Header("Internal Data")]
    public string CurrentSceneName;
    public string NextSceneName;

    private string _rootDataPath;
    private string _gameToLoad;

    private AsyncOperation resourceUnloadTask;
    private AsyncOperation sceneLoadTask;
    private enum SceneState { FadeOut, Reset, Preload, Load, Unload, Postload, Ready, Run, FadeIn, Count };
    private SceneState sceneState;
    private delegate void UpdateDelegate();
    private UpdateDelegate[] updateDelegates;

    void Awake ()
    {
        DontDestroyOnLoad(transform.gameObject);
        updateDelegates = new UpdateDelegate[(int)SceneState.Count];
        updateDelegates[(int)SceneState.FadeOut] = UpdateSceneFadeOut;
        updateDelegates[(int)SceneState.Reset] = UpdateSceneReset;
        updateDelegates[(int)SceneState.Preload] = UpdateScenePreload;
        updateDelegates[(int)SceneState.Load] = UpdateSceneLoad;
        updateDelegates[(int)SceneState.Unload] = UpdateSceneUnload;
        updateDelegates[(int)SceneState.Postload] = UpdateScenePostload;
        updateDelegates[(int)SceneState.Ready] = UpdateSceneReady;
        updateDelegates[(int)SceneState.FadeIn] = UpdateSceneFadeIn;
        updateDelegates[(int)SceneState.Run] = UpdateSceneRun;
        sceneState = SceneState.Ready;
    }

    public void NewGame()
    {
        NextSceneName = "LevelTemplate";
        InitializeGame(Path.Combine(Application.dataPath, DefaultGamePath), DefaultGame);
    }

    public void LoadGame(string name)
    {
        NextSceneName = "LevelTemplate";
        InitializeGame(Application.persistentDataPath, name);
    }

    public void SaveGame(string name)
    {
        GameMaster.RootDataPath = Path.Combine(Application.persistentDataPath, SavedGamesPath);
        GameMaster.SaveCurrentGame(name);
    }

# region InitializeGame

    private void InitializeGame(string path, string name)
    {
        _rootDataPath = path;
        _gameToLoad = name;

        sceneState = SceneState.FadeOut;
    }

    protected void Update()
    {
        updateDelegates[(int)sceneState]();
    }

    private void UpdateSceneFadeOut()
    {
        if (FadingScreen.color.a < 1)
        {
            FadingCanvas.sortingOrder = 999;
            FadingCanvas.enabled = true;
            FadingScreen.color = new Color(0, 0, 0, FadingScreen.color.a + Time.deltaTime * FadingSpeed);
        }
        else
        {
            sceneState = SceneState.Reset;
        }
    }

    private void UpdateSceneReset()
    {
        GC.Collect();
        sceneState = SceneState.Preload;
    }

    private void UpdateScenePreload()
    {
        sceneLoadTask = SceneManager.LoadSceneAsync(NextSceneName);
        sceneState = SceneState.Unload;
        if (sceneLoadTask.isDone)
        {
            sceneState = SceneState.Unload;
        }
        else
        {
            // Update some scene loading progress bar
        }
    }

    private void UpdateSceneUnload()
    {
        if (resourceUnloadTask == null)
        {
            resourceUnloadTask = Resources.UnloadUnusedAssets();
        }
        else
        {
            if (resourceUnloadTask.isDone)
            {
                resourceUnloadTask = null;
                sceneState = SceneState.Load;
            }
        }
    }

    private void UpdateSceneLoad()
    {
        GameMaster.RootDataPath = Path.Combine(Application.dataPath, GameDataPath); 
        GameMaster.InitializeGame();

        // Load the Game
        GameMaster.RootDataPath = _rootDataPath;
        GameMaster.LoadGame(_gameToLoad);

        //  Init the Tabletop and the Location
        GameObject tabletop = Instantiate<GameObject>(TabletopPrefab);
        Level level = tabletop.GetComponent<Level>();
        level.Location = GameMaster.CurrentLocation;
        level.Create();

        // Entrance Points

        // Load the Monsters
        
        // Load the PCs

        sceneState = SceneState.Postload;
    }

    private void UpdateScenePostload()
    {
        sceneState = SceneState.Ready;
    }

    private void UpdateSceneReady()
    {
        sceneState = SceneState.FadeIn;
    }

    private void UpdateSceneFadeIn()
    {
        if (FadingScreen.color.a > 0)
        {
            FadingScreen.color = new Color(0, 0, 0, FadingScreen.color.a - Time.deltaTime * FadingSpeed);
        }
        else
        {
            FadingCanvas.enabled = false;
            sceneState = SceneState.Run;
        }
    }

    private void UpdateSceneRun()
    {
    }

    #endregion
}
