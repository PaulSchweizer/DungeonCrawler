using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonCrawler.Character;
using DungeonCrawler.Core;

public class EnemyCharacter : BaseCharacter
{
    public override void Awake()
    {
        Idle = EnemyIdleState.Instance;
        Chase = EnemyChaseState.Instance;
        base.Awake();
    }
}


public class EnemyIdleState : CharacterState
{
    public static EnemyIdleState Instance = new EnemyIdleState();

    public EnemyIdleState()
    {
        DebugColor = Color.green;
    }

    public override void Enter(BaseCharacter character)
    {
        character.NavMeshAgent.isStopped = true;
    }

    public override void Update(BaseCharacter character)
    {
        foreach (PlayerCharacter pc in Tabletop.PlayerParty)
        {
            if (Vector3.Distance(character.transform.position, pc.transform.position) < character.AlertnessRadius)
            {

                // Angle Rotation 
                Vector3 pos = new Vector3(character.transform.position.x, 0, character.transform.position.z);
                Vector3 rotation = Vector3.RotateTowards(character.transform.forward, pc.transform.position - pos, 2 * Mathf.PI, 1);
                character.SetDestination(pc.transform.position, rotation);

                character.NavMeshAgent.SetDestination(pc.transform.position);
                character.ChangeState(character.Chase);
                return;
            }
        }
    }

    public override void Exit(BaseCharacter character)
    {
        character.NavMeshAgent.isStopped = false;
    }
}


public class EnemyChaseState : CharacterState
{

    private float rotationThreshold = 4f;

    public static EnemyChaseState Instance = new EnemyChaseState();

    public EnemyChaseState()
    {
        DebugColor = Color.magenta;
    }

    public override void Enter(BaseCharacter character) { }

    public override void Update(BaseCharacter character)
    {
        if (character.NavMeshAgent.pathPending)
        {
            return;
        }

        foreach (int[] point in character.CharacterData.AttackShape)
        {
            if (GameMaster.CharactersOnGridPoint(character.CharacterData.Transform.Map(point),
                                                 types: character.CharacterData.Enemies).Length > 0)
            {
                character.ChangeState(character.Attack);
                return;
            }
        }

        if (character.NavMeshAgent.remainingDistance > character.NavMeshAgent.stoppingDistance + character.NavMeshAgent.radius + 1)
        {
            foreach (PlayerCharacter pc in Tabletop.PlayerParty)
            {
                if (Vector3.Distance(character.transform.position, pc.transform.position) < character.AlertnessRadius)
                {
                    Vector3 pos = new Vector3(character.transform.position.x, 0, character.transform.position.z);
                    Vector3 rotation = Vector3.RotateTowards(character.transform.forward, pc.transform.position - pos, 2 * Mathf.PI, 1);
                    character.SetDestination(pc.transform.position, rotation);
                    character.NavMeshAgent.SetDestination(pc.transform.position);
                    return;
                }
            }

        }
        character.ChangeState(character.Idle);
    }

    public override void Exit(BaseCharacter character)
    {
        character.NavMeshAgent.isStopped = true;
    }
}
