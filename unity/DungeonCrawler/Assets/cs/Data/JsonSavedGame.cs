using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "JsonSavedGame", menuName = "DungeonCrawler/JsonSavedGame")]
public class JsonSavedGame : ScriptableObject
{
    [Header("PCs")]
    public TextAsset[] PCs;

    [Header("SavedState")]
    public string Location;
    public TextAsset GlobalState;

    // Internals to override the TextAssets
    private string[] _pcsData;
    private string _globalStateData;

    private void OnEnable()
    {
        _pcsData = null;
        _globalStateData = null;
    }

    public string[] PCsData
    {
        get
        {
            if (_pcsData != null)
            {
                return _pcsData;
            }
            else
            {
                string[] pcs = new string[PCs.Length];
                for (int i = 0; i < PCs.Length; i++)
                {
                    pcs[i] = PCs[i].text;
                }
                return pcs;
            }
        }
        set
        {
            _pcsData = value;
        }
    }

    public string GlobalStateData
    {
        get
        {
            if (_globalStateData != null)
            {
                return _globalStateData;
            }
            else
            {
                return GlobalState.text;
            }
        }
        set
        {
            _globalStateData = value;
        }
    }

    //public void LoadData()
    //{
    //    //string rootPath = Path.Combine(GameMaster.RootDataPath, name);
    //    //GameState gameState = DeserializeFromJson(File.ReadAllText(Path.Combine(rootPath, "GameState.json")));
    //    //string json = "";
    //    //foreach (string path in gameState.PlayerCharacters)
    //    //{
    //    //    json = File.ReadAllText(Path.Combine(GameMaster.RootDataPath, string.Format("{0}.json", path)));
    //    //    GameMaster.RegisterCharacter(Character.Character.DeserializeFromJson(json));
    //    //}
    //    //json = File.ReadAllText(Path.Combine(Path.Combine(GameMaster.RootDataPath, "Locations"), string.Format("{0}.json", gameState.Location)));
    //    //GameMaster.CurrentLocation = Core.Location.DeserializeFromJson(json);
    //}
}
