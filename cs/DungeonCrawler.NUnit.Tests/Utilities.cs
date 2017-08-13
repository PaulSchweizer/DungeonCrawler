﻿using DungeonCrawler.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DungeonCrawler.NUnit.Tests
{
    internal static class Utilities
    {

        public static string JsonResource(string file)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = string.Format("DungeonCrawler.NUnit.Tests.Resources.{0}.json", file);
            string json;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }
            return json;
        }

        public static void LoadRulebook()
        {
            string json = JsonResource("Rulebook");
            Rulebook.DeserializeFromJson(json);
        }

        public static Character.Character Hero()
        {
            return Character.Character.DeserializeFromJson(JsonResource("Hero"));
        }

        public static Character.Character Rat()
        {
            return Character.Character.DeserializeFromJson(JsonResource("Rat"));
        }

        public static Item Item()
        {
            return Core.Item.DeserializeFromJson(JsonResource("Item"));
        }

        public static Weapon Weapon()
        {
            return Core.Weapon.DeserializeFromJson(JsonResource("Weapon"));
        }

        public static Armour Armour()
        {
            return Core.Armour.DeserializeFromJson(JsonResource("Armour"));
        }
    }

    /// <summary>
    /// Mock the dice roll and provides a predictable result.
    /// </summary>
    internal class NonRandomDie : Random
    {
        public static int DieResult = 0;

        public NonRandomDie(int dieResult = 0)
        {
            DieResult = dieResult;
        }

        public override int Next(int a, int b)
        {
            return DieResult;
        }
    }
}
