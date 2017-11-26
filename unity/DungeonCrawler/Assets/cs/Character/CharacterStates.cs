using System;
using DungeonCrawler.Character;
using DungeonCrawler.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterState
{
    public Color DebugColor = new Color(1, 1, 1, 1);
    public virtual void Enter(BaseCharacter character) { }
    public abstract void Update(BaseCharacter character);
    public virtual void Exit(BaseCharacter character) { }
}

public class IdleState : CharacterState
{
    public static IdleState Instance = new IdleState();

    public IdleState()
    {
        DebugColor = Color.green;
    }

    public override void Enter(BaseCharacter character)
    {
        character.NavMeshAgent.isStopped = true;
    }

    public override void Update(BaseCharacter character)
    {
    }

    public override void Exit(BaseCharacter character)
    {
        character.NavMeshAgent.isStopped = false;
    }
}

public class MoveState : CharacterState
{
    public static MoveState Instance = new MoveState();

    public MoveState()
    {
        DebugColor = Color.blue;
    }

    public override void Enter(BaseCharacter character)
    {
        character.NavMeshAgent.isStopped = false;
    }

    public override void Update(BaseCharacter character)
    {
        if (character.NavMeshAgent.pathPending)
        {
            return;
        }

        if (character.NavMeshAgent.remainingDistance <= character.NavMeshAgent.stoppingDistance)
        {
            if (Vector3.Angle(character.transform.forward, character.DestinationRotation) < 0.1)
            {
                character.ChangeState(character.Idle);
            }
            else
            {
                float step = (character.NavMeshAgent.angularSpeed * Time.deltaTime * Mathf.PI) / 180;
                Vector3 newDir = Vector3.RotateTowards(character.transform.forward, character.DestinationRotation, step, 0);
                character.transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }

    public override void Exit(BaseCharacter character)
    {
        character.NavMeshAgent.isStopped = false;
    }
}

public class ChaseState : CharacterState
{

    private float rotationThreshold = 4f;

    public static ChaseState Instance = new ChaseState();

    public ChaseState()
    {
        DebugColor = Color.magenta;
    }

    public override void Enter(BaseCharacter character)
    {
        character.NavMeshAgent.enabled = true;
    }

    public override void Update(BaseCharacter character)
    {
        if (character.NavMeshAgent.pathPending)
        {
            return;
        }

        if (character.NavMeshAgent.remainingDistance <= character.NavMeshAgent.stoppingDistance + character.NavMeshAgent.radius * 2)
        {
            if (Vector3.Angle(character.transform.forward, character.DestinationRotation) < rotationThreshold)
            {
                if (character.CharacterData.EnemiesInAttackShape().Length > 0)
                {
                    character.ChangeState(character.Attack);
                    return;
                }
                character.ChangeState(character.Idle);
            }
            if (character.NavMeshAgent.remainingDistance <= character.NavMeshAgent.stoppingDistance)
            {
                float step = (float)((character.NavMeshAgent.angularSpeed * Time.deltaTime * Math.PI) / 180);
                Vector3 newDir = Vector3.RotateTowards(character.transform.forward, character.DestinationRotation, step, 0);
                character.transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }

    public override void Exit(BaseCharacter character)
    {
        character.NavMeshAgent.isStopped = true;
    }
}

public class AttackState : CharacterState
{
    public static readonly AttackState Instance = new AttackState();

    public AttackState()
    {
        DebugColor = Color.red;
    }

    public override void Enter(BaseCharacter character)
    {
        character.NavMeshAgent.isStopped = true;
    }

    public override void Exit(BaseCharacter character)
    {
        character.CharacterData.ScheduledAttack.Stop();
        character.AttackSlider.value = 0;
    }

    public override void Update(BaseCharacter character)
    {
        if (character.CharacterData.ScheduledAttack.IsActive)
        {
            character.CharacterData.ScheduledAttack.CurrentTime += Time.deltaTime;
            character.AttackSlider.value = character.CharacterData.ScheduledAttack.CurrentTime;
            if (character.CharacterData.ScheduledAttack.Progress() >= 0.5 && 
                !character.CharacterData.ScheduledAttack.HitOccurred)
            {
                character.CharacterData.ScheduledAttack.Hit();
                character.AttackSlider.fillRect.GetComponent<Image>().color = new Color(0f, 0f, 1f);
            }
            else if (character.CharacterData.ScheduledAttack.Progress() >= 1)
            {
                character.ChangeState(character.Idle);
            }
        }
        else
        {
            character.CharacterData.ScheduleAttack();
        }
    }
}

public class TakenOutState : CharacterState
{
    public static TakenOutState Instance = new TakenOutState();

    public TakenOutState()
    {
        DebugColor = Color.grey;
    }

    public override void Enter(BaseCharacter character)
    {
        character.NavMeshAgent.enabled = false;
        character.DropLoot();
        GameMaster.DeRegisterCharacter(character.CharacterData);
        character.tag = "Untagged";
    }

    public override void Update(BaseCharacter character) { }

    public override void Exit(BaseCharacter character) { }
}

public class MoveToNPCState : CharacterState
{
    public static MoveToNPCState Instance = new MoveToNPCState();

    public MoveToNPCState()
    {
        DebugColor = Color.blue;
    }

    public override void Enter(BaseCharacter character)
    {
        character.NavMeshAgent.isStopped = false;
    }

    public override void Update(BaseCharacter character)
    {
        if (character.NavMeshAgent.pathPending)
        {
            return;
        }

        if (character.NavMeshAgent.remainingDistance <= character.NavMeshAgent.stoppingDistance)
        {
            if (Vector3.Angle(character.transform.forward, character.DestinationRotation) < 0.1)
            {
                character.ChangeState(character.Conversation);
            }
            else
            {
                Vector3 pos = new Vector3(character.transform.position.x, 0, character.transform.position.z);
                Vector3 rotation = Vector3.RotateTowards(character.transform.forward, character.DestinationPosition - pos, 2 * Mathf.PI, 1);
                character.SetDestination(character.DestinationPosition, rotation);

                float step = (float)((character.NavMeshAgent.angularSpeed * Time.deltaTime * Math.PI) / 180);
                Vector3 newDir = Vector3.RotateTowards(character.transform.forward, character.DestinationRotation, step, 0);
                character.transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }

    public override void Exit(BaseCharacter character)
    {
        character.NavMeshAgent.isStopped = false;
    }
}

public class ConversationState : CharacterState
{
    public static ConversationState Instance = new ConversationState();

    public ConversationState()
    {
        DebugColor = Color.white;
    }

    public override void Enter(BaseCharacter character)
    {
        character.NavMeshAgent.isStopped = true;
        ConversationUI.Instance.Open(character.DestinationNPC.InkStory);
    }

    public override void Update(BaseCharacter character)
    {
    }

    public override void Exit(BaseCharacter character)
    {
        character.DestinationNPC = null;
    }
}