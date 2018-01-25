using DungeonCrawler.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DungeonCrawler.NUnit.Tests
{
    internal static class Utilities
    {
        public static string RootDataPath = @"..\..\Resources\GameData";

        public static string JsonResource(string file)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = string.Format("DungeonCrawler.NUnit.Tests.Resources.{0}.json", file);
            string json;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }
            return json;
        }

        public static string JsonResourceFromFile(string file)
        {
            string root = "C:\\PROJECTS\\DungeonCrawler\\cs\\DungeonCrawler.NUnit.Tests\\Resources\\GameData\\{0}.json";
            return File.ReadAllText(string.Format(root, file));
        }

        public static void LoadRulebook()
        {
            InitializeGame();
        }

        public static Character.Character Hero()
        {
            return Character.Character.DeserializeFromJson(JsonResource("GameData.PCs.Hero"));
        }

        public static Character.Character Rat()
        {
            return Character.Character.DeserializeFromJson(JsonResource("GameData.Monsters.Rat"));
        }

        public static Item Item()
        {
            return Core.Item.DeserializeFromJson(JsonResource("GameData.Items.Items.Item"));
        }

        public static Weapon Weapon()
        {
            return Core.Weapon.DeserializeFromJson(JsonResource("GameData.Items.Weapons.Weapon"));
        }

        public static Armour Armour()
        {
            return Core.Armour.DeserializeFromJson(JsonResource("GameData.Items.Armour.Armour"));
        }

        public static Location Location()
        {
            return Core.Location.DeserializeFromJson(JsonResource("GameData.Locations.Location"));
        }

        public static void SetupTestDice()
        {
            Dice.Distribution = new Dictionary<int, int>() { { -4, -4 }, { -3, -3 }, { -2, -2 }, { -1, -1 }, { 0, 0 },
                                                             { 1, 1 }, { 2, 2 }, { 3, 3 }, { 4, 4 }};
        }

        public static void InitializeGame()
        {
            GameMaster.RootDataPath = RootDataPath;
            string rulebook = File.ReadAllText(Path.Combine(RootDataPath, "Rulebook.json"));

            string[] armourFiles = Directory.GetFiles(Path.Combine(Path.Combine(RootDataPath, "Items"), "Armour"));
            string[] armours = new string[armourFiles.Length];
            for (int i = 0; i < armourFiles.Length; i++)
            {
                armours[i] = File.ReadAllText(armourFiles[i]);
            }

            string[] itemFiles = Directory.GetFiles(Path.Combine(Path.Combine(RootDataPath, "Items"), "Items"));
            string[] items = new string[itemFiles.Length];
            for (int i = 0; i < itemFiles.Length; i++)
            {
                items[i] = File.ReadAllText(itemFiles[i]);
            }

            string[] weaponFiles = Directory.GetFiles(Path.Combine(Path.Combine(RootDataPath, "Items"), "Weapons"));
            string[] weapons = new string[weaponFiles.Length];
            for (int i = 0; i < weaponFiles.Length; i++)
            {
                weapons[i] = File.ReadAllText(weaponFiles[i]);
            }

            string[] skillFiles = Directory.GetFiles(Path.Combine(RootDataPath, "Skills"));
            string[] skills = new string[skillFiles.Length];
            for (int i = 0; i < skillFiles.Length; i++)
            {
                skills[i] = File.ReadAllText(skillFiles[i]);
            }

            string[] monsters = new string[] { };

            string[] locationFiles = Directory.GetFiles(Path.Combine(RootDataPath, "Locations"));
            string[] locations = new string[locationFiles.Length];
            for (int i = 0; i < locationFiles.Length; i++)
            {
                locations[i] = File.ReadAllText(locationFiles[i]);
            };

            string[] cellBlueprintFiles = Directory.GetFiles(Path.Combine(RootDataPath, "CellBlueprints"));
            string[] cellBlueprints = new string[cellBlueprintFiles.Length];
            for (int i = 0; i < cellBlueprintFiles.Length; i++)
            {
                cellBlueprints[i] = File.ReadAllText(cellBlueprintFiles[i]);
            };

            GameMaster.InitializeGame(rulebook, armours, items, weapons, skills, monsters, locations, cellBlueprints);
        }
    }

    /// <summary>
    /// Mock the dice roll and provide a predictable result.
    /// </summary>
    internal class NonRandomDie : Random
    {
        public static int DieResult = 0;
        public static List<int> DieResults = new List<int>();
        private static int _index = 0;

        public NonRandomDie(int dieResult = 0)
        {
            DieResult = dieResult;
        }

        public NonRandomDie(List<int> dieResults)
        {
            DieResults = dieResults;
        }

        public static void Initialize(int dieResult)
        {
            DieResult = dieResult;
            _index = 0;
        }

        public static void Initialize(List<int> dieResults)
        {
            DieResults = dieResults;
            _index = 0;
        }

        public override int Next(int a, int b)
        {
            if (_index >= DieResults.Count)
            {
                return DieResult;
            }
            else
            {
                _index += 1;
                return DieResults[_index - 1];
            }
        }
    }
}
