using UnityEngine;
using UnityEngine.SceneManagement; 

public class TriggerTeleportLevel : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        Debug.Log("Loading Scene");
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