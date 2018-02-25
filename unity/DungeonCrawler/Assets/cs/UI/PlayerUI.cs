using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SlotSystem;
using DungeonCrawler.Character;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

    [Header("HUD")]
    public GameObject MenuPanel;
    public RectTransform PortraitsPanel;

    [Header("Main Menu")]
    public GameObject CharacterButton;
    public GameObject SkillsButton;
    public GameObject InventoryButton;
    public GameObject QuestsButton;
    public InventoryUI InventoryView;

    [Header("Prefabs")]
    public RectTransform CharacterPortrait;

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
        MenuPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        Instance = null;
        foreach (PlayerCharacter player in PlayerCharacter.PlayerCharacters)
        {
            player.Data.OnTakenOut -= new TakenOutHandler(PlayerGotTakenOut);
            player.Data.Inventory.OnItemAdded -= new ItemAddedHandler(ItemAdded);
            player.Data.Inventory.OnItemRemoved -= new ItemRemovedHandler(ItemRemoved);
        }
    }

    public void Initialize()
    {
        foreach (PlayerCharacter player in PlayerCharacter.PlayerCharacters)
        {
            // Connect signals
            player.Data.OnTakenOut += new TakenOutHandler(PlayerGotTakenOut);

            // Add Portraits for each Character
            RectTransform portrait = GameObject.Instantiate<RectTransform>(CharacterPortrait, PortraitsPanel.transform);
            portrait.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 115);
            portrait.GetComponent<CharacterPortraitUI>().Initialize(player);

            // Initialize the ItemView
            InventoryView.InitFromInventory(player.Data.Inventory);

            // Connect Signals
            player.Data.Inventory.OnItemAdded += new ItemAddedHandler(ItemAdded);
            player.Data.Inventory.OnItemRemoved += new ItemRemovedHandler(ItemRemoved);
        }
    }

    #region Actions

    public void ToggleMenu()
    {
        MenuPanel.SetActive(!MenuPanel.activeSelf);
    }

    #endregion

    #region Updates
       
    public void ItemAdded(object sender, ItemAddedEventArgs e)
    {
        HUDMessageUI.Instance.PushMessage(new MessageStruct("Found Item", string.Format("{0} {1}", e.Amount, e.Item.Name)));
        InventoryView.InitFromInventory(PlayerCharacter.PlayerCharacters[0].Data.Inventory);
    }

    public void ItemRemoved(object sender, EventArgs e)
    {
        InventoryView.InitFromInventory(PlayerCharacter.PlayerCharacters[0].Data.Inventory);
    }

    #endregion

    #region Events

    public void PlayerGotTakenOut(object sender, EventArgs e)
    {
        bool allTakenOut = true;
        foreach (PlayerCharacter player in PlayerCharacter.PlayerCharacters)
        {
            if (!player.Data.IsTakenOut)
            {
                allTakenOut = false;
                break;
            }
        }

        if (allTakenOut)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion
}
