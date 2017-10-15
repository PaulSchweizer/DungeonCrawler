Master: [![Build status](https://ci.appveyor.com/api/projects/status/a92mehch8obru8iy?svg=true)](https://ci.appveyor.com/project/PaulSchweizer/dungeoncrawler) [![Coverage Status](https://coveralls.io/repos/github/PaulSchweizer/DungeonCrawler/badge.svg?branch=master)](https://coveralls.io/github/PaulSchweizer/DungeonCrawler?branch=master)

Dev: [![Coverage Status](https://coveralls.io/repos/github/PaulSchweizer/DungeonCrawler/badge.svg?branch=dev)](https://coveralls.io/github/PaulSchweizer/DungeonCrawler?branch=dev)

Docs: https://paulschweizer.github.io/DungeonCrawler/

# DungeonCrawler

1. Dialogs are managed with InkleWriter, a custom parser and custom classes for the dialogs.

2. Data management with Google Spreadsheets and custom google script extensions.

3. Serialization data format is JSON.

# Todos
- [ ] GameMaster
    - [x] Location
    - [x] Cell
    - [ ] Situation
    - [x] LoadRulebook
- [x] Tags
- [x] Aspects
    - [ ] Negative Aspects
    - [ ] Pos/Neg Aspects
    - [ ] Levels of Aspects?
    - [ ] Aspects add to other values too:
        - [ ] Range of Attack

# Rulebook
- [x] All Available Skills
- [x] All available Tags
- [x] All available Items

# Listener
- [x] Listen to Actions

# Character
- [ ] Character
    - [x] Serialize to json
    - [x] Attributes
    - [x] Aspects
        - [x] Cost
    - [x] Consequences
        - [x] Order them from lowest to highest
        - [x] Put armour consequences first
        - [ ] Can't take Consequence of Armour when pointing to original
            - [x] Amount as separate list
            - [x] Copy Object from the Rulebook.Items etc. if given as a string
            - [x] Otherwise, take the entire object
                - [x] Unique and non unique items
                - [ ] Replace with a stand-in object later that only
                      holds the necessary, changeable attributes
            - [x] Add Weapon to Inventory
            - [x] Add Armour
            - [x] Add Item
            - [x] Remove Items
                - [x] Might not remove the correct item
            - [x] Armour provides Protection
            - [x] Item has to be in the Inventory to be taken into account when being equipped
            - [ ] What if serialized and then equipped???
                  Item should be unique

    - [x] Skills
    - [x] Inventory
        - [x] AddItem
        - [x] RemoveItem
        - [x] Items
    - [x] Get the SkillValue as a list of Modifiers
        - [x] What if the Skill doesn't exist?
    - [ ] Loot!

    - [x] Equipment
        - [x] Weapons
        - [x] Armour
        - [x] What if equipped in the wrong slot?
        - [x] What if equipped item is not in Inventory?

    - [x] XP and Levels
        [x] Level Formula 100 * n**2
        [x] XP Cost
        [x] Receive SkillPoints
        [x] How to Skill up
            - Per Level +1 Skill Point
            - The max for skills is 6
            - 5 * (6+5+4+3+2+1) = 105 meaning at Level 105 all Skills would be maxed
            - The cost for a Skills is $level + 1

    - [ ] Stunts
        - [ ] Cost
        - [ ] FatePoints / ActionPoints etc.

- [ ] Quest System
- [ ] Conversation System

- [ ] GameController => GameMaster
    - [ ] Wait for input
    - [ ] Organize Combat
        - [ ] Combat Sequence Controller
            - [ ] Load Protagonists
            - [ ] Determine their positions
            - [ ] Determine their Order
            - [ ] Hand the Action to each Character in turn
            - [ ] Return to the Combat Sequence Controller and start next round
            - [ ] Determine break scenarios

# Where are we?
Location -> Cell -> Grid

1. Grid on the Location
    - [x] GridPoint(int x int y) GridPoint is in world space
    - [x] IsValidPoint(x, y)
    - [x] Character.MoveTo(Position)
        - Get target Cell
        - Set Position on the Grid
        - Register on that Position or on that GridPoint?
    - [ ] Remove current cell
    - [ ] Remove CurrentTags


# How do Characters and their PositionalInteraction Work?

----------------------------------

- GameMaster knows all Characters
- Character wants to know whether a Character is in reach
    - Ask the GameMaster for who is on the given Position
        - Loop over all of them and return the matches
- The Character knows about it's Cell
    - Take the currentCell from the Character to determine the Tags

----------------------------------


    - [ ] Move also get the target rotation (target transform)
    - [ ] Chase get the target Transform

    - [ ] Mask of Enemies, use the defined tags on the Character

    - [ ] GameMaster.Characters
        - [ ] GameMaster.PlayerCharacters?
        - [ ] GameMaster.Enemies?

    - [x] GameMaster.CharactersOnCell(Cell)

    - [ ] GameMaster.CharactersOnPosition(Position)
        - [ ] CharactersRegisterInCell
        - [ ] CharacterRegistersInPosition

    - [ ] GameMaster.CurrentCell.Characters?
        - [ ] Revisit the CurrentCell, rather get the CurrentCell per Character

    - [x] PositionIsWalkable(Position)
        - [x] For now all GridPositions on Cells are walkable

    - [x] Visualize the Grid ...
    - [x] ... and the Character on the Grid

    - [x] Map Rotation properly
    - [x] Char Rotates the opposite way
    - [x] Default AttackShape

    - [ ] Place the Characters on the Grid properly, find a good spot for each

    - [ ] Character.Attack()
            - [x] positionOfCharacter x, y
            - [x] directionOfCharacter 0, 0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75 PI
            AttackShape of Attack
                - the origin is at character pos
                - then transform the attack points to get the WorldSpace GridPoints
                e.g. [[0, 1], [1, 1], [1, 0]] results in an sweeping blow from left to right:
                   [1][2]   [1]
                   [C][3]   [C]
                            [2]
                - Attacks are executed in the order of the list
            TimeOffset of Attack
            ScheduleAttackOnGridPoints([...], timeOffset, Character, Attack)
                --> TimeOffset ...
                    { Things can end the Attack during this time,
                      e.g. getting hit, or moving away. }
                    ApplyAttackOnGridPoints([...])

    Attack {
        "PreTime": 1,
        "PostTime": 0,
        "Shape": [
            [1, 0],
            [1, 1],
            [0, 1]
        ],
        "Value": 3
    }

    Attack {
        Attacker
        Skill
        Stunt
    }


# Who are we?
Player -> Characters

# What is happening?
Situation

## Combat
    1. Attack one or multiple GridPoints
    2. Countdown until Attack takes Effect
    3. Check if Enemies on the GridPoints
    4. Apply Damage to the Enemies

# Save State
- [ ] Party
    - [ ] Characters
- [ ] GameMaster
    - [ ] CurrentLocation
    - [ ] CurrentCell

# Actions that a Character can perform
- [ ] Attack
    - [x] MeleeWeapons
    - [ ] RangedWeapons
    - [ ] Magic
- [ ] Defend
    - [x] MeleeWeapons
    - [ ] RangedWeapons
    - [ ] Magic
- [ ] Repair
    - [ ] Item
- [ ] Heal
    - [ ] Stress
    - [x] Consequence
    - [x] Bug Consequence adds empty Aspects to the List when healing ???
- [ ] Move
    - [ ] Hit the surface
        - [ ] Get the closest Grid Point
            - [ ] Set the position

# Actions that a Player can perform
- [x] Attack(skill, attacker, defender)
    - [x] Add Weapon Damage
    - [ ] Ranges and Zones on the battlefield
        - [ ] Combat Grid
        - [ ] Range of Weapons
        - [ ] Aspects that add to the Range
- [ ] Move(destination)
- [ ] Repair(item)
- [x] Heal(character)
- [x] Equip(item)
- [x] UnEquip(item)
- [ ] ImproveSkill(skill)
- [ ] LearnSkill(skill)
- [ ] AddNewAspect(aspect)
- [ ] EditAspect(aspect)

# Skills
- [x] MeleeWeapons
    - Attack
    - Defend
- [ ] RangedWeapons
    - Attack
- [ ] Craftsmanship
    - Repair
- [x] Healing
    - Heal

# Unity
- [ ] Mock up UI for the player stats
- [ ] Levelbuilder
    - [x] Read Json and build the level
    - [x] Testing tiles are forest and clearing for now
    - [x] Dynamically build levels
    - [ ] Add enemies to the tiles
    - [ ] Add treasure to the tiles
    - [ ] Add Quest things to the tiles
- [ ] Worldmap
    - [ ] Location
        - [ ] Cell
            - [ ] Grid


- [ ] Game Startup Procedure
    - [ ] Init Rulebook
    - [ ] If New Game
        - [ ] Load Default Environment
    - [ ] If Load
        - [ ] Load Saved Game State
    - [ ] Wait for user input


# Different interaction levels in the game
Worldmap
  |
  Location
    |
    Cell
      |
      Situation
        |
        Action

## Worldmap
- [ ] Travel between locations
- [ ] Explore new territories
- [ ] Random encounters during traveling
- [ ] Hunting
- [ ] Collecting herbs
- [ ] Camping

## Location
- [ ] Take control of the characters and explore the location
- [ ] Isometric top down perspective
- [ ] Location is made up of Cells

## Cell
- [ ] Building blocks for a location
- [ ] Decorative elements
- [ ] Items
- [ ] NPCs
- [ ] Enemies
- [ ] Obstacles
- [ ] Each cell has it's defined content randomly placed on the Cell
- [ ] A flat quad
- [ ] NavMesh has to be figured out

## Situation
The situational state of the game.

- [ ] Exploration
- [ ] Combat
- [ ] Conversation
- [ ] Trade
- [ ] Cinematic (=> Displaying story text)

## Action
The action that a Character takes, basically the used Skill.

- [ ] Attack
- [ ] Magic
- [ ] Use Item
- [ ] Other Skills ...


# Skill and Combat System - Driven by Aspects
Skills have a base value ranging from 0 to 10.

The base value is altered by Aspects of the Character that correlate with the current Situation.
Aspects are traits that define and describe anything in the game.

These Aspects can be beneficial or counterproductive to the action that the Character wants to take and thus add or subtract from the Skill's base value.

Example:

    John the Ranger is known for having defeated the white #wolf. He is a child of the #wilderness. But he is afraid of #caverns.

Each tag describes an aspect of the Player Character.

    John is exploring #dark #caverns and stumbles upon a #hungry #northern #wolf.

The wolf attacks and we find ourselves in a MeleeCombat Situation. John has to slay the attacking beast. These are his Stats:

```json
"Skills": {
    "MeleeCombat": 2,
    "Stealth": 1
},
"Aspects": [
    {
        "Name": "Master of the white #wolf",
        "Skills": ["MeleeCombat", "Stealth"],
        "Bonus": 1
    },
    {
        "Name": "Child of the #wilderness",
        "Skills": ["Stealth"],
        "Bonus": 0
    },
    {
        "Name": "Afraid of #caverns",
        "Skills": ["MeleeCombat"],
        "Bonus": -1
    }
],
"Equipment": {
    "Name": "Wolfbane",
    "Description": "Magic sword against wolves",
    "Skills": ["Combat"],
    "Bonus": 3,
    "Type": "Weapon",
    "Aspects": [
        {
            "Name": "Slayer of #wolves",
            "Skills": ["Combat"],
            "Bonus": 1
        }
    ]
}
```

Let's look at the aspects of the situation first:
    #dark #caverns

Now let's compare these aspects to John the Ranger's aspects:
    #wolf #wilderness #caverns

This means that the situation affects his combat skill negatively because he is afraid in these dark #caverns.

Now, let's analyze the action, MeleeCombat. The opponent is a #wolf and since John has experience with wolves, he gets a bonus of +1 on his action.

John also wields Wolfbane, a magic sword against wolves that will be helpful.

The final MeleeCombat value for this action in this particular situation for John is:

    2(base) +3(weapon) -1(#caverns) +1(#wolf) +1(#wolf) = 6

## Aspects and Tags
Tags are the key descriptors of the game elements.
These tags will trigger the corresponding aspects.

Aspects fall in three categories:

1. Add 1 if the situation provides a corresponding tag.
2. Subtract 1 if the situation does not provide the corresponding tag.
3. Add 1 if the situation provides a corresponding tag but also subtract 1 if the situation does not provide the corresponding tag.

Each aspect only affects a certain set of Skills.

Tag:

```json
{
    "Name": "wolf",
    "Alternatives": ["wolves"]
}
```

Aspect:

```json
{
    "Name": "#Master of #wolves",
    "Skills": ["Combat", "Stealth"],
    "Bonus": 1
}
```

### Acquiring, Removing and Changing Aspects
- Permanent Aspects can be acquired on Level-Up, through taking Consequences and through completing certain quests.
- Aspects can be changed on Level-Up.
- Aspects can be changed and removed in certain areas (e.g. fountain of healing) or on completing certain tasks.


## Special Skills
These Skills work in conjunction with the basic Skills.
They improve the Skill value for an action.
They cost activation points.
They can have tags assigned that limit their effectiveness to only those certain situations that hold a corresponding tag.

This Special Skill for example will only work against undead opponents.

```json
{
    "Name": "Vanquish the #undead",
    "Bonus": 4,
    "Skills": ["Combat"]
}
```

# Combat

## Melee

```cs
Character.Attack(Character attacker,
                 Skill skill,
                 Weapon weapon,
                 Stunt stunt,
                 Character[] defenders,
                 Situation situation){
    int aspectBonus = AspectBonus(Attacker, defenders, situation);
    int damage = Max(0, skill.Value
                        + weapon.Bonus
                        + stunt.Bonus
                        + aspectBonus
                        + DFate);
    for (Character defender in defenders) {
        defender.ReceiveDamage(damage, attacker);
    }
}
```

```cs
Character.ReceiveDamage(int damage, Character attacker) {
    if (PhysicalStress.Value + damage > PhysicalStress.MaxValue) {
        TakeConsequence(damage);
    }
    else {
        PhysicalStress.Value += damage;
    }
}

Character.TakeConsequence(int damage, Character attacker) {
    // Try to see if the damage can be absorbed by a consequence
    for (Consequence consequence in Consequences) {
        if (damage <= consequence.Capacity && !consequence.Active)
        {
            consequence.Active = True;
            return;
        }
    }

    // Too much damage, can't take another consequence
    ChangeState(States.PassedOut);
}
```

# Tags

    TagsTable.Synonyms(string tag)
    TagsTable.Opposing(string tag)


