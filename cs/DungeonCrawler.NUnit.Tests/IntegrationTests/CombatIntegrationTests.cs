﻿using NUnit.Framework;
using DungeonCrawler.Core;
using System.Collections.Generic;
using System;

namespace DungeonCrawler.NUnit.Tests.IntegrationTests
{
    [TestFixture]
    public class CombatIntegrationTests
    {
        Character.Character hero;
        Character.Character rat;
        Weapon weapon;
        Armour armour;

        [SetUp]
        public void SetUp()
        {
            hero = Utilities.Hero();
            rat = Utilities.Rat();
            weapon = Utilities.Weapon();
            armour = Utilities.Armour();
            Utilities.LoadRulebook();
            Utilities.SetupTestDice();
            Dice.Die = new NonRandomDie(0);
            hero.Aspects = new List<Aspect>();
            rat.Equipment["RightHand"] = null;

            GameEventsLogger.Reset();
        }

        [Test]
        public void Hero_suffers_damage_from_rat_attack()
        {
            hero.Skills["MeleeWeapons"] = 0;
            rat.Skills["MeleeWeapons"] = 1;

            rat.Attack(hero);
            Assert.AreEqual(1, hero.PhysicalStress.Value);

            rat.Inventory.AddItem(weapon);
            rat.Equip(weapon.Identifier, "RightHand");

            rat.Attack(hero);
            Assert.AreEqual(1 + 1 + weapon.Damage, hero.PhysicalStress.Value);
        }

        [Test]
        public void Hero_generates_spin_by_defending()
        {
            hero.Skills["MeleeWeapons"] = 5;
            rat.Skills["MeleeWeapons"] = 1;

            rat.Attack(hero);
            Assert.AreEqual(2, hero.Spin);
        }

        [Test]
        public void Hero_uses_spin_when_attacking()
        {
            hero.Skills["MeleeWeapons"] = 1;
            rat.Skills["MeleeWeapons"] = 1;
            hero.Spin = 2;
            hero.Attack(rat);
            Assert.AreEqual(2, rat.PhysicalStress.Value);
        }

        [Test]
        public void Hero_uses_Stunt()
        {
            hero.Skills["MeleeWeapons"] = 1;
            rat.Skills["MeleeWeapons"] = 1;

            hero.Attack(rat, "MeleeWeapons", hero.Stunts[0]);
            Assert.AreEqual(2, rat.PhysicalStress.Value);
        }

        [Test]
        public void Example_combat()
        {
            // Our Hero is venturing around in #dark #caverns.
            // He encounters a group of 3 mutated rats that attack him.
            // He will fight them off.
            // He will collect their loot.
            // And he will suffer some injuries.

            ////////////////////////////////////////////////////////

            // 1. Setup 
            // GameMaster defines the current cell that the events happen in.
            Cell cell = new Cell();
            cell.Tags = new string[] { "dark", "cavern" };
            GameMaster.CurrentCell = cell;

            // 2. Preparing the Rats
            Character.Character rat1 = Utilities.Rat();
            Character.Character rat2 = Utilities.Rat();
            Character.Character rat3 = Utilities.Rat();
            rat1.Name = "Rat 1";
            rat2.Name = "Rat 2";
            rat3.Name = "Rat 3";

            // 3. The Hero equips his Weapon
            Character.Character hero = Utilities.Hero();
            //hero.Inventory.Weapons.Add(weapon);
            //hero.Equip(weapon.Name, "RightHand");

            // 3. The fight happens
            // Round 1:
            // The Hero defeats the first rat right away due to good dice rolls and usage of his Stunt
            NonRandomDie.Initialize(new List<int> { 2, 0 });
            hero.Attack(rat1, "MeleeWeapons", hero.Stunts[0]);

            // The first rat attacks the hero, it has to roll at least 3 in order to deal some damage,
            // The 0 is not enough
            NonRandomDie.Initialize(new List<int> { 0, 0 });
            rat1.Attack(hero, "MeleeWeapons");

            // The next rat is really lucky and rolls a 4, dealing 2 damage
            NonRandomDie.Initialize(new List<int> { 4, 0 });
            rat2.Attack(hero, "MeleeWeapons");

            // The third rat is really unlucky and rolls a -1, giving the hero 2 Spin on his next Action
            NonRandomDie.Initialize(new List<int> { -2, 0 });
            rat3.Attack(hero, "MeleeWeapons");

            // Round 2:
            // The Hero damages the second rat right away due to good dice rolls
            NonRandomDie.Initialize(new List<int> { 2, 0 });
            hero.Attack(rat2, "MeleeWeapons");

            NonRandomDie.Initialize(new List<int> { 0, 0 });
            rat2.Attack(hero, "MeleeWeapons");

            NonRandomDie.Initialize(new List<int> { 2, 0 });
            rat3.Attack(hero, "MeleeWeapons");

            // Round 3:
            NonRandomDie.Initialize(new List<int> { 1, 0 });
            hero.Attack(rat2, "MeleeWeapons");

            NonRandomDie.Initialize(new List<int> { -1, 0 });
            rat2.Attack(hero, "MeleeWeapons");

            NonRandomDie.Initialize(new List<int> { 3, 0 });
            rat3.Attack(hero, "MeleeWeapons");

            // Round 4:
            NonRandomDie.Initialize(new List<int> { 1, -1 });
            hero.Attack(rat3, "MeleeWeapons");

            NonRandomDie.Initialize(new List<int> { 3, -3 });
            rat3.Attack(hero, "MeleeWeapons");

            // Round 5:
            NonRandomDie.Initialize(new List<int> { 1, -1 });
            hero.Attack(rat3, "MeleeWeapons");

            Console.WriteLine(GameEventsLogger.Next);
        }

        [Test]
        public void Combat_on_grid()
        {
            // Place Attack on grid points
            // Countdown
            // Check if Characters on GridPoint
            // Apply Attack
        }
    }
}
