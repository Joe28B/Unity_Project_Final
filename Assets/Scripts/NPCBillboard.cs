using UnityEngine;

/// <summary>
/// Makes a GameObject always face the main camera (used for NPC name labels).
/// </summary>
public class NPCBillboard : MonoBehaviour
{
    private Transform _cam;

    void Awake()
    {
        if (Camera.main != null)
            _cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (_cam == null) return;
        transform.LookAt(transform.position + _cam.forward);
    }
}
