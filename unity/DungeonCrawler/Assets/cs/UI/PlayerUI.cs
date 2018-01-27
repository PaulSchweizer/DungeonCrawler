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

    public void Initialize()
    {
        foreach (PlayerCharacter player in PlayerCharacter.PlayerCharacters)
        {
            // Connect signals
            player.Character.Data.OnTakenOut += new TakenOutHandler(PlayerGotTakenOut);

            // Add Portraits for each Character
            RectTransform portrait = GameObject.Instantiate<RectTransform>(CharacterPortrait, PortraitsPanel.transform);
            portrait.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 115);
            portrait.GetComponent<CharacterPortraitUI>().Initialize(player);

            // Initialize the ItemView
            InventoryView.InitFromInventory(player.Character.Data.Inventory);

            // Connect Signals
            player.Character.Data.Inventory.OnItemAdded += new ItemAddedHandler(ItemAdded);
            player.Character.Data.Inventory.OnItemRemoved += new ItemRemovedHandler(ItemRemoved);
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
        InventoryView.InitFromInventory(PlayerCharacter.PlayerCharacters[0].Character.Data.Inventory);
    }

    public void ItemRemoved(object sender, EventArgs e)
    {
        InventoryView.InitFromInventory(PlayerCharacter.PlayerCharacters[0].Character.Data.Inventory);
    }

    #endregion

    #region Events

    public void PlayerGotTakenOut(object sender, EventArgs e)
    {
        bool allTakenOut = true;
        foreach (PlayerCharacter player in PlayerCharacter.PlayerCharacters)
        {
            if (!player.Character.Data.IsTakenOut)
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
