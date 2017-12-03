using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SlotSystem;
using DungeonCrawler.Character;

public class PlayerUI : MonoBehaviour
{
    [Header("Data")]
    public PlayerParty Party;

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
        MenuPanel.SetActive(false);
    }

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        foreach (CharacterData character in Party.Characters)
        {
            // Connect signals
            character.Data.OnTakenOut += new TakenOutHandler(PlayerGotTakenOut);

            // Add Portraits for each Character
            RectTransform portrait = GameObject.Instantiate<RectTransform>(CharacterPortrait, PortraitsPanel.transform);
            portrait.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 115);

            // Initialize the ItemView
            InventoryView.InitFromInventory(character.Data.Inventory);

            // Connect Signals
            character.Data.Inventory.OnItemAdded += new ItemAddedHandler(ItemAdded);
            character.Data.Inventory.OnItemRemoved += new ItemRemovedHandler(ItemRemoved);
        }
    }

    #region Actions

    public void ToggleMenu()
    {
        MenuPanel.SetActive(!MenuPanel.activeSelf);
    }

    #endregion

    #region Updates
       
    public void ItemAdded(object sender, EventArgs e)
    {
        InventoryView.InitFromInventory(Party.Characters[0].Data.Inventory);
    }

    public void ItemRemoved(object sender, EventArgs e)
    {
        InventoryView.InitFromInventory(Party.Characters[0].Data.Inventory);
    }

    #endregion

    #region Events

    public void PlayerGotTakenOut(object sender, EventArgs e)
    {
        bool allTakenOut = true;
        foreach (CharacterData character in Party.Characters)
        {
            if (!character.Data.IsTakenOut)
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
