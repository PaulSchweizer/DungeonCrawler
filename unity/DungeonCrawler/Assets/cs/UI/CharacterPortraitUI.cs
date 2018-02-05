using DungeonCrawler.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class CharacterPortraitUI : MonoBehaviour
{
    public Text Name;
    public Image Portrait;
    public Slider PhysicalStress;

    private PlayerCharacter Player;

    public void Initialize(PlayerCharacter player)
    {
        Player = player;
        Name.text = Player.Data.Name;
        PhysicalStress.minValue = Player.Data.PhysicalStress.MinValue;
        PhysicalStress.maxValue = Player.Data.PhysicalStress.MaxValue;
        PhysicalStress.value = Player.Data.PhysicalStress.Value;
        Player.Data.OnPhysicalStressChanged += new PhysicalStressChangedHandler(PhysicalStressChanged);
    }

    public void PhysicalStressChanged(object sender, EventArgs e)
    {
        PhysicalStress.value = Player.Data.PhysicalStress.Value;
    }
}

