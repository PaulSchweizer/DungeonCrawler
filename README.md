[![Build status](https://ci.appveyor.com/api/projects/status/a92mehch8obru8iy?svg=true)](https://ci.appveyor.com/project/PaulSchweizer/dungeoncrawler) [![Coverage Status](https://coveralls.io/repos/github/PaulSchweizer/DungeonCrawler/badge.svg?branch=master)](https://coveralls.io/github/PaulSchweizer/DungeonCrawler?branch=master)

Docs: https://paulschweizer.github.io/DungeonCrawler/

# DungeonCrawler

1. Dialogs are managed with InkleWriter, a custom parser and custom classes for the dialogs.

2. Data management with Google Spreadsheets and custom google script extensions.

3. Serialization data format is JSON.

# Todos
- [ ] GameMaster
    - [ ] Location
    - [x] Cell
    - [ ] Situation
    - [ ] LoadRulebook
- [x] Tags
- [x] Aspects

# Rulebook
- [x] All Available Skills
- [x] All available Tags

# Character
- [ ] Character
    - [x] Serialize to json
    - [x] Attributes
    - [x] Aspects
    - [x] Consequences
    - [x] Skills

    - [ ] Inventory
    - [ ] Equipment
        - [ ] Weapons
        - [ ] Armour

    - [ ] XP and Levels
    - [ ] Stunts

- [ ] Aspect & Tag System
    - [ ] Actions
        - [ ] Combat
        - [ ] Magic
        - [ ] Trading

- Quest System
(- Conversation System)

# Actions that a Character can perform
- [ ] Attack
    - [ ] MeleeWeapons
    - [ ] RangedWeapons
    - [ ] Magic
- [ ] Defend
    - [ ] MeleeWeapons
    - [ ] RangedWeapons
    - [ ] Magic
- [ ] Repair
    - [ ] Item
- [ ] Heal
    - [ ] Stress
    - [ ] Consequence
- [ ] Move

# Actions that a Player can perform
- [ ] Attack(attacker, defender)
- [ ] Move(destination)
- [ ] Repair(item)
- [ ] Heal(character)
- [ ] Equip(item)
- [ ] UnEquip(item)
- [ ] ImproveSkill(skill)
- [ ] LearnSkill(skill)
- [ ] AddNewAspect(aspect)
- [ ] EditAspect(aspect)

# Skills
- [ ] MeleeWeapons
    - Attack
    - Defend
- [ ] RangedWeapons
    - Attack
- [ ] Craftsmanship
    - Repair
- [ ] Healing
    - Heal

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


