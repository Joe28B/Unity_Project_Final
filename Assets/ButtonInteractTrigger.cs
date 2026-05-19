using UnityEngine;
using TMPro;
using UnityEngine.UI; // Required to handle the Button component

public class UmarButtonInteractTrigger : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button targetButton;          // Reference to your TextMeshPro Button
    [SerializeField] private TextMeshProUGUI uiText;       // Reference to the text that should appear

    [Header("Audio References")]
    [SerializeField] private AudioSource audioSource;      // The Audio Source component that plays the sound
    [SerializeField] private AudioClip interactionSound;   // The specific audio clip to play

    void Start()
    {
        // 1. Ensure the text starts completely invisible/hidden
        if (uiText != null)
        {
            uiText.gameObject.SetActive(false);
        }

        // 2. Automatically listen for the button click without needing manual Inspector events
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogWarning("Target Button reference is missing on " + gameObject.name);
        }
    }

    private void OnButtonClicked()
    {
        // Toggle text visibility
        if (uiText != null)
        {
            bool isActive = uiText.gameObject.activeSelf;
            uiText.gameObject.SetActive(!isActive);
        }

        // Toggle audio play/stop
        if (audioSource != null && interactionSound != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            else
            {
                audioSource.PlayOneShot(interactionSound);
            }
        }
    }
    private void OnDestroy()
    {
        // Clean up the listener when this object is destroyed to avoid memory leaks
        if (targetButton != null)
        {
            targetButton.onClick.RemoveListener(OnButtonClicked);
        }
    }
}