using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using DungeonCrawler.Core;
using DungeonCrawler.QuestSystem;

public class ConversationUI : MonoBehaviour
{ 
    public Image Portrait;
    public Text Name;
    public Text Dialog;
    public Button[] Choices;
    public Story InkStory;

    public static ConversationUI Instance;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(false);
    }

    public void Open(Story story, NPCCharacter NPC)
    {
        gameObject.SetActive(true);
        InputController.Instance.enabled = false;

        // Prepare the UI
        Name.text = NPC.Name;
        Portrait.sprite = NPC.Portrait;

        // Story
        InkStory = story;
        story.ChoosePathString("start");
        Next();
    }

    public void Close()
    {
        InputController.Instance.enabled = true;
        gameObject.SetActive(false);
        PlayerCharacter[] pcs = GameObject.FindObjectsOfType<PlayerCharacter>();
        for (int i = 0; i < pcs.Length; i++)
        {
            PlayerCharacter pc = pcs[i];
            pc.ChangeState(pc.Idle);
        }
    }

    public void Next()
    {
        // Reset the options buttons
        for (int i = 0; i < Choices.Length; ++i)
        {
            Choices[i].gameObject.SetActive(false);
        }
        Dialog.text = "";
        while (InkStory.canContinue)
        {
            Dialog.text += InkStory.Continue();
        }

        // Put any global variables from the Story back to the GlobalState
        //
        foreach (string variable in InkStory.variablesState)
        {
            bool value;
            if (GlobalState.Instance.Conditions.TryGetValue(variable, out value))
            {
                GlobalState.Instance.Conditions[variable] = (int)InkStory.variablesState[variable] == 1;
            }
        }
        
        // Send out any Signals if certain Tags are present in the Story
        //
        foreach (string tag in InkStory.currentTags)
        {
            string[] parts = tag.Split(':');
            if (parts.Length == 3)
            {
                if (parts[0] == "Signal" && parts[1] == "StartQuest")
                {
                    StartQuest(parts[2]);
                }
            }
        }

        if (InkStory.currentChoices.Count > 0)
        {
            for (int i = 0; i< InkStory.currentChoices.Count; ++i)
            {
                Choice choice = InkStory.currentChoices[i];
                Choices[i].GetComponentInChildren<Text>().text = choice.text;
                Choices[i].gameObject.SetActive(true);
            }
        }
        else
        {
            Close();
        }
    }

    public void Choose(int index)
    {
        InkStory.ChooseChoiceIndex(index);
        Next();
    }

    private void StartQuest(string quest)
    {
        Rulebook.Instance.Quests[quest].Start();
    }
}
