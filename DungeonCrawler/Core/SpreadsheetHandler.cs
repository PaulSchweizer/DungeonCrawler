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

namespace DungeonCrawler.Core
{
    public static class SpreadsheetHandler
    {

        public static string AppUrl = "https://script.google.com/macros/s/AKfycbyhrsOHHB9ilJ_9Y9E1XzfilzXeTMt6alkfHUwW1G-N2CuINIg/exec";

        public static string RegisteredGames = "Table=RegisteredGames";
        //public static string Attributes = "Table=Attributes";
        //public static string QuestTypes = "Table=QuestTypes";
        //public static string Events = "Table=Events";
        //public static string Tiles = "Table=Tiles";
        public static string Items = "Table=Items&Game=TestGame";
        public static string Locations = "Table=Locations&Game=TestGame";

        public static string Get(string data)
        {
            using (WebClient client = new WebClient())
            {
                var url = string.Concat(AppUrl, "?", data);
                var json = client.DownloadString(url);
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
    }
}