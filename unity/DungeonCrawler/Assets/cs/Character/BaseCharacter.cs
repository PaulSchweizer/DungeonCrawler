using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DungeonCrawler.Character;
using DungeonCrawler.Core;
using UnityEngine.UI;
using DungeonCrawler.Utility;

public class BaseCharacter : MonoBehaviour
{
    [Header("Data")]
    public CharacterData Character;

    [Header("References")]
    public UnityEngine.AI.NavMeshAgent NavMeshAgent;

    [HideInInspector]
    public Vector3 DestinationPosition;
    [HideInInspector]
    public Vector3 DestinationRotation;
    [HideInInspector]
    public NPCCharacter DestinationNPC;

    [Header("Attributes")]
    public bool isLoot;

    public CharacterState Idle = IdleState.Instance;
    public CharacterState Move = MoveState.Instance;
    public CharacterState Chase = ChaseState.Instance;
    public CharacterState Attack = AttackState.Instance;
    public CharacterState TakenOut = TakenOutState.Instance;
    public CharacterState MoveToNPC = MoveToNPCState.Instance;
    public CharacterState Conversation = ConversationState.Instance;
    protected CharacterState CurrentState;

    [Header("UI")]
    public Slider PhysicalStressSlider;
    public Slider AttackSlider;

    public virtual void Awake()
    {
        // Unity 
        tag = Character.Data.Type;
        CurrentState = Idle;
        NavMeshAgent.radius = Character.Data.Radius;
        ResetUI();

        // Connect Events
        Character.Data.OnPhysicalStressChanged += new PhysicalStressChangedHandler(PhysicalStressChanged);
        Character.Data.OnAttackScheduled += new AttackScheduledHandler(AttackScheduled);
        Character.Data.OnTakenOut += new TakenOutHandler(GotTakenOut);
    }

    private void Start()
    {
        NavMeshAgent.enabled = true;
        NavMeshAgent.updateRotation = true;
    }

    public virtual void Update()
    {
        Character.Data.Transform.Position.X = transform.position.x;
        Character.Data.Transform.Position.Y = transform.position.z;
        Character.Data.Transform.Rotation = (float)((Math.PI / 180f) * (transform.eulerAngles.y - 90));
        CurrentState.Update(this);
    }

    public void ChangeState(CharacterState state)
    {
        if (state != CurrentState)
        {
            CurrentState.Exit(this);
            CurrentState = state;
            CurrentState.Enter(this);
        }
    }

    public void SetDestination(Vector3 position, Vector3 rotation)
    {
        DestinationPosition = position;
        DestinationRotation = rotation;
        NavMeshAgent.SetDestination(position);
    }

    public void DropLoot()
    {
        tag = "Navigation";
        isLoot = true;
    }

    /// <summary>
    /// Collect the Loot on collision.</summary>
    private void OnTriggerEnter(Collider other)
    {
        Looted(other);
    }

    private void OnTriggerStay(Collider other)
    {
        Looted(other);
    }

    private void Looted(Collider other)
    {
        if (isLoot)
        {
            if (other.gameObject.tag == "Player")
            {
                BaseCharacter character = other.gameObject.GetComponent<BaseCharacter>();
                character.Character.Data.Inventory += Character.Data.Inventory;
                isLoot = false;
                gameObject.SetActive(false);
            }
        }
    }

    #region Debug

#if UNITY_EDITOR

    public Color DebugColor;

    void OnDrawGizmos()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            Color oldColor = Gizmos.color;

            // Position
            Gizmos.color = DebugColor;
            Vector3 center = new Vector3(Character.Data.Transform.Position.X, 0, Character.Data.Transform.Position.Y);
            Vector3 size = new Vector3(1, 0.01f, 1);
            Gizmos.DrawCube(center, size);

            // AttackShape
            Gizmos.color = Color.red;
            foreach(AttackShapeMarker shape in Character.Data.AttackShape)
            {
                Vector mapped = Character.Data.Transform.Map(shape.Transform.Position);
                center = new Vector3(mapped.X, 0.01f, mapped.Y);
                size = new Vector3(1, 0.01f, 1);
                DebugExtension.DebugCircle(center, Gizmos.color, shape.Radius);

                //shape.Transform.Rotation
                float angle = shape.Angle * Mathf.Rad2Deg;
                Vector3 to = Quaternion.AngleAxis(angle / 2 + Character.Data.Transform.Rotation * Mathf.Rad2Deg, Vector3.up) * Vector3.left;
                Gizmos.DrawLine(center, center + to);
                to = Quaternion.AngleAxis(-angle / 2 + Character.Data.Transform.Rotation * Mathf.Rad2Deg, Vector3.up) * Vector3.left;
                Gizmos.DrawLine(center, center + to);

                // Forward
                shape.Apply(Character.Data.Transform.Rotation);
                to = new Vector3(shape.Forward[0], 0, shape.Forward[1]);
                Gizmos.DrawLine(center, center + to);
            }

            // AlertnessRadius
            DebugExtension.DebugCircle(transform.position, Gizmos.color, Character.Data.AlertnessRadius);

            // States
            DebugExtension.DebugCircle(transform.position, CurrentState.DebugColor, NavMeshAgent.radius);

            Gizmos.color = oldColor;
        }
    }

    #endif

    #endregion

    #region Events and Connections to CharacterData

    private void ResetUI()
    {
        PhysicalStressSlider.maxValue = Character.Data.PhysicalStress.MaxValue;
        PhysicalStressSlider.minValue = Character.Data.PhysicalStress.MinValue;
        PhysicalStressSlider.value = Character.Data.PhysicalStress.Value;
        AttackSlider.value = 0;
    }

    private void PhysicalStressChanged(object sender, EventArgs e)
    {
        PhysicalStressSlider.value = Character.Data.PhysicalStress.Value;
    }

    private void AttackScheduled(object sender, EventArgs e)
    {
        AttackSlider.minValue = 0;
        AttackSlider.maxValue = (sender as Character).ScheduledAttack.TotalTime;
        AttackSlider.value = 0;
        AttackSlider.fillRect.GetComponent<Image>().color = new Color(1f, 1f, 0f);
    }

    private void GotTakenOut(object sender, EventArgs e)
    {
        ChangeState(TakenOut);
    }

    #endregion
}
