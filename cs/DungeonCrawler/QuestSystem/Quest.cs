using DungeonCrawler.Core;
using DungeonCrawler.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DungeonCrawler.QuestSystem
{
    #region Delegates

    public delegate void QuestStartedHandler(object sender, QuestStatusChangedEventArgs e);
    public delegate void QuestCompletedHandler(object sender, QuestStatusChangedEventArgs e);
    public delegate void ObjectiveCompletedHandler(object sender, EventArgs e);

    #endregion

    public class Quest
    {
        public enum States { Inactive, Active, Completed };
        public string Name;
        public string Description;
        public Dictionary<string, bool> Requirements;
        public States State;
        public int XP;
        public List<Objective> Objectives;

        #region Events

        public event QuestStartedHandler OnQuestStarted;
        public event QuestCompletedHandler OnQuestCompleted;

        #endregion

        public void Start()
        {
            State = States.Active;
            OnQuestStarted?.Invoke(this, new QuestStatusChangedEventArgs(this));
        }

        public void CheckProgress()
        {
            bool allObjectivesCompleted = true;
            for (int i = 0; i < Objectives.Count; i++)
            {
                Objectives[i].CheckProgress();
                if (!Objectives[i].IsCompleted)
                {
                    allObjectivesCompleted = false;
                    break;
                }
            }

            if (allObjectivesCompleted)
            {
                CompleteSuccessfully();
            }
        }

        public void CompleteSuccessfully()
        {
            State = States.Completed;
            OnQuestCompleted?.Invoke(this, new QuestStatusChangedEventArgs(this));
        }

        #region Serialization

        public static Quest DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Quest>(json);
        }

        public static string SerializeToJson(Quest quest)
        {
            string json = JsonConvert.SerializeObject(quest, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.Indentation = 4;
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }

        #endregion

    }

    public class QuestStatusChangedEventArgs : EventArgs
    {
        public Quest Quest;
        
        public QuestStatusChangedEventArgs(Quest quest)
        {
            Quest = quest;
        }
    }
}