using UnityEngine;
using TMPro;
using UnityEngine.UI; // Required to handle the Button component

public class ButtonInteractTriggerExtraSound : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button targetButton;          // Reference to your TextMeshPro Button
    [SerializeField] private TextMeshProUGUI uiText;       // Reference to the text that should appear

    [Header("Audio References")]
    [SerializeField] private AudioSource audioSource;      // The Audio Source component that plays the sound
    [SerializeField] private AudioClip interactionSound;   // The specific audio clip to play

    [SerializeField] private AudioSource audioSource1;      // The Audio Source component that plays the sound
    [SerializeField] private AudioClip interactionSound1;
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
        // 1. Make the text visible
        if (uiText != null)
        {
            uiText.gameObject.SetActive(true);
        }

        // 2. Play the audio clip if both the source and clip are assigned
        if (audioSource != null && interactionSound != null)
        {
            audioSource.PlayOneShot(interactionSound);
        }

        if (audioSource1 != null && interactionSound1 != null)
        {
            audioSource1.PlayOneShot(interactionSound1);
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