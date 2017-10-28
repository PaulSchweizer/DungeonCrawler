using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DungeonCrawler.Character;
using DungeonCrawler.Core;
using UnityEngine.UI;

public class BaseCharacter : MonoBehaviour
{
    public TextAsset JsonFile;
    public Character CharacterData;
    public UnityEngine.AI.NavMeshAgent NavMeshAgent;

    [HideInInspector]
    public Vector3 DestinationPosition;
    [HideInInspector]
    public Vector3 DestinationRotation;
    [HideInInspector]
    public NPCCharacter DestinationNPC;

    private bool isLoot;

    public CharacterState Idle = IdleState.Instance;
    public CharacterState Move = MoveState.Instance;
    public CharacterState Chase = ChaseState.Instance;
    public CharacterState Attack = AttackState.Instance;
    public CharacterState TakenOut = TakenOutState.Instance;
    public CharacterState MoveToNPC = MoveToNPCState.Instance;
    public CharacterState Conversation = ConversationState.Instance;
    protected CharacterState CurrentState;

    // Additional Character Attributes
    public int AlertnessRadius = 0;

    // UI
    public Slider PhysicalStressSlider;
    public Slider AttackSlider;

    public virtual void Awake()
    {
        CharacterData = Character.DeserializeFromJson(JsonFile.text);
        GameMaster.RegisterCharacter(CharacterData);

        // Unity 
        tag = CharacterData.Type;
        CurrentState = Idle;
        ResetUI();

        // Connect Events
        CharacterData.OnPhysicalStressChanged += new PhysicalStressChangedHandler(PhysicalStressChanged);
        CharacterData.OnAttackScheduled += new AttackScheduledHandler(AttackScheduled);
        CharacterData.OnTakenOut += new TakenOutHandler(GotTakenOut);
    }

    private void Start()
    {
        NavMeshAgent.enabled = true;
        NavMeshAgent.updateRotation = true;
    }

    private void Update()
    {
        CharacterData.Transform.Position.X = Mathf.RoundToInt(transform.position.x);
        CharacterData.Transform.Position.Y = Mathf.RoundToInt(transform.position.z);
        CharacterData.Transform.Rotation = (float)((Math.PI / 180f) * (transform.eulerAngles.y - 67.5));
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
                character.CharacterData.Inventory += CharacterData.Inventory;
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
            Vector3 center = new Vector3(CharacterData.Transform.Position.X, 0, CharacterData.Transform.Position.Y);
            Vector3 size = new Vector3(1, 0.01f, 1);
            Gizmos.DrawCube(center, size);

            // AttackShape
            Gizmos.color = Color.red;
            foreach(int[] shape in CharacterData.AttackShape)
            {
                int[] mapped = CharacterData.Transform.Map(shape);
                center = new Vector3(mapped[0], 0, mapped[1]);
                size = new Vector3(1, 0.01f, 1);
                Gizmos.DrawCube(center, size);
            }

            // AlertnessRadius
            DebugExtension.DebugCircle(transform.position, Gizmos.color, AlertnessRadius);

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
        PhysicalStressSlider.maxValue = CharacterData.PhysicalStress.MaxValue;
        PhysicalStressSlider.minValue = CharacterData.PhysicalStress.MinValue;
        PhysicalStressSlider.value = CharacterData.PhysicalStress.Value;
        AttackSlider.value = 0;
    }

    private void PhysicalStressChanged(object sender, EventArgs e)
    {
        PhysicalStressSlider.value = CharacterData.PhysicalStress.Value;
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
