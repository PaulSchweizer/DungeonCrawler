using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DungeonCrawler;
using DungeonCrawler.Core;
using System.IO;
using System;

public class Level : MonoBehaviour
{
    public int ScalingFactor;

    [Header("Cells")]
    public GameObject ForestPrefab;
    public GameObject ClearingPrefab;
    public List<GameObject> Cells = new List<GameObject>();

    [Header("Cell Additionals")]
    public GameObject ExitPrefab;

    [Header("Monsters")]
    public GameObject[] MonsterPrefabs;
    public string[] MonsterNames;

    [Header("NPCs")]
    public GameObject[] NPCPrefabs;
    public string[] NPCNames;

    [Header("Items")]
    public GameObject LootPrefab;

    [Header("Prefabs")]
    public GameObject ExitUIPrefab;

    // Internals
    public Location Location;
    
    public void Create()
    {
        name = Location.Name;

        ExitUI exitUI = Instantiate(ExitUIPrefab).GetComponent<ExitUI>();

        foreach (Cell cell in Location.Cells)
        {
            GameObject instance = null;
            if (cell.Type == "Forest")
            {
                instance = Instantiate(ForestPrefab, transform);
            }
            else if (cell.Type == "Clearing")
            {
                instance = Instantiate(ClearingPrefab, transform);
            }
            else
            {
                throw new Exception(string.Format("Celltype {0} does not exist."));
            }
            instance.name = string.Format("{0}_{1}_{2}", cell.Type, cell.Position[0], cell.Position[1]);
            instance.transform.localScale = new Vector3(ScalingFactor / 10, ScalingFactor / 10, ScalingFactor / 10);
            instance.transform.position = new Vector3(-cell.Position[0] * ScalingFactor, 0, cell.Position[1] * ScalingFactor);
            Cells.Add(instance);

            // Destination - Cell is an Exit to another Location
            //
            if (cell.Destination != null)
            {
                GameObject exit = Instantiate(ExitPrefab, instance.transform);
                exit.GetComponent<Exit>().Destination = cell.Destination;
                exit.name = string.Format("ExitTo_{0}", cell.Destination);
                exit.transform.localScale = new Vector3(1, 1, 1);
                exit.transform.localPosition = new Vector3(0, 0, 0);
                exit.GetComponent<Exit>().UI = exitUI;
            }

            // Monsters
            //
            if (cell.Monsters != null)
            {
                foreach (KeyValuePair<string, int> entry in cell.Monsters)
                {
                    GameObject prefab = MonsterPrefabs[Array.IndexOf(MonsterNames, entry.Key)];
                    for (int i = 0; i < entry.Value; i++)
                    {
                        GameObject monster = Instantiate(prefab, transform);
                        monster.transform.position = instance.transform.position;
                    }
                }
            }

            // NPCs
            //
            if (cell.NPCs != null)
            {
                foreach (string npcName in cell.NPCs)
                {
                    GameObject prefab = NPCPrefabs[Array.IndexOf(NPCNames, npcName)];
                    GameObject npc = Instantiate(prefab, transform);
                    npc.transform.position = instance.transform.position;
                }
            }

            // Items
            //
            if (cell.Items.Count > 0)
            {
                Loot lootPrefab = Instantiate(LootPrefab, transform).GetComponent<Loot>();
                lootPrefab.transform.position = instance.transform.position;
                lootPrefab.Items = cell.Items;
                lootPrefab.RemoveItemsOnCollect = true;
                foreach (DungeonCrawler.Core.Event ev in cell.Events)
                {
                    if(ev.Type == DungeonCrawler.Core.Event.Types.OnItemsPickedUp)
                    {
                        lootPrefab.GlobalStateCondition = ev.Condition;
                        lootPrefab.GlobalStateValue = ev.Value;
                        break;
                    }
                }
            }

            // Events
            //
            //if (cell.Events != null)
            //{
            //    foreach (KeyValuePair<string, string> entry in cell.Events)
            //    {
            //        Debug.Log(string.Format("Add Event {0} - {1}", entry.Key, entry.Value));

            //        if (entry.Key == "OnItemPickedUp")
            //        {
            //        }
            //    }
            //}
        }

        NPCCharacter[] npcs = GameObject.FindObjectsOfType<NPCCharacter>();
        foreach (NPCCharacter npc in npcs)
        {
            npc.transform.parent = transform;
        }

        GetComponent<NavMeshSurface>().BuildNavMesh();

        EnemyCharacter[] enemies = GameObject.FindObjectsOfType<EnemyCharacter>();
        foreach (EnemyCharacter enemy in enemies)
        {
            enemy.transform.parent = transform;
        }

        PlayerCharacter[] pcs = GameObject.FindObjectsOfType<PlayerCharacter>();
        foreach (PlayerCharacter pc in pcs)
        {
            pc.transform.parent = transform;
        }
    }

#if UNITY_EDITOR

    public bool DrawDebug = true;

    void OnDrawGizmos()
    {
        if (!DrawDebug)
        {
            return;
        }
        Color oldColor = Gizmos.color;
        Gizmos.color = Color.white;
        foreach (GameObject cell in Cells)
        {
            for (int i = 0; i < 10; i++)
            {
                Gizmos.DrawRay(cell.transform.position - Vector3.right * (5 - i) - Vector3.forward * 5 + Vector3.right * 0.5f,
                               Vector3.forward * 10);
                Gizmos.DrawRay(cell.transform.position - Vector3.forward * (5 - i) - Vector3.left * 5 + Vector3.forward * 0.5f,
                   Vector3.left * 10);
            }
        }
        Gizmos.color = oldColor;
    }

#endif
}
