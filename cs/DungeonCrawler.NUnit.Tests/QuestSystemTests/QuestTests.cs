using DungeonCrawler.Core;
using DungeonCrawler.QuestSystem;
using NUnit.Framework;
using System;

namespace DungeonCrawler.NUnit.Tests.QuestSystemTests
{
    [TestFixture]
    public class QuestSystemTests
    {
        Quest quest;

        [SetUp]
        public void SetUp()
        {
            quest = Utilities.Quest();
        }

        [Test]
        public void Start_quest()
        {
            quest.Start();
            Assert.AreEqual(Quest.States.Active, quest.State);
        }

        [Test]
        public void CheckProgress_finishes_quest_if_applicable()
        {
            quest.Start();
            quest.CheckProgress();
            Assert.AreEqual(Quest.States.Active, quest.State);

            foreach(Objective objective in quest.Objectives)
            {
                foreach(string condition in objective.Conditions)
                {
                    GlobalState.Instance.Conditions[condition] = true;
                }
            }

            quest.CheckProgress();
            Assert.AreEqual(Quest.States.Completed, quest.State);
        }
    }
}