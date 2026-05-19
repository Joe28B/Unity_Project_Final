using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Dialogue box UI. Creates itself — no scene setup needed.
/// Call NPCDialogueUI.CreateUI() or let NPCClickInteract do it automatically.
/// </summary>
public class NPCDialogueUI : MonoBehaviour
{
    public bool IsOpen { get; private set; }

    private GameObject _panel;
    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _bodyText;

    // ── Static factory ────────────────────────────────────────────
    public static NPCDialogueUI CreateUI()
    {
        var go = new GameObject("[DialogueUI]");
        DontDestroyOnLoad(go);
        return go.AddComponent<NPCDialogueUI>();
    }

    void Awake() => BuildCanvas();

    void BuildCanvas()
    {
        // ── Canvas ────────────────────────────────────────────────
        var canvasGO = new GameObject("Canvas");
        canvasGO.transform.SetParent(transform);
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        canvasGO.AddComponent<CanvasScaler>().uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGO.AddComponent<GraphicRaycaster>();

        // ── Panel (dark box at bottom of screen) ──────────────────
        var panelGO = new GameObject("Panel");
        panelGO.transform.SetParent(canvasGO.transform, false);
        _panel = panelGO;

        var panelImg = panelGO.AddComponent<Image>();
        panelImg.color = new Color(0.05f, 0.05f, 0.1f, 0.92f);

        var panelRT = panelGO.GetComponent<RectTransform>();
        panelRT.anchorMin = new Vector2(0.1f, 0.02f);
        panelRT.anchorMax = new Vector2(0.9f, 0.28f);
        panelRT.offsetMin = Vector2.zero;
        panelRT.offsetMax = Vector2.zero;

        // ── Accent bar (top of panel) ─────────────────────────────
        var barGO = new GameObject("AccentBar");
        barGO.transform.SetParent(panelGO.transform, false);
        var barImg = barGO.AddComponent<Image>();
        barImg.color = new Color(0.8f, 0.2f, 0.2f, 1f);
        var barRT = barGO.GetComponent<RectTransform>();
        barRT.anchorMin = new Vector2(0f, 1f);
        barRT.anchorMax = new Vector2(1f, 1f);
        barRT.offsetMin = new Vector2(0, -4);
        barRT.offsetMax = Vector2.zero;

        // ── NPC Name ──────────────────────────────────────────────
        var nameGO = new GameObject("NPCName");
        nameGO.transform.SetParent(panelGO.transform, false);
        _nameText = nameGO.AddComponent<TextMeshProUGUI>();
        _nameText.fontSize   = 22;
        _nameText.fontStyle  = FontStyles.Bold;
        _nameText.color      = new Color(0.9f, 0.7f, 0.3f);
        _nameText.text       = "";

        var nameRT = nameGO.GetComponent<RectTransform>();
        nameRT.anchorMin = new Vector2(0f, 0.72f);
        nameRT.anchorMax = new Vector2(1f, 1f);
        nameRT.offsetMin = new Vector2(20, 0);
        nameRT.offsetMax = new Vector2(-20, -8);

        // ── Dialogue Body ─────────────────────────────────────────
        var bodyGO = new GameObject("DialogueBody");
        bodyGO.transform.SetParent(panelGO.transform, false);
        _bodyText = bodyGO.AddComponent<TextMeshProUGUI>();
        _bodyText.fontSize  = 17;
        _bodyText.color     = Color.white;
        _bodyText.text      = "";

        var bodyRT = bodyGO.GetComponent<RectTransform>();
        bodyRT.anchorMin = new Vector2(0f, 0f);
        bodyRT.anchorMax = new Vector2(1f, 0.72f);
        bodyRT.offsetMin = new Vector2(20, 10);
        bodyRT.offsetMax = new Vector2(-20, -5);

        // ── Hint text ─────────────────────────────────────────────
        var hintGO = new GameObject("Hint");
        hintGO.transform.SetParent(panelGO.transform, false);
        var hintText = hintGO.AddComponent<TextMeshProUGUI>();
        hintText.fontSize  = 12;
        hintText.color     = new Color(1f, 1f, 1f, 0.4f);
        hintText.text      = "[Click] Next    [E] Close";
        hintText.alignment = TextAlignmentOptions.BottomRight;

        var hintRT = hintGO.GetComponent<RectTransform>();
        hintRT.anchorMin = new Vector2(0f, 0f);
        hintRT.anchorMax = new Vector2(1f, 0.3f);
        hintRT.offsetMin = new Vector2(0, 0);
        hintRT.offsetMax = new Vector2(-12, 0);

        Hide();
    }

    // ── Public API ────────────────────────────────────────────────
    public void ShowLine(string npcName, string line)
    {
        _nameText.text = npcName;
        _bodyText.text = line;
        _panel.SetActive(true);
        IsOpen = true;
    }

    public void Hide()
    {
        if (_panel != null) _panel.SetActive(false);
        IsOpen = false;
    }
}
