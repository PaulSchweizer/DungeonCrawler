using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonCrawler.Character;
using Ink.Runtime;

public class NPCCharacter : MonoBehaviour
{
    public TextAsset InkData;

    [HideInInspector]
    public Story InkStory;

    void Awake()
    {
        InkStory = new Story(InkData.text);
    }
}
