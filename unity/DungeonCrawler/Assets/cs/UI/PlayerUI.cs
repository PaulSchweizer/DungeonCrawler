using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SlotSystem;
using DungeonCrawler.Character;

public class PlayerUI : MonoBehaviour
{
    // HUD
    [Header("HUD")]
    public GameObject MenuPanel;
    public RectTransform PortraitsPanel;

    // Main Menu
    [Header("Main Menu")]
    public GameObject CharacterButton;
    public GameObject SkillsButton;
    public GameObject InventoryButton;
    public GameObject QuestsButton;

    // Main Menu
    public InventoryUI InventoryView;

    // Prefabs
    [Header("Prefabs")]
    public RectTransform CharacterPortrait;

    // Internals
    public static PlayerUI Instance;

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

    public void Initialize()
    {
        foreach (PlayerCharacter pc in Tabletop.PlayerParty)
        {
            // Add Portraits for each Character
            RectTransform portrait = GameObject.Instantiate<RectTransform>(CharacterPortrait, PortraitsPanel.transform);
            portrait.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 115);

            // Initialize the ItemView
            InventoryView.InitFromInventory(pc.CharacterData.Inventory);

            // Connect Signals
            pc.CharacterData.Inventory.OnItemAdded += new ItemAddedHandler(ItemAdded);
            pc.CharacterData.Inventory.OnItemRemoved += new ItemRemovedHandler(ItemRemoved);
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
        InventoryView.InitFromInventory(Tabletop.PlayerParty[0].CharacterData.Inventory);
    }

    public void ItemRemoved(object sender, EventArgs e)
    {
        InventoryView.InitFromInventory(Tabletop.PlayerParty[0].CharacterData.Inventory);
    }

    #endregion
}
