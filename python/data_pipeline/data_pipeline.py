"""Dowload the GameData from the google drive."""
import os
import json
import urllib2
from collections import OrderedDict


url = ("https://script.google.com/macros/s/"
       "AKfycbyhrsOHHB9ilJ_9Y9E1XzfilzXeTMt6alkfHUwW1G-N2CuINIg/exec")
game = "TestGame"
data_path = ("C:\PROJECTS\DungeonCrawler\unity\DungeonCrawler\Assets\\"
             "json\GameData")


def download_items():
    items = get("?Game={game}&Table=Items".format(game=game))
    for item in items:
        item["Id"] = str(item["Id"])
        item["Tags"] = item["Tags"] or []
        item["Aspects"] = item["Aspects"] or []
        save("Items/Items", item["Name"], item)


def download_weapons():
    items = get("?Game={game}&Table=Weapons".format(game=game))
    for item in items:
        item["Id"] = str(item["Id"])
        item["Tags"] = item["Tags"] or []
        item["Aspects"] = item["Aspects"] or []
        item["AttackShape"] = item["AttackShape"] or []
        weapon = OrderedDict()
        for k, v in item.items():
            if k not in ("MinDamage", "MaxDamage", "Range",
                         "AmmunitionPerUse", "Hands", "CalculatedCost"):
                weapon[k] = v
        save("Items/Weapons", weapon["Name"], weapon)


def download_armour():
    items = get("?Game={game}&Table=Armor".format(game=game))
    for armour in items:
        armour["Id"] = str(armour["Id"])
        armour["Tags"] = armour["Tags"] or []
        armour["Aspects"] = armour["Aspects"] or []
        save("Items/Armour", armour["Name"], armour)


def download_locations():
    locations = get("?Game={game}&Table=Locations".format(game=game))

    for location in locations:
        for i, cell in enumerate(location["Cells"]):
            location["Cells"][i] = {
                "Position": cell["Position"],
                "Type": cell["Type"]
            }
            if "Destination" in cell.keys():
                location["Cells"][i]["Destination"] = cell["Destination"]

        save("Locations", location["Name"], location)


def download_cellblueprints():
    cell_blueprints = get("?Game={game}&Table=CellBlueprints".format(game=game))

    for cell_blueprint in cell_blueprints:
        save("CellBlueprints", cell_blueprint["Type"], cell_blueprint)


def download_monsters():
    monsters = get("?Game={game}&Table=Monsters".format(game=game))
    for monsters in monsters:
        monsters["Tags"] = monsters["Tags"] or []
        monsters["Aspects"] = monsters["Aspects"] or []
        monsters["Equipment"] = monsters["Equipment"] or {}
        monsters["Inventory"] = monsters["Inventory"] or {}
        save("Monsters", monsters["Name"], monsters)


def download_pcs():
    pcs = get("?Game={game}&Table=PCs".format(game=game))
    for pc in pcs:
        pc["Tags"] = pc["Tags"] or []
        pc["Aspects"] = pc["Aspects"] or []
        pc["Equipment"] = pc["Equipment"] or {}
        pc["Inventory"] = pc["Inventory"] or {}
        save("PCs", pc["Name"], pc)


def download_skills():
    skills = get("?Game={game}&Table=Skills".format(game=game))
    for skill in skills:
        skill["OpposingSkills"] = skill["OpposingSkills"] or []
        save("Skills", skill["Name"], skill)


def download_quests():
    quests = get("?Game={game}&Table=Quests".format(game=game))
    for quest in quests:
        save("Quest", quest["Name"], quest)


def download_globalstate():
    conditions = get("?Game={game}&Table=GlobalState".format(game=game))
    globalstate = {"Conditions":
            {c["Name"]: c["Value"] for c in conditions}
        }
    save("", "GlobalState", globalstate)


def download_rulebook():
    rulebook = get("?Game={game}&Table=Rulebook".format(game=game))
    save("", "Rubelbook", rulebook[0])


def get(request):
    """GET request to google spreadsheets."""
    return json.load(urllib2.urlopen(url + request),
                     object_pairs_hook=OrderedDict)


def save(subfolder, name, data):
    """Save the date in the given sub folder."""
    if not os.path.exists(os.path.join(data_path, subfolder)):
        os.makedirs(os.path.join(data_path, subfolder))
    path = os.path.join(data_path,
                        subfolder,
                        "{0}.json".format(name.replace(" ", "")))
    json.dump(data, open(path, "w"), indent=2)


if __name__ == "__main__":
    download_items()
    download_weapons()
    download_armour()
    download_locations()
    download_cellblueprints()
    download_monsters()
    # download_npcs()
    download_pcs()
    download_skills()
    download_quests()
    download_globalstate()
    download_rulebook()
