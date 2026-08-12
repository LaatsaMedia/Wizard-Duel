using UnityEngine;
using UnityEngine.UI;

public class CustomPackSelectionEntry : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button inspectButton;
    [SerializeField] private Button editButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button deleteButton;

    private CustomPackData pack;

    private PackPreviewPanel packPreviewPanel;
    private CustomPackEditor customPackEditor;
    private PackSelectionUI packSelectionUI;

    public CustomPackData Pack => pack;

    private void Awake()
    {
        if (inspectButton != null)
            inspectButton.onClick.AddListener(Inspect);

        if (editButton != null)
            editButton.onClick.AddListener(Edit);

        if (selectButton != null)
            selectButton.onClick.AddListener(Select);

        if (deleteButton != null)
            deleteButton.onClick.AddListener(Delete);
    }

    public void Setup(
        CustomPackData pack,
        PackPreviewPanel packPreviewPanel,
        CustomPackEditor customPackEditor,
        PackSelectionUI packSelectionUI)
    {
        this.pack = pack;
        this.packPreviewPanel = packPreviewPanel;
        this.customPackEditor = customPackEditor;
        this.packSelectionUI = packSelectionUI;

        Refresh();
    }

    private void Refresh()
    {
        if (selectButton != null)
        {
            selectButton.interactable =
                pack != null && pack.IsComplete;
        }
    }

    public void Inspect()
    {
        if (pack == null)
            return;

        if (packPreviewPanel == null)
        {
            Debug.LogWarning(
                "CustomPackSelectionEntry: PackPreviewPanel is not assigned.");
            return;
        }

        packPreviewPanel.OpenCustomPack(pack);
    }

    public void Edit()
    {
        if (pack == null)
            return;

        if (customPackEditor == null)
        {
            Debug.LogWarning(
                "CustomPackSelectionEntry: CustomPackEditor is not assigned.");
            return;
        }

        customPackEditor.Open(pack);
    }

    public void Select()
    {
        if (pack == null)
            return;

        if (!pack.IsComplete)
            return;

        if (packSelectionUI == null)
        {
            Debug.LogWarning(
                "CustomPackSelectionEntry: PackSelectionUI is not assigned.");
            return;
        }

        packSelectionUI.SelectCustomPack(pack);
    }

    public void Delete()
    {
        if (pack == null)
            return;

        Debug.Log(
            $"CustomPackSelectionEntry: Delete requested for {pack.packName}");

        // We will connect the actual custom-pack deletion system here.
    }
}