using DungeonCrawler.Core;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StoryController : MonoBehaviour
{
	public static StoryController Instance;

    public TextAsset[] Stories;
    public string[] StoryNames;

    private Dictionary<string, Story> _initializedStories;

    private void Awake()
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
        _initializedStories = new Dictionary<string, Story>();
    }

    public Story GetStory(string name)
    {
        Story story = null;

        if (!_initializedStories.TryGetValue(name, out story))
        {
			InitializeStory(name);
            _initializedStories.TryGetValue(name, out story);
        }

        // Init any global variables that are on the GlobalState and present in the Story to their GlobalState value
        //
        foreach(KeyValuePair<string, bool> entry in GlobalState.Instance.Conditions)
        {
            if (story.variablesState.Contains<string>(entry.Key))
            {
                story.variablesState[entry.Key] = entry.Value;
            }
        }

        return story;
    }

    private void InitializeStory(string name)
    {
        _initializedStories[name] = new Story(Stories[Array.IndexOf(StoryNames, name)].text);
    }
}
