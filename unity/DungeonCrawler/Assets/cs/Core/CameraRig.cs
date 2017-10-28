using UnityEngine;

/// <summary>
/// Keep the Camera aimed and focused on the PlayerCharacter.</summary>
[ExecuteInEditMode]
public class CameraRig : MonoBehaviour
{
    public static CameraRig Instance;
    public Camera Camera;
    public Transform Target;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (Target == null)
        {
            return;
        }
        transform.position = Target.position;
        Camera.transform.LookAt(Target);
    }
}
