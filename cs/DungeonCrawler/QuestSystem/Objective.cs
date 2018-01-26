using DungeonCrawler.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.QuestSystem
{
    public class Objective
    {
        public string Name;
        public List<string> Conditions;
        public bool IsCompleted;

        #region Events

        public event ObjectiveCompletedHandler OnObjectiveCompleted;

        #endregion

        public void CheckProgress()
        {
            bool allConditionsMet = true;
            for (int i = 0; i < Conditions.Count; i++)
            {
                GlobalState.Instance.Conditions.TryGetValue(Conditions[i], out bool value);
                if (!value)
                {
                    allConditionsMet = false;
                    break;
                }
            }

            if (allConditionsMet)
            {
                CompleteSuccessfully();
            }
        }

        public void CompleteSuccessfully()
        {
            IsCompleted = true;
            OnObjectiveCompleted?.Invoke(this, null);
        }
    }
}
