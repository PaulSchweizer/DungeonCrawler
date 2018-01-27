using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonCrawler.Character;
using Ink.Runtime;

public class NPCCharacter : MonoBehaviour
{
    public string Name;
    public Sprite Portrait;

    public string StoryName;

    public Story InkStory
    {
        get
        {
            return StoryController.Instance.GetStory(StoryName);
        }
    }

    //public TextAsset InkData;

    //public Story InkStory;

    //private void Awake()
    //{
    //    InkStory = new Story(InkData.text);
    //}

    //private void OnDestroy()
    //{
    //    //SerializeField the story
    //    //InkStory.ToJsonString
    //}
}
