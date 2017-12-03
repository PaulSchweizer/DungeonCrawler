using DungeonCrawler.Character;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Data")]
    public PlayerParty Party;

    public static InputController Instance;
    private bool _pointerIsDown;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        _pointerIsDown = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        _pointerIsDown = false;
    }

    void Update()
    {
        if (_pointerIsDown)
        {
            HandleInput();
        }
    }

    public void HandleInput()
    {
        RaycastHit hit = HitFromInput();
        if (hit.collider == null)
        {
            return;
        }
        else if (hit.collider.CompareTag("Navigation"))
        {
            HitNavigation(hit);
        }
        else if (hit.collider.CompareTag("Monster"))
        {
            HitEnemy(hit);
        }
        else if (hit.collider.CompareTag("NPC"))
        {
            HitNPC(hit);
        }
    }

    /// <summary>
    /// Raycast the user input to retrieve a possible hit.</summary>
    private RaycastHit HitFromInput()
    {
        //#if (UNITY_EDITOR || UNITY_WEBPLAYER)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //for touch device
        //#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
        //        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //#endif
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Navigation", "Enemies", "Default"));
        return hit;
    }

    private void HitNavigation(RaycastHit hit)
    {
        ApplyCharacterDestinations(Tabletop.PlayerParty, hit.point);
        for (int i = 0; i < Tabletop.PlayerParty.Length; i++)
        {
            PlayerCharacter pc = Tabletop.PlayerParty[i];
            pc.ChangeState(pc.Move);
        }
    }

    private void HitEnemy(RaycastHit hit)
    {
        BaseCharacter enemy = hit.transform.GetComponent<BaseCharacter>();
        Vector3 point = new Vector3(enemy.Character.Data.Transform.Position.X, 0, enemy.Character.Data.Transform.Position.Y); 
        for (int i = 0; i < Tabletop.PlayerParty.Length; i++)
        {
            PlayerCharacter pc = Tabletop.PlayerParty[i];
            if (!pc.Character.Data.ScheduledAttack.IsActive)
            {
                Vector3 from_pc_to_enemy = point - pc.transform.position;
                float mag = from_pc_to_enemy.magnitude;
                from_pc_to_enemy.Normalize();
                from_pc_to_enemy *= mag - pc.NavMeshAgent.radius - enemy.NavMeshAgent.radius;
                point = pc.transform.position + from_pc_to_enemy;
                ApplyCharacterDestinations(new PlayerCharacter[] { pc }, point);
                pc.ChangeState(pc.Chase);
            }
        }
    }

    private void HitNPC(RaycastHit hit)
    {
        ApplyCharacterDestinations(Tabletop.PlayerParty, hit.collider.transform.position);
        for (int i = 0; i < Tabletop.PlayerParty.Length; i++)
        {
            PlayerCharacter pc = Tabletop.PlayerParty[i];
            pc.ChangeState(pc.MoveToNPC);
            pc.DestinationNPC = hit.collider.gameObject.GetComponent<NPCCharacter>();
        }
    }

    private void ApplyCharacterDestinations(PlayerCharacter[] characters, Vector3 position)
    {
        position = new Vector3(position.x, 0, position.z);
        for (int i = 0; i < characters.Length; i++)
        {
            PlayerCharacter pc = characters[i];
            if (!pc.NavMeshAgent.isOnNavMesh)
            {
                continue;
            }

            // Angle Rotation 
            Vector3 pos = new Vector3(pc.transform.position.x, 0, pc.transform.position.z);
            Vector3 rotation = Vector3.RotateTowards(pc.transform.forward, position - pos, 2 * Mathf.PI, 1);
            pc.SetDestination(position, rotation);
        }
    }
}