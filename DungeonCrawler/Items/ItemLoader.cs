using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;

namespace DungeonCrawler.Items
{
    public class ItemLoader
    {

        public string Url = "https://script.google.com/macros/s/AKfycbzewPtBvw_vTozKqb1DD6uw5VZ_tKow86fpYVbi-kIsHfI3qh8/exec?table=Items";

        public Item[] LoadItemsFromUrl()
        {
            using (WebClient client = new WebClient())
            {
                var json = client.DownloadString(Url);
                Item[] items = JsonConvert.DeserializeObject<Item[]>(json);
                return items;
            }
        }

    }
}
