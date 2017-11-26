using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DungeonCrawler.Core;

namespace SlotSystem
{

    [RequireComponent(typeof(CanvasGroup))]
    public class SlottableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static SlottableItem DraggedItem;

        // Data
        public Item Item;

        // UI
        public int Amount;
        public Text Name;
        public Text AmountText;
        public Image Image;
        public Image Background;
        public Slot Slot;

        // Internals
        private Slot _startSlot;
        private RectTransform _rect;
        private CanvasGroup _canvasGroup;
        private Canvas _canvas;

        void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            FitIntoSlot();
        }

        public void FitIntoSlot()
        {
            _rect.pivot = new Vector2(0, 1);
            _rect.anchorMin = new Vector2(0, 0);
            _rect.anchorMax = new Vector2(1, 1);
            transform.localScale = new Vector3(1, 1, 1);
            _rect.anchoredPosition3D = new Vector3(0, 0, 0);
            _rect.offsetMin = new Vector2(0, 0);
            _rect.offsetMax = new Vector2(0, 0);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!Slot.AllowsDrag)
            {
                return;
            }
            _canvas = GetComponentInParent<Canvas>();
            DraggedItem = this;
            _startSlot = Slot;
            _canvasGroup.blocksRaycasts = false;
            transform.SetParent(GetComponentInParent<Canvas>().transform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!Slot.AllowsDrag)
            {
                return;
            }
            var width = (_rect.rect.width * _canvas.scaleFactor) * 0.5f;
            var height = (_rect.rect.height * _canvas.scaleFactor) * 0.5f;
            transform.position = new Vector3(Input.mousePosition.x - width,
                                             Input.mousePosition.y + height,
                                             1);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!Slot.AllowsDrag)
            {
                return;
            }
            DraggedItem = null;
            _canvasGroup.blocksRaycasts = true;
            if (Slot == _startSlot)
            {
                Slot.Drop(this);
            }
        }

        public bool AcceptsSlot(Slot slot)
        {
            return true;
        }

        public void Init(Item item, int amount)
        {
            Item = item;
            Amount = amount;
            UpdateDisplay();
            gameObject.SetActive(true);
        }

        public void UpdateDisplay()
        {
            Name.text = Item.Name;
            AmountText.text = Amount.ToString();

            //
            // Load Item image from resources
            //
            //Image.sprite = Item.Sprite;
        }
    }
}
