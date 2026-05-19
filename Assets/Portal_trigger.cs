using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerLevel : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    void Start()
    {
        // Check 1: Does the collider exist and is it a trigger?
        Collider col = GetComponent<Collider>();
        if (col == null)
            Debug.LogError("NO COLLIDER found on " + gameObject.name);
        else if (!col.isTrigger)
            Debug.LogError("Collider IS NOT set to Trigger on " + gameObject.name);
        else
            Debug.Log("Collider OK on " + gameObject.name);

        // Check 2: Is the scene name filled in?
        if (string.IsNullOrEmpty(sceneToLoad))
            Debug.LogError("Scene name is EMPTY in the Inspector!");
        else
            Debug.Log("Scene to load: " + sceneToLoad);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check 3: Is anything entering the trigger at all?
        Debug.Log("Something entered trigger: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected, loading scene: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}