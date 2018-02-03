using DungeonCrawler.Core;
using DungeonCrawler.QuestSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HUDMessageUI : MonoBehaviour
{
    public static HUDMessageUI Instance;

    public int ShowTime;
    public Text Headline;
    public Text Content;

    private List<MessageStruct> _messages = new List<MessageStruct>();
    private int _currentIndex;
    private float _endTime;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        Instance = null;
        foreach (KeyValuePair<string, Quest> entry in Rulebook.Instance.Quests)
        {
            entry.Value.OnQuestStarted -= new QuestStartedHandler(OnQuestStarted);
            entry.Value.OnQuestCompleted -= new QuestCompletedHandler(OnQuestCompleted);
        }
    }

    public void Initialize()
    {
        foreach(KeyValuePair<string, Quest> entry in Rulebook.Instance.Quests)
        {
            entry.Value.OnQuestStarted += new QuestStartedHandler(OnQuestStarted);
            entry.Value.OnQuestCompleted += new QuestCompletedHandler(OnQuestCompleted);
        }
    }

    public void PushMessage(MessageStruct message)
    {
        _messages.Add(message);
        _endTime += ShowTime;
        if (!gameObject.activeSelf)
        {
            Show();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _currentIndex = 0;
        _endTime += Time.time;
    }

    public void Close()
    {
        gameObject.SetActive(false);
        _endTime = 0;
        _messages.Clear();
    }

    private void Update()
    {
        if (_messages.Count == 0)
        {
            return;
        }
        if (Time.time >= _endTime)
        {
            Close();
            return;
        }
        int index = (int)Math.Floor(_messages.Count - (_endTime - Time.time) / ShowTime);
        Headline.text = _messages[index].Headline;
        Content.text = _messages[index].Content;
    }

    #region Events

    private void OnQuestStarted(object sender, QuestStatusChangedEventArgs e)
    {
        PushMessage(new MessageStruct("New Quest", e.Quest.Name));
    }

    private void OnQuestCompleted(object sender, QuestStatusChangedEventArgs e)
    {
        PushMessage(new MessageStruct("Quest Completed", e.Quest.Name));
    }

    #endregion

}


public struct MessageStruct
{
    public string Headline;
    public string Content;

    public MessageStruct(string headline, string content)
    {
        Headline = headline;
        Content = content;
    }
}
