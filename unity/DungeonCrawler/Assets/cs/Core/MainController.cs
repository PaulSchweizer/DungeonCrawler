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
    public GameObject PlayerCharacterPrefab;

    [Header("Internal Data")]
    public string CurrentSceneName;
    public string NextSceneName;

    // Internals
    private string _rootDataPath;
    private string _gameToLoad;
    private string _loadAction;

    private AsyncOperation resourceUnloadTask;
    private AsyncOperation sceneLoadTask;
    private enum SceneState { FadeOut, Preload, Load, Unload, Run, FadeIn, Count };
    private SceneState sceneState;
    private delegate void UpdateDelegate();
    private UpdateDelegate[] updateDelegates;

    private void Awake ()
    {
        DontDestroyOnLoad(transform.gameObject);
        updateDelegates = new UpdateDelegate[(int)SceneState.Count];
        updateDelegates[(int)SceneState.FadeOut] = UpdateSceneFadeOut;
        updateDelegates[(int)SceneState.Preload] = UpdateScenePreload;
        updateDelegates[(int)SceneState.Load] = UpdateSceneLoad;
        updateDelegates[(int)SceneState.Unload] = UpdateSceneUnload;
        updateDelegates[(int)SceneState.FadeIn] = UpdateSceneFadeIn;
        updateDelegates[(int)SceneState.Run] = UpdateSceneRun;
        sceneState = SceneState.Run;
    }

    private void Update()
    {
        updateDelegates[(int)sceneState]();
    }

    #region Actions

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

    private void InitializeGame(string path, string name)
    {
        _rootDataPath = path;
        _gameToLoad = name;

        _loadAction =;

        sceneState = SceneState.FadeOut;
    }

    public void SaveGame(string name)
    {
        GameMaster.RootDataPath = Path.Combine(Application.persistentDataPath, SavedGamesPath);
        GameMaster.SaveCurrentGame(name);
    }

    public void SwitchLocation(string location)
    {
        // 1. Make the PlayerCharacters undeletable
        // 2. FadeOut
        // 3. Unload
        // 4. Load
        // 5. FadeOut
    }

    #endregion

    #region SwitchLocationSequence


    #endregion

    #region InitializationSequence

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

    #region Internals

    private void Fade(int direction)
    {
        FadingCanvas.sortingOrder = 999;
        FadingCanvas.enabled = true;
        FadingScreen.color = new Color(0, 0, 0, FadingScreen.color.a + direction * Time.deltaTime * FadingSpeed);
    }

    #endregion
}
