using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using DungeonCrawler.Core;
using UnityEngine;

public static class SpreadsheetHandler
{

    public static bool Online;
    public static string AppUrl = "https://script.google.com/macros/s/AKfycbyhrsOHHB9ilJ_9Y9E1XzfilzXeTMt6alkfHUwW1G-N2CuINIg/exec";

    public static string Location
    {
        get
        {
            if (Online) { return "Table=RegisteredGames"; }
            else { return @"C:\PROJECTS\DungeonCrawler\unity\DungeonCrawler\Assets\json\Location.json"; }
        }
    }

    public static string JsonFolder
    {
        get
        {
            return Path.Combine(Application.dataPath, @"json\");
        }
    }

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
            string path = Path.Combine(JsonFolder, string.Format("{0}.json", data));
            var json = File.ReadAllText(path);
            return json;
        }

    }

    public static Location GetLocation(string location)
    {
        string json = Get(location);
        return JsonConvert.DeserializeObject<Location>(json);
    }


}