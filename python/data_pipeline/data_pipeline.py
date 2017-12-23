import os
import json
import urllib
import urllib2


url = 'https://script.google.com/macros/s/AKfycbyhrsOHHB9ilJ_9Y9E1XzfilzXeTMt6alkfHUwW1G-N2CuINIg/exec'
game = "TestGame"

destination = "C:\PROJECTS\DungeonCrawler\unity\DungeonCrawler\Assets\json\TestData\GameData"

def get_locations():
    locations = json.load(urllib2.urlopen(url+"?Game={game}&Table=Locations".format(game=game)))

    for location in locations:
        for i, cell in enumerate(location["Cells"]):
            location["Cells"][i] = {
                "Position": cell["Position"],
                "Type": cell["Type"]
            }
        path = os.path.join(destination, "Locations", "{0}.json".format(location["Name"].replace(" ", "")))
        json.dump(location, open(path, "w"))

    print locations


get_locations()
