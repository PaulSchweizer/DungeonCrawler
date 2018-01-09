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
    public TextAsset JsonFile;
    public GameObject ForestPrefab;
    public GameObject ClearingPrefab;
    public List<GameObject> Cells = new List<GameObject>();

    [Header("Monsters")]
    public GameObject[] MonsterPrefabs;
    public string[] MonsterNames;

    // Internals
    public Location Location;
    
    public void Create()
    {
        name = Location.Name;

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
                continue;
            }
            instance.name = string.Format("{0}_{1}_{2}", cell.Type, cell.Position[0], cell.Position[1]);
            instance.transform.localScale = new Vector3(ScalingFactor / 10, ScalingFactor / 10, ScalingFactor / 10);
            instance.transform.position = new Vector3(-cell.Position[0] * ScalingFactor, 0, cell.Position[1] * ScalingFactor);
            Cells.Add(instance);

            if (cell.Monsters != null)
            {
                foreach (KeyValuePair<string, int> entry in cell.Monsters)
                {
                    GameObject prefab = MonsterPrefabs[Array.IndexOf(MonsterNames, entry.Key)];
                    for (int i = 0; i < entry.Value; i++)
                    {
                        Instantiate(prefab, transform);
                    }
                }
            }
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
