using UnityEngine;
using UnityEngine.EventSystems;

namespace SlotSystem
{
    public class Slot : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        public SlottableItem Item;

        public bool AllowsDrag = true;
        public string AcceptedItemType;

        public void OnDrop(PointerEventData eventData)
        {
            Swap(SlottableItem.DraggedItem);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsFree)
            {
                OnClicked();
            }
        }

        public virtual void OnClicked() { }

        public bool AcceptsItem(SlottableItem item)
        {
            if (AcceptedItemType == "" || AcceptedItemType == item.Item.GetType().Name)
            {
                return true;
            }
            return false;
        }

        public bool IsFree
        {
            get
            {
                return Item == null;
            }
        }

        public void Swap(SlottableItem item)
        {
            var currentItem = Item;
            var draggedItem = item;
            if (draggedItem == null || currentItem == draggedItem)
            {
                return;
            }
            var oldSlot = draggedItem.Slot;

            if (IsFree && AcceptsItem(draggedItem) && draggedItem.AcceptsSlot(this))
            {
                Drop(draggedItem);
                oldSlot.Clear();
                return;
            }

            // Items need to be swapped
            if (AcceptsItem(draggedItem) && oldSlot.AcceptsItem(currentItem) &&
                draggedItem.AcceptsSlot(this) && currentItem.AcceptsSlot(oldSlot))
            {
                Drop(draggedItem);
                oldSlot.Drop(currentItem);
            }
        }

        public void Drop(SlottableItem item)
        {
            var oldItem = Item;
            var newItem = item;
            item.transform.SetParent(transform);
            item.FitIntoSlot();
            ExecuteEvents.ExecuteHierarchy<ISlotChanged>(gameObject, null, (x, y) => x.SlotChanged(this, newItem, oldItem));
            Item = item;
            Item.Slot = this;
        }

        public void Clear()
        {
            var oldItem = Item;
            Item = null;
            ExecuteEvents.ExecuteHierarchy<ISlotChanged>(gameObject, null, (x, y) => x.SlotChanged(this, null, oldItem));
        }
    }

    public interface ISlotChanged : IEventSystemHandler
    {
        void SlotChanged(Slot slot, SlottableItem newItem, SlottableItem oldItem);
    }
}