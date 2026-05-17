using UnityEngine;
// 1. You must include this to handle scene switching
using UnityEngine.SceneManagement; 

public class TriggerLevel : MonoBehaviour
{
    // The name of the scene you want to load (set this in the Unity Inspector)
    [SerializeField] private string sceneToLoad;

    // Use this if your game is 3D
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    // Use this instead if your game is 2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        // Make sure you've typed the scene name correctly in the Inspector
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene to load is not specified in the Inspector!");
        }
    }
}