using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Attach to your XR Origin.
/// Walk up to an NPC and press F to interact.
/// Press F again to advance lines. Press E to close.
/// </summary>
public class NPCClickInteract : MonoBehaviour
{
    [Tooltip("How close you need to be to interact (in units).")]
    public float interactRange = 5f;

    private Camera _cam;
    private NPCDialogueUI _ui;
    private NPCDialogue _currentNPC;

    void Start()
    {
        _cam = Camera.main;

        if (_cam == null)
            Debug.LogError("[NPCInteract] No Main Camera found! Tag your XR camera as MainCamera.");
        else
            Debug.Log("[NPCInteract] Ready. Walk up to an NPC and press F.");

        _ui = FindFirstObjectByType<NPCDialogueUI>();
        if (_ui == null)
            _ui = NPCDialogueUI.CreateUI();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // F = interact / next line
        if (Keyboard.current.fKey.wasPressedThisFrame)
            TryInteract();

        // E = close
        if (Keyboard.current.eKey.wasPressedThisFrame)
            CloseDialogue();
    }

    void TryInteract()
    {
        // If already talking to someone, just advance the line
        if (_currentNPC != null && _ui.IsOpen)
        {
            _ui.ShowLine(_currentNPC.GetNPCName(), _currentNPC.GetNextLine());
            return;
        }

        // Find the closest NPC within range
        NPCDialogue closest = null;
        float closestDist = interactRange;

        foreach (var npc in FindObjectsByType<NPCDialogue>(FindObjectsSortMode.None))
        {
            float dist = Vector3.Distance(transform.position, npc.transform.position);
            Debug.Log($"[NPCInteract] NPC '{npc.GetNPCName()}' is {dist:F1} units away.");
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = npc;
            }
        }

        if (closest != null)
        {
            Debug.Log($"[NPCInteract] Talking to {closest.GetNPCName()}");
            _currentNPC = closest;
            _ui.ShowLine(closest.GetNPCName(), closest.GetNextLine());
        }
        else
        {
            Debug.Log($"[NPCInteract] No NPC within {interactRange} units. Get closer and press F.");
        }
    }

    void CloseDialogue()
    {
        _currentNPC?.ResetDialogue();
        _currentNPC = null;
        _ui.Hide();
    }
}
