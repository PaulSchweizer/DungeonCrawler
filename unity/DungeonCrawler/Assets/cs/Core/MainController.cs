using DungeonCrawler.Character;
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
    public static MainController Instance;

    [Header("Game Data")]
    //public string GameDataPath = "json/TestData/GameData";
    //public string SavedGamesPath = "SavedGames";
    //public string DefaultGamePath = "json/TestData/GameData";
    //public string DefaultGame = "TestSave";
    public JsonDatabase GameData;
    public JsonSavedGame DefaultGame;

    [Header("Fading Screen")]
    public Canvas FadingCanvas;
    public Image FadingScreen;
    public float FadingSpeed = 3;
    public Text LoadingText;

    [Header("Prefabs")]
    public GameObject TabletopPrefab;
    public GameObject PlayerCharacterPrefab;

    [Header("Internal Data")]
    public string CurrentSceneName;
    public string NextSceneName;

    // Internals
    private string _rootDataPath;
    private string _gameToLoad;
    private string _locationToLoad;
    private LoadAction _loadAction;

    private AsyncOperation resourceUnloadTask;
    private AsyncOperation sceneLoadTask;
    private enum SceneState { FadeOut, Preload, Load, Unload, Run, FadeIn, Count };
    private SceneState sceneState;
    private delegate void UpdateDelegate();
    private UpdateDelegate[] updateDelegates;

    private enum LoadAction { StartGame, LoadLocation, Count };
    private delegate void LoadActionDelegate();
    private LoadActionDelegate[] loadActionDelegates;

    private void Awake ()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
        updateDelegates = new UpdateDelegate[(int)SceneState.Count];
        updateDelegates[(int)SceneState.FadeOut] = UpdateSceneFadeOut;
        updateDelegates[(int)SceneState.Preload] = UpdateScenePreload;
        updateDelegates[(int)SceneState.Load] = UpdateSceneLoad;
        updateDelegates[(int)SceneState.Unload] = UpdateSceneUnload;
        updateDelegates[(int)SceneState.FadeIn] = UpdateSceneFadeIn;
        updateDelegates[(int)SceneState.Run] = UpdateSceneRun;
        sceneState = SceneState.Run;

        loadActionDelegates = new LoadActionDelegate[(int)LoadAction.Count];
        loadActionDelegates[(int)LoadAction.StartGame] = StartGame;
        loadActionDelegates[(int)LoadAction.LoadLocation] = LoadLocation;
        _loadAction = LoadAction.StartGame;
    }

    #region Actions

    public void NewGame()
    {
        NextSceneName = "LevelTemplate";
        InitializeGame(DefaultGame); // Path.Combine(Application.dataPath, DefaultGamePath), DefaultGame);
    }

    public void LoadGame(string name)
    {
        NextSceneName = "LevelTemplate";
        //
        // Use the TextAsset on the ScriptabelObject to create a struct of strings that hold all the json information 
        // for the game, this goes for loading games
        //
        //InitializeGame(Application.persistentDataPath, name);
    }

    public void SaveGame(string name)
    {
        //
        // Find the correct place to store save data
        //
        //GameMaster.RootDataPath = Path.Combine(Application.persistentDataPath, SavedGamesPath);
        GameMaster.SaveCurrentGame(name);
    }

    private void InitializeGame(JsonSavedGame savedGame)
    {
        //_rootDataPath = path;
        //_gameToLoad = name;
        _loadAction = LoadAction.StartGame;
        sceneState = SceneState.FadeOut;
    }

    public static void SwitchLocation(string location)
    {
        // 1. Make the PlayerCharacters undeletable

        // 2. FadeOut
        Instance.NextSceneName = "LevelTemplate";
        Instance.sceneState = SceneState.FadeOut;

        // Load the next Location
        Instance._locationToLoad = location;
        Instance._loadAction = LoadAction.LoadLocation;
    }

    #endregion

    #region InitializationSequence

    private void Update()
    {
        updateDelegates[(int)sceneState]();
    }

    private void UpdateSceneFadeOut()
    {
        if (FadingScreen.color.a < 1)
        {
            Fade(1);
        }
        else
        {
            sceneState = SceneState.Preload;
        }
    }

    private void UpdateScenePreload()
    {
        GC.Collect();
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
        loadActionDelegates[(int)_loadAction]();
        sceneState = SceneState.FadeIn;
    }

    private void UpdateSceneFadeIn()
    {
        if (FadingScreen.color.a > 0)
        {
            Fade(-1);
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

    #region Load Actions

    private void StartGame()
    {
        //
        // Exchange to feed the GameMaster with the contents of the TextAssets instead!
        //
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

        // Camera
        CameraRig camera = FindObjectOfType<CameraRig>();

        // Load the PCs  
        List<Character> players = GameMaster.CharactersOfType("Player");
        foreach (Character character in players)
        {
            GameObject player = Instantiate(PlayerCharacterPrefab);
            PlayerCharacter playerCharacter = player.GetComponent<PlayerCharacter>();
            playerCharacter.Character.Data = character;
            camera.Target = player.transform;
        }
    }

    private void LoadLocation()
    {
        Debug.Log("Switching Location to: " + _locationToLoad);
    }

    #endregion

    #region Utility

    private void Fade(int direction)
    {
        FadingCanvas.sortingOrder = 999;
        FadingCanvas.enabled = true;
        FadingScreen.color = new Color(0, 0, 0, FadingScreen.color.a + direction * Time.deltaTime * FadingSpeed);
    }

    #endregion
}
