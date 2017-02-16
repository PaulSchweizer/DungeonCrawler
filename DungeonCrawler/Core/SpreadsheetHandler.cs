using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using DungeonCrawler.Items;
using System.Data;

namespace DungeonCrawler.Core
{
    public static class SpreadsheetHandler
    {

        public static string AppUrl = "https://script.google.com/macros/s/AKfycbyhrsOHHB9ilJ_9Y9E1XzfilzXeTMt6alkfHUwW1G-N2CuINIg/exec";
        public static string RegisteredGames = "table=RegisteredGames";
        public static string Attributes = "table=Attributes";
        public static string QuestTypes = "table=QuestTypes";
        public static string Events = "table=Events";
        public static string Tiles = "table=Tiles";

        public static string Get(string data)
        {
            using (WebClient client = new WebClient())
            {
                var url = string.Concat(AppUrl, "?", data);
                var json = client.DownloadString(url);
                return json;
            }
        }

        public static DataSet GetRegisteredGames()
        {
            var json = Get(RegisteredGames);
            DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(json);
            return dataSet;
        }

    }
}