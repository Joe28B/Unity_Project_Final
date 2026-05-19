using UnityEngine;

/// <summary>
/// Attach to an empty GameObject to build a simple humanoid NPC body from primitives.
/// One NPC per GameObject — duplicate the GameObject to make more.
/// Click the NPC to trigger Zodiac Killer dialogue via NPCClickInteract.
/// 
/// Body is made of: head (sphere), torso (cube), 2 arms (cubes), 2 legs (cubes).
/// A single CapsuleCollider on the root handles click detection.
/// </summary>
public class NPCHumanoid : MonoBehaviour
{
    [Header("Appearance")]
    public Color skinColor  = new Color(0.8f, 0.6f, 0.5f);
    public Color shirtColor = new Color(0.2f, 0.3f, 0.7f);
    public Color pantsColor = new Color(0.15f, 0.15f, 0.25f);

    [Header("Dialogue")]
    public string npcName = "Witness";

    void Awake()
    {
        BuildBody();
        SetupCollider();
        SetupDialogue();
    }

    void BuildBody()
    {
        // All Y values are local, feet at Y=0

        // --- Torso ---
        CreatePart("Torso", shirtColor, new Vector3(0f, 1.1f, 0f), new Vector3(0.5f, 0.6f, 0.28f));

        // --- Head ---
        CreatePart("Head", skinColor, new Vector3(0f, 1.75f, 0f), new Vector3(0.28f, 0.28f, 0.28f));

        // --- Left Arm ---
        CreatePart("ArmL", shirtColor, new Vector3(-0.38f, 1.1f, 0f), new Vector3(0.16f, 0.55f, 0.16f));

        // --- Right Arm ---
        CreatePart("ArmR", shirtColor, new Vector3( 0.38f, 1.1f, 0f), new Vector3(0.16f, 0.55f, 0.16f));

        // --- Left Leg ---
        CreatePart("LegL", pantsColor, new Vector3(-0.13f, 0.38f, 0f), new Vector3(0.18f, 0.6f, 0.18f));

        // --- Right Leg ---
        CreatePart("LegR", pantsColor, new Vector3( 0.13f, 0.38f, 0f), new Vector3(0.18f, 0.6f, 0.18f));
    }

    void CreatePart(string partName, Color color, Vector3 localPos, Vector3 localScale)
    {
        var part = GameObject.CreatePrimitive(PrimitiveType.Cube);
        part.name = partName;
        part.transform.SetParent(transform);
        part.transform.localPosition = localPos;
        part.transform.localScale    = localScale;
        part.transform.localRotation = Quaternion.identity;

        // Remove individual colliders — root collider handles everything
        Destroy(part.GetComponent<Collider>());

        // Apply colour
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit")
                               ?? Shader.Find("Standard"));
        mat.color = color;
        part.GetComponent<Renderer>().material = mat;
    }

    void SetupCollider()
    {
        // Single collider on root covering the full body height
        var col = gameObject.AddComponent<CapsuleCollider>();
        col.height = 2f;
        col.radius = 0.3f;
        col.center = new Vector3(0f, 1f, 0f);
    }

    void SetupDialogue()
    {
        var dialogue = gameObject.AddComponent<NPCDialogue>();
        dialogue.npcName = npcName;
    }
}
