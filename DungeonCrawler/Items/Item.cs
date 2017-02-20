using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Items
{
    public interface IItem
    {
        void Use();
    }

    public class Item : IItem
    {
        public int Id;

        public string Name;

        public string Description;

        public float Price;

        public bool QuestItem;

        public override string ToString()
        {
            return string.Format("{0} | {1} | {2} | {3} | {4}", Id, Name, Description, Price, QuestItem);
        }

        public void Use()
        {
            throw new NotImplementedException();
        }
    }
}
