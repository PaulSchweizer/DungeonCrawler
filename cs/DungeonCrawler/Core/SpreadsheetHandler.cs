using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using DungeonCrawler.Items;
using System.Data;
using DungeonCrawler.Locations;
using DungeonCrawler.Dialogs;

namespace DungeonCrawler.Core
{
    public static class SpreadsheetHandler
    {

        public static bool Online;
        public static string AppUrl = "https://script.google.com/macros/s/AKfycbyhrsOHHB9ilJ_9Y9E1XzfilzXeTMt6alkfHUwW1G-N2CuINIg/exec";

        public static string RegisteredGames
        {
            get {
                if (Online) { return "Table=RegisteredGames"; }
                else { return @"C:\PROJECTS\DungeonCrawler\unity\DungeonCrawler\Assets\__temp\Locations.json"; }    
            }
        }

        public static string Dialogs
        {
            get
            {
                if (Online) { return "Table=Dialogs"; }
                else { return @"..\..\..\..\json\Dialogs.json"; }
            }
        }

        public static string GameConditions
        {
            get
            {
                if (Online) { return "Table=GameConditions"; }
                else { return @"..\..\..\..\json\GameConditions.json"; }
            }
        }

        //public static string Attributes = "Table=Attributes";
        //public static string QuestTypes = "Table=QuestTypes";
        //public static string Events = "Table=Events";
        //public static string Tiles = "Table=Tiles";
        public static string Items = "Table=Items&Game=TestGame";
        public static string Locations = "Table=Locations&Game=TestGame";

        public static string Get(string data)
        {

            if (Online)
            {
                using (WebClient client = new WebClient())
                {
                    var url = string.Concat(AppUrl, "?", data);
                    var json = client.DownloadString(url);
                    return json;
                }
            }
            else
            {
                var json = File.ReadAllText(data);
                return json;
            }

        }

        public static GameData[] GetRegisteredGames()
        {
            var json = Get(RegisteredGames);
            return JsonConvert.DeserializeObject<GameData[]>(json);
        }

        public static Item[] GetItems()
        {
            var json = Get(Items);
            return JsonConvert.DeserializeObject<Item[]>(json);
        }

        public static Location[] GetLocations()
        {
            var json = Get(Locations);
            return JsonConvert.DeserializeObject<Location[]>(json);
        }

        public static Dialog[] GetDialogs()
        {
            var json = Get(Dialogs);
            return JsonConvert.DeserializeObject<Dialog[]>(json);
        }

        public static GameCondition[] GetGameConditions()
        {
            var json = Get(GameConditions);
            return JsonConvert.DeserializeObject<GameCondition[]>(json);
        }
    }
}