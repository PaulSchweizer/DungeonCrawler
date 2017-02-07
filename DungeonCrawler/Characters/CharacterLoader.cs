using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;

namespace DungeonCrawler.Characters
{
    public class CharacterLoader
    {

        public string Url = "https://script.google.com/macros/s/AKfycbzewPtBvw_vTozKqb1DD6uw5VZ_tKow86fpYVbi-kIsHfI3qh8/exec?table=CharactersTest";

        public Character[] LoadCharactersFromUrl()
        {
            using (WebClient client = new WebClient())
            {
                var json = client.DownloadString(Url);

                Console.WriteLine(json);


                Character[] characters = JsonConvert.DeserializeObject<Character[]>(json);




                return characters;
            }
        }

    }
}
