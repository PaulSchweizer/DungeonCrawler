using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

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

    public void Open(Story story)
    {
        gameObject.SetActive(true);
        InkStory = story;
        story.ChoosePathString("start");
        Next();
    }

    public void Close()
    {
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
}
