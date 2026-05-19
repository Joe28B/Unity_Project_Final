using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

/// <summary>
/// Attach to NPC root alongside NPCDialogue.
/// - Shows prompt when XR ray hovers over the NPC
/// - T or trigger to advance dialogue
/// - E closes dialogue only (doesn't fly)
/// </summary>
[RequireComponent(typeof(NPCDialogue))]
public class NPCInteractable : MonoBehaviour
{
    [Tooltip("How close the player needs to be to interact.")]
    public float interactRadius = 5f;

    private NPCDialogue _dialogue;
    private NPCDialogueUI _ui;
    private bool _playerInRange = false;
    private bool _triggerWasPressed = false;
    private Transform _playerTransform;
    private InputDevice _rightController;
    private bool _isRayHovering = false;

    private static NPCInteractable _activeNPC = null;

    void Start()
    {
        _dialogue = GetComponent<NPCDialogue>();

        _ui = FindFirstObjectByType<NPCDialogueUI>();
        if (_ui == null)
            _ui = NPCDialogueUI.CreateUI();

        if (Camera.main != null)
            _playerTransform = Camera.main.transform;

        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, devices);
        if (devices.Count > 0)
            _rightController = devices[0];

        // Make sure we have a collider for the ray to hit
        if (GetComponentInChildren<Collider>() == null)
        {
            var col = gameObject.AddComponent<CapsuleCollider>();
            col.height = 2f;
            col.radius = 0.4f;
            col.center = new Vector3(0f, 1f, 0f);
        }
    }

    void Update()
    {
        if (_playerTransform == null) return;

        // ── Ray hover detection via camera raycast ─────────────────
        bool rayOnMe = false;
        if (Camera.main != null)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRadius * 3f))
                rayOnMe = (hit.collider != null && hit.collider.GetComponentInParent<NPCInteractable>() == this);
        }

        // ── Distance check ─────────────────────────────────────────
        float dist = Vector3.Distance(transform.position, _playerTransform.position);
        bool inRange = dist <= interactRadius;

        // Another NPC is active — stay silent
        if (_activeNPC != null && _activeNPC != this)
        {
            if (_playerInRange) ExitRange();
            return;
        }

        // Enter range by proximity OR ray hover
        if ((inRange || rayOnMe) && !_playerInRange)
            EnterRange();
        else if (!inRange && !rayOnMe && _playerInRange)
            ExitRange();

        // Update prompt if ray just started/stopped hovering
        if (rayOnMe != _isRayHovering && _playerInRange && !_ui.IsOpen)
        {
            _isRayHovering = rayOnMe;
            _ui.ShowPrompt($"Press T to talk to {_dialogue.GetNPCName()}");
        }

        if (_playerInRange)
            HandleInput();
    }

    void EnterRange()
    {
        _playerInRange = true;
        _activeNPC = this;
        // Show NPC name immediately in the prompt
        _ui.ShowPrompt($"Press T to talk to {_dialogue.GetNPCName()}");
    }

    void ExitRange()
    {
        _playerInRange = false;
        _isRayHovering = false;
        if (_activeNPC == this) _activeNPC = null;
        _dialogue.ResetDialogue();
        _ui.Hide();
    }

    void HandleInput()
    {
        var keyboard = UnityEngine.InputSystem.Keyboard.current;

        // ── E key: close dialogue only, consume the event ─────────
        if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
        {
            ExitRange();
            return; // don't let E propagate to XRI locomotion
        }

        // ── Trigger / T key: advance dialogue ─────────────────────
        bool triggerPressed = false;

        if (_rightController.isValid)
            _rightController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed);

        if (keyboard != null && keyboard.tKey.wasPressedThisFrame)
        {
            triggerPressed = true;
            _triggerWasPressed = false;
        }

        if (triggerPressed && !_triggerWasPressed)
        {
            _triggerWasPressed = true;
            string line = _dialogue.GetNextLine();
            if (line != null)
                _ui.ShowLine(_dialogue.GetNPCName(), line);
            else
            {
                _ui.Hide();
                _dialogue.ResetDialogue();
                _activeNPC = null;
                _playerInRange = false;
            }
        }
        else if (!triggerPressed)
        {
            _triggerWasPressed = false;
        }
    }
}
