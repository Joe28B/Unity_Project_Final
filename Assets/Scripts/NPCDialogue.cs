using UnityEngine;

/// <summary>
/// Attach to an NPC GameObject. Holds a set of Zodiac Killer dialogue lines.
/// When the player clicks the NPC, NPCDialogueUI displays the next line.
/// </summary>
public class NPCDialogue : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName = "Witness";

    [TextArea(2, 6)]
    public string[] dialogueLines = new string[]
    {
        "The Zodiac Killer was an unidentified serial killer active in Northern California in the late 1960s and early 1970s.",
        "He is confirmed to have killed at least five people, though he claimed 37 victims in his taunting letters to police.",
        "The Zodiac sent a series of cryptic ciphers to newspapers. Only one of the four ciphers — the Z408 — has ever been fully solved.",
        "His victims were attacked in Benicia, Vallejo, Lake Berryessa, and San Francisco between December 1968 and October 1969.",
        "The killer gave himself the name 'Zodiac' in a letter to the San Francisco Chronicle in August 1969.",
        "Despite extensive investigation, the Zodiac's identity has never been officially confirmed. The case remains open.",
        "Arthur Leigh Allen was long considered the prime suspect by many investigators, but DNA and fingerprint evidence never conclusively tied him to the crimes.",
        "The Zodiac's last confirmed letter was sent in 1974, containing a reference to the film 'The Exorcist'. He was never heard from again.",
    };

    private int _lineIndex = 0;

    /// <summary>Returns the next dialogue line (cycles through all lines).</summary>
    public string GetNextLine()
    {
        string line = dialogueLines[_lineIndex];
        _lineIndex = (_lineIndex + 1) % dialogueLines.Length;
        return line;
    }

    public string GetNPCName() => npcName;

    /// <summary>Resets dialogue back to the first line.</summary>
    public void ResetDialogue() => _lineIndex = 0;
}
